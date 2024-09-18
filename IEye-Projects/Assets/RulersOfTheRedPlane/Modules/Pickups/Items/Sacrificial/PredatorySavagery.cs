using MSU;
using MSU.Config;
using RoR2;
using RoR2.Items;
using R2API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R2API.Networking;
using RoR2.ContentManagement;
using static MSU.BaseBuffBehaviour;

namespace IEye.RRP.Items
{
    public class PredatorySavagery : RRPItem
    {
        public const string token = "RRP_ITEM_PREDATORY_DESC";
        //public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("PredatorySavagery", RRPBundle.Items);

        public override RRPAssetRequest AssetRequest => RRPAssets.LoadAssetAsync<ItemAssetCollection>("acSavagery", RRPBundle.Items);

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Armor with rush per stack(default 15).")]
        [FormatToken(token, opType:default, 0)]
        public static int armor = 15;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Percent damage increase per stack(default 15%).")]
        [FormatToken(token, opType:default, 1)]
        public static float damage = 15f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Pecent global cooldown reduction per stack(default 5%)(caps at 50%).")]
        [FormatToken(token, opType:default, 2)]
        public static float cooldownReduction = 5f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Added percentage jump force(regardless of stack)(default 35%).")]
        [FormatToken(token, opType:default, 3)]
        public static float jumpMult = 35f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Added crit damage per stack(default 10%).")]
        [FormatToken(token, opType:default, 4)]
        public static float critDamage = 10f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Added crit chance(regardless of stack)(default 6%).")]
        [FormatToken(token, opType:default, 5)]
        public static float critChance = 6f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Base duration of the rush(default 10s).")]
        [FormatToken(token, opType:default, 6)]
        public static float duration = 10f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Duration added on stack(default 3s).")]
        [FormatToken(token, opType:default, 7)]
        public static float stackDuration = 3f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Percent chance of rush on hit(default 2%).")]
        [FormatToken(token, opType:default, 8)]
        public static float hitChance = 2f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, ConfigDescOverride = "Percent chance of rush on kill(default 8%).")]
        [FormatToken(token, opType:default, 9)]
        public static float killChance = 8f;


        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver, IOnKilledOtherServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.PredatorySavagery;

            public void OnDamageDealtServer(DamageReport report)
            {
                //DefNotRRPLog.Message("Damage Dealt");
                if(Util.CheckRoll(hitChance * report.damageInfo.procCoefficient, report.attackerMaster))
                {
                    RRPLog.Message("Roll passed");
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
        public sealed class SavageBehavior : BaseBuffBehaviour, IBodyStatArgModifier, IOnTakeDamageServerReceiver
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.PredatoryRush;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                if (HasAnyStacks)
                {
                    int stack = CharacterBody.inventory.GetItemCount(RRPContent.Items.PredatorySavagery);
                    args.damageMultAdd += PredatorySavagery.damage / 100 * stack;
                    if ((PredatorySavagery.cooldownReduction * stack) < 50)
                    {
                        args.cooldownMultAdd -= PredatorySavagery.cooldownReduction / 100 * stack;
                    }
                    else
                    {
                        args.cooldownMultAdd -= .5f;
                    }

                    args.armorAdd += PredatorySavagery.armor * stack;

                    args.jumpPowerMultAdd += PredatorySavagery.jumpMult / 100;

                    args.critAdd += PredatorySavagery.critChance;
                    args.critDamageMultAdd += PredatorySavagery.critDamage / 100 * stack;
                }
                
            }

            public void OnTakeDamageServer(DamageReport damageReport)
            {
                //body.RemoveBuff(RRPContent.Buffs.PredatoryRush);
            }
        }

        public override void Initialize()
        {

        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }
    }
}
