using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using System.Linq;

namespace IEye.RulersOfTheRedPlane.Items {
    public class FourDimensionalDagger : ItemBase
    {
        private const string token = "RRP_ITEM_FOURDIMENSIONALDAGGER_DESC";
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("FourDimensionalDagger", RRPBundle.Items);


        public sealed class Behavior : BaseItemMasterBehavior, IOnDamageDealtServerReceiver
        {
            [ItemDefAssociation]

            private static ItemDef GetItemDef() => RRPContent.Items.FourDimensionalDagger;

            public void OnDamageDealtServer(DamageReport report)
            {
                var victim = report.victim;
                var attacker = report.attacker;
                
            }


            public HurtBox PickNextTarget(Vector3 position, HealthComponent currentVictim)
            {
                if (this.search == null)
                {
                    this.search = new BullseyeSearch();
                }
                float range = baseRange;
                if (currentVictim && currentVictim.body)
                {
                    range += currentVictim.body.radius;
                }
                this.search.searchOrigin = position;
                this.search.searchDirection = Vector3.zero;
                this.search.teamMaskFilter = TeamMask.allButNeutral;
                this.search.teamMaskFilter.RemoveTeam(this.teamIndex);
                this.search.filterByLoS = false;
                this.search.sortMode = BullseyeSearch.SortMode.Distance;
                this.search.maxDistanceFilter = range;
                this.search.RefreshCandidates();
                HurtBox hurtBox = (from v in this.search.GetResults()
                                   where !this.bouncedObjects.Contains(v.healthComponent)
                                   select v).FirstOrDefault<HurtBox>();
                if (hurtBox)
                {
                    this.bouncedObjects.Add(hurtBox.healthComponent);
                }
                return hurtBox;
            }
            private BullseyeSearch search;
            public TeamIndex teamIndex;
            public List<HealthComponent> bouncedObjects;
        }
    }
}