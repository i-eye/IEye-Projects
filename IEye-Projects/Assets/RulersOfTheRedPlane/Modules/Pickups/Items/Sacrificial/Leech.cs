using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using R2API;
using EntityStates.AffixEarthHealer;

namespace IEye.RRP.Items
{

    public class Leech : ItemBase
    {
        private const string token = "RRP_ITEM_LEECH_DESC";
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("Leech", RRPBundle.Items);

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Chance for this item to proc per stack(default 15%).")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float percentChance = 15f;
        public override void Initialize()
        {
            base.Initialize();
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

        public sealed class Behavior : BaseItemBodyBehavior, IOnTakeDamageServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.Leech;


            public void OnTakeDamageServer(DamageReport report)
            {
                
                if((report.damageInfo.procCoefficient > 0) && (report.damageInfo.dotIndex.Equals(DotController.DotIndex.None)) && ((int)report.damageInfo.damageType) != 66)
                {
                    var dotinfo = new InflictDotInfo
                    {
                        victimObject = report.victim.gameObject,
                        attackerObject = report.victim.gameObject,
                        dotIndex = DotController.DotIndex.Bleed,
                        duration = 2f * report.damageInfo.procCoefficient,
                        damageMultiplier = 1f,
                        totalDamage = report.damageDealt / 5
                    };
                    DotController.InflictDot(ref dotinfo);
                }
                
            }
        }

    }
} 
