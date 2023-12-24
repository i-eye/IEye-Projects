using RoR2;
using RoR2.Items;
using Moonstorm;
using R2API;
using UnityEngine;
using System.Collections.Generic;
using R2API.Networking;

namespace IEye.RRP.Items
{ 
    [DisabledContent]
    public class BloodlustIvy : ItemBase
    {

        private const string token = "RRP_ITEM_BLOODIVY_DESC";
        public static float percentHealthCoef = 15;

        public static float distanceNeeded = 15f;
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("BloodlustIvy", RRPBundle.Items);

        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageInflictedServerReceiver
        {
            public static float damageNeeded;
            public static float healthTracker;

            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.BloodyIvy;

            
            private void Explode()
            {
                
                List<HealthComponent> poisonTime = PickNextTarget(body.healthComponent);
                foreach(HealthComponent healthComponent in poisonTime)
                {
                    healthComponent.ApplyDot(body.gameObject, DotController.DotIndex.Poison, 10f, 1.5f);
                }
                damageNeeded = body.healthComponent.fullCombinedHealth * percentHealthCoef;
            }

            public void OnDamageInflictedServer(DamageReport damageReport)
            {
                float distance = Vector3.Distance(damageReport.victimBody.transform.position, body.transform.position);
                if (distance >= distanceNeeded)
                {
                    healthTracker += damageReport.damageDealt;
                }

                if (healthTracker >= damageNeeded)
                {
                    Explode();
                }

            }

            public List<HealthComponent> PickNextTarget(HealthComponent currentVictim)
            {

                if (this.search == null)
                {
                    this.search = new SphereSearch();
                }
                float range = baseRange;
                if (currentVictim && currentVictim.body)
                {
                    range += currentVictim.body.radius;
                }

                TeamMask mask = TeamMask.AllExcept(TeamIndex.Player);
                this.search.mask = LayerIndex.entityPrecise.mask;
                this.search.radius = range;
                this.search.origin = currentVictim.body.corePosition;
                this.search.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                this.search.RefreshCandidates();
                this.search.FilterCandidatesByHurtBoxTeam(mask);
                this.search.FilterCandidatesByDistinctHurtBoxEntities();

                HurtBox[] hurtBoxes = this.search.GetHurtBoxes();
                healthComponents = new List<HealthComponent>();
                foreach (HurtBox hurtBox in hurtBoxes)
                {
                    if (hurtBox.healthComponent != currentVictim)
                    {
                        healthComponents.Add(hurtBox.healthComponent);
                    }
                }

                if (healthComponents.Count == 0)
                {
                    //DefNotSS2Log.Message("Search is null");
                    return null;
                }

                return healthComponents;



            }
            private SphereSearch search;
            private List<HealthComponent> healthComponents;
            public float baseRange = distanceNeeded;
        }
    }
}

