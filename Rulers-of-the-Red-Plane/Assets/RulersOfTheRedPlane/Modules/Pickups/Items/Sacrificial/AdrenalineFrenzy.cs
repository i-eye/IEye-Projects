using Moonstorm;
using RoR2;
using RoR2.Items;
using R2API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R2API.Networking;

namespace IEye.RulersOfTheRedPlane.Items
{
    public class AdrenalineFrenzy : ItemBase
    {
        public const string token = "RRP_ITEM_ADRFRENZY_DESC";
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("AdrenalineFrenzy");


        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base percentage speed added on kill(default 5%).")]
        public static float killSpeed = 5f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base percentage speed added on kill per stack(default 3%).")]
        public static float killSpeedStack = 3f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base time for on kill speed boost(default 10s).")]
        public static float killSpeedDuration = 10f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base percentage speed added on getting hit(default 10%).")]
        public static float onHitSpeed = 10f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base percentage speed added on getting hit per stack(default(5%).")]
        public static float onHitSpeedStack = 5f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Base time for on getting hit speed boost(default 10s).")]
        public static float onHitSpeedDuration = 10f;

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
                var cb = report.victim.GetComponent<CharacterBody>();
                cb.AddTimedBuffAuthority(RRPContent.Buffs.AdrenalineOnGettingHit.buffIndex, onHitSpeedDuration);
            }
        }




    }
}
