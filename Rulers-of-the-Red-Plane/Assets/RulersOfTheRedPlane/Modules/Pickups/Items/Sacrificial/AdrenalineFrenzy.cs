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
    public class AdrenalineFrenzy : ItemBase
    {
        public const string token = "RRP_ITEM_ADRFRENZY_DESC";
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("AdrenalineFrenzy");


        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base percentage speed added on kill(default 6%).")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float killSpeed = 6f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base percentage speed added on kill per stack(default 4%).")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float killSpeedStack = 4f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base time for on kill speed boost(default 10s).")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float killSpeedDuration = 10f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base percentage speed added on getting hit(default 12%).")]
        [TokenModifier(token, StatTypes.Default, 3)]
        public static float onHitSpeed = 12f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base percentage speed added on getting hit per stack(default(7%).")]
        [TokenModifier(token, StatTypes.Default, 4)]
        public static float onHitSpeedStack = 7f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base time for on getting hit speed boost(default 8s).")]
        [TokenModifier(token, StatTypes.Default, 5)]
        public static float onHitSpeedDuration = 8f;

        public sealed class Behavior : BaseItemBodyBehavior, IOnTakeDamageServerReceiver, IOnKilledOtherServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.AdrenalineFrenzy;


            private void Start()
            {
                if (body.inventory.GetItemCount(RRPContent.Items.SacrificialHelper) == 0)
                {
                    body.inventory.GiveItem(RRPContent.Items.SacrificialHelper);
                }
            }
            public void OnKilledOtherServer(DamageReport report)
            {
                
                if ((report.damageInfo.procCoefficient > 0) && (report.damageInfo.dotIndex.Equals(DotController.DotIndex.None)) && ((int)report.damageInfo.damageType) != 66)
                {
                    var cb = report.attacker.GetComponent<CharacterBody>();
                    cb.AddTimedBuffAuthority(RRPContent.Buffs.AdrenalineOnKill.buffIndex, killSpeedDuration);
                }
            }

            public void OnTakeDamageServer(DamageReport report)
            {
                if ((report.damageInfo.procCoefficient > 0) && (report.damageInfo.dotIndex.Equals(DotController.DotIndex.None)) && ((int)report.damageInfo.damageType) != 66){
                    var cb = report.victim.GetComponent<CharacterBody>();
                    cb.AddTimedBuffAuthority(RRPContent.Buffs.AdrenalineOnGettingHit.buffIndex, onHitSpeedDuration);
                }
            }
        }




    }
}
