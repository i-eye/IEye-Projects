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
        
        private const string token = "RRP_ITEM_FOURDIMENSIONALDAGGER_DESC";
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("FourDimensionalDagger", RRPBundle.Items);

        

        [ConfigurableField(ConfigName = "Chance")]
        [TokenModifier(token, StatTypes.MultiplyByN, 0, "100")]
        public static float percentChance = .15f;

        [ConfigurableField(ConfigName = "Radius")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float radiusBase = 25f;

        [TokenModifier(token, StatTypes.Default, 2)]
        public static float radiusIncrease = (radiusBase * .2f);

        [ConfigurableField(ConfigName = "Duration")]
        public static float duration = 3f;


        
        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver
        {
            
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.FourDimensionalDagger;
            
            public void OnDamageDealtServer(DamageReport report)
            {
                if (report.damageInfo.procCoefficient == 0f)
                {
                    return;
                }
                var attackVictim = report.victim;
                var attacker = report.attacker;
                TeamIndex teamIndex = report.attackerBody.teamComponent.teamIndex;
                if (!report.damageInfo.procChainMask.HasProc(ProcType.BleedOnHit)){
                    ProcChainMask procChainMask = report.damageInfo.procChainMask;
                    procChainMask.AddProc(ProcType.BleedOnHit);
                    if (Random.value < (percentChance * stack))
                    {
                        HealthComponent victim = PickNextTarget(report.victimBody.corePosition, attackVictim);
                        if(victim == null)
                        {
                            return;
                        }
                        //DefNotSS2Log.Message("Victim:" + victim.gameObject.name);
                        var dotInfo = new InflictDotInfo()
                        {
                            attackerObject = attacker,
                            victimObject = victim.gameObject,
                            dotIndex = DotController.DotIndex.Bleed,
                            duration = report.damageInfo.procCoefficient * duration,
                            damageMultiplier = 1f,
                        };
                        //DefNotSS2Log.Message("Before inflict");
                        DotController.InflictDot(ref dotInfo);
                        //DefNotSS2Log.Message("I hope an enemy nearby has an effect lol");

                    }
                }


            }


            public HealthComponent PickNextTarget(Vector3 position, HealthComponent currentVictim)
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
                this.search.radius = range + (radiusIncrease * .2f);
                this.search.origin = position;
                this.search.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;


                
                
                this.search.RefreshCandidates();
                this.search.FilterCandidatesByHurtBoxTeam(mask);
                this.search.FilterCandidatesByDistinctHurtBoxEntities();

                HurtBox[] hurtBoxes = this.search.GetHurtBoxes();
                List<HealthComponent> healthComponents = new List<HealthComponent>();
                foreach(HurtBox hurtBox in hurtBoxes)
                {
                    if(hurtBox.healthComponent != currentVictim)
                    {
                        healthComponents.Add(hurtBox.healthComponent);
                    }
                }
                if (healthComponents.Count != 0)
                {
                    //DefNotSS2Log.Message("Found healthcomponent array(length): " + healthComponents.Length);
                    selected = healthComponents[Random.Range(0, healthComponents.Count)];
                }
                else
                {
                    //DefNotSS2Log.Message("Search is null");
                    return null;
                }

                return selected;
                    
                
                
            }
            private SphereSearch search;
            private HealthComponent selected;
            public float baseRange = radiusBase;
        }
    }
}