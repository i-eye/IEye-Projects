using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using R2API;
using System.Linq;

namespace IEye.RulersOfTheRedPlane.Items {
    public class FourDimensionalDagger : ItemBase
    {
        public override void Initialize()
        {
            base.Initialize();
            DefNotSS2Log.Info("4D script loaded");
        }
        private const string token = "RRP_ITEM_FOURDIMENSIONALDAGGER_DESC";
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("FourDimensionalDagger", RRPBundle.Items);

        [TokenModifier(token, StatTypes.Default, 0)]
        public static float radiusBase = 25f;

        [TokenModifier(token, StatTypes.MultiplyByN, 100)]
        public static float percentChance = .15f;

        [TokenModifier(token, StatTypes.Default, 0)]
        public static float duration = 1f;

        
        public sealed class Behavior : BaseItemMasterBehavior, IOnDamageDealtServerReceiver
        {
            public void Awake()
            {
                DefNotSS2Log.Info("Is Awake");
            }
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.FourDimensionalDagger;

            public void OnDamageDealtServer(DamageReport report)
            {
                DefNotSS2Log.Info("4D dagger has OnDamgeDealtServer");
                var attackVictim = report.victim;
                var attacker = report.attacker;
                TeamIndex teamIndex = report.attackerBody.teamComponent.teamIndex;
                if(Random.value >  percentChance)
                {
                    DefNotSS2Log.Message("Item Proc");
                    HurtBox victim = PickNextTarget(report.victimBody.corePosition, attackVictim, teamIndex);
                    DotController.InflictDot(victim.gameObject, attacker, DotController.DotIndex.Bleed, duration * report.damageInfo.procCoefficient, 1f);
                    DefNotSS2Log.Message("I hope an enemy nearby has an effect lol");
                }
                


            }


            public HurtBox PickNextTarget(Vector3 position, HealthComponent currentVictim, TeamIndex teamIndex)
            {
                index = teamIndex;
                if (this.search == null)
                {
                    this.search = new SphereSearch();
                }
                float range = baseRange;
                if (currentVictim && currentVictim.body)
                {
                    range += currentVictim.body.radius;
                }

                this.search.radius = range;
                this.search.origin = position;
                this.search.RefreshCandidates();
                this.search.FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(teamIndex));


                HurtBox[] hurtBoxes = this.search.GetHurtBoxes();
                HurtBox selected = hurtBoxes[Random.Range(0, hurtBoxes.Length)];
                DefNotSS2Log.Message("Should have hurtbox now");
                return selected;

                
            }
            private SphereSearch search;
            public TeamIndex index;
            public List<HealthComponent> bouncedObjects;
            public float baseRange = radiusBase;
        }
    }
}