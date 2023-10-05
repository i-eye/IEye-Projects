using Moonstorm;
using RoR2;
using RoR2.Items;
using R2API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R2API.Networking;

namespace IEye.RRP.Items
{
    public class PredatorySavagery : ItemBase
    {
        public const string token = "RRP_ITEM_PREDATORY_DESC";
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("PredatorySavagery", RRPBundle.Items);


        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Armor with rush per stack(default 15).")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static int armor = 15;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percent damage increase per stack(default 15%).")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float damage = 15f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Pecent global cooldown reduction per stack(default 5%)(caps at 50%).")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float cooldownReduction = 5f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Added percentage jump force(regardless of stack)(default 35%).")]
        [TokenModifier(token, StatTypes.Default, 3)]
        public static float jumpMult = 35f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Added crit damage per stack(default 10%).")]
        [TokenModifier(token, StatTypes.Default, 4)]
        public static float critDamage = 10f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Added crit chance(regardless of stack)(default 6%).")]
        [TokenModifier(token, StatTypes.Default, 5)]
        public static float critChance = 6f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base duration of the rush(default 10s).")]
        [TokenModifier(token, StatTypes.Default, 6)]
        public static float duration = 10f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Duration added on stack(default 3s).")]
        [TokenModifier(token, StatTypes.Default, 7)]
        public static float stackDuration = 3f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percent chance of rush on hit(default 2%).")]
        [TokenModifier(token, StatTypes.Default, 8)]
        public static float hitChance = 2f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percent chance of rush on kill(default 8%).")]
        [TokenModifier(token, StatTypes.Default, 9)]
        public static float killChance = 8f;


        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver, IOnKilledOtherServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.PredatorySavagery;

            public void OnDamageDealtServer(DamageReport report)
            {
                //DefNotSS2Log.Message("Damage Dealt");
                if(Util.CheckRoll(hitChance * report.damageInfo.procCoefficient, report.attackerMaster))
                {
                    RRPMain.logger.LogMessage("Roll passed");
                    report.attackerBody.AddTimedBuffAuthority(RRPContent.Buffs.PredatoryRush.buffIndex, report.damageInfo.procCoefficient * (duration + (stackDuration * stack)));
                }
            }


            public void OnKilledOtherServer(DamageReport report)
            {
                if (Util.CheckRoll(killChance, report.attackerMaster))
                {
                    report.attackerBody.AddTimedBuffAuthority(RRPContent.Buffs.PredatoryRush.buffIndex, duration + (stackDuration * stack));
                }
            }

            private void Start()
            {
                if (body.inventory.GetItemCount(RRPContent.Items.SacrificialHelper) == 0)
                {
                    body.inventory.GiveItem(RRPContent.Items.SacrificialHelper);
                }
            }
        }
    }
}
