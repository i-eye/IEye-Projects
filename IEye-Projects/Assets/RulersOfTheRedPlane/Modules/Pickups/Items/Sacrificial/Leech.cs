using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using R2API;

namespace IEye.RRP.Items
{

    public class Leech : ItemBase
    {
        public const string token = "RRP_ITEM_LEECH_DESC";
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("SacrificialLeech", RRPBundle.Items);


        public override void Initialize()
        {
            base.Initialize();
            On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
        }
        private float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen)
        {
            amount /= (self.body.inventory.GetItemCount(RRPContent.Items.Leech) + 1);
            return orig(self, amount, procChainMask, nonRegen);
        }

        public sealed class Behavior : BaseItemBodyBehavior, IOnTakeDamageServerReceiver
        {

            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.Leech;
            

            

            public void OnTakeDamageServer(DamageReport damageReport)
            {
                var dotinfo = new InflictDotInfo
                {
                    victimObject = damageReport.victim.gameObject,
                    attackerObject = damageReport.victim.gameObject,
                    dotIndex = DotController.DotIndex.Bleed,
                    duration = 3f,
                    damageMultiplier = .2f,
                    totalDamage = damageReport.damageDealt / 2
                };
                DotController.InflictDot(ref dotinfo);
            }
        }

    }
} 
