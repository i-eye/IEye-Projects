using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MSU;
using MSU.Config;
using RoR2;
using RoR2.Items;
using R2API;
using EntityStates.AffixEarthHealer;
using RoR2.ContentManagement;

namespace IEye.RRP.Items
{

    public class Leech : RRPItem
    {
        private const string token = "RRP_ITEM_LEECH_DESC";
        //public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("Leech", RRPBundle.Items);

        public override RRPAssetRequest AssetRequest => RRPAssets.LoadAssetAsync<ItemAssetCollection>("acLeech", RRPBundle.Items);

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Chance for this item to proc per stack(default 15%).")]
        [FormatToken(token, opType:default, 0)]
        public static float percentChance = 15f;
        public override void Initialize()
        {
            On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
        }
        private float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen)
        {
            if (self.body.inventory)
            {
                amount /= (self.body.inventory.GetItemCount(RRPContent.Items.Leech) + 1);
            }
            return orig(self, amount, procChainMask, nonRegen);
        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }

        public sealed class Behavior : BaseItemBodyBehavior, IOnTakeDamageServerReceiver, IOnKilledOtherServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.Leech;


            public void OnTakeDamageServer(DamageReport report)
            {
                
                if((report.damageInfo.procCoefficient > 0) && (report.damageInfo.dotIndex.Equals(DotController.DotIndex.None)) && ((int)report.damageInfo.damageType.damageType) != 66)
                {
                    var dotinfo = new InflictDotInfo
                    {
                        victimObject = report.victim.gameObject,
                        attackerObject = report.victim.gameObject,
                        dotIndex = DotController.DotIndex.Bleed,
                        duration = 2f * report.damageInfo.procCoefficient,
                        damageMultiplier = 1f,
                        totalDamage = report.damageDealt / 6
                    };
                    DotController.InflictDot(ref dotinfo);
                }
                
            }

            public void OnKilledOtherServer(DamageReport report)
            {
                if( Random.Range(0f, 100f) < 10f)
                {
                    body.inventory.RemoveItem(RRPContent.Items.Leech,1);
                }
                    
                
            }
        }

    }
} 
