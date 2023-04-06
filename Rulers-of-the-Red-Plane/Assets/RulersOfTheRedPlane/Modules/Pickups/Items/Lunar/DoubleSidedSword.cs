using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;

namespace IEye.RulersOfTheRedPlane.Items
{
    public class DoubleSidedSword : ItemBase
    {

        private const string token = "RRP_ITEM_DOUBLESWORD_DESC";
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("DoubleSidedSword", RRPBundle.Items);


        [ConfigurableField(ConfigName = "Radius")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float radiusBase = 50f;

        [ConfigurableField(ConfigName = "Damage")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float damage = 2f;

        [ConfigurableField(ConfigName = "Duration")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float duration = 3f;

       

        public sealed class Behavior : BaseItemBodyBehavior, IOnTakeDamageServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.DoubleSidedSword;
            public void OnTakeDamageServer(DamageReport report)
            {
                //DefNotSS2Log.Message("dotIndex is: " + report.damageInfo.dotIndex);
                if (report.damageInfo.procCoefficient > 0 && report.damageInfo.dotIndex == DotController.DotIndex.None)
                {
                    var victim = report.victim;
                    

                    var dotInfoVictim = new InflictDotInfo()
                    {
                        attackerObject = victim.gameObject,
                        victimObject = victim.gameObject,
                        dotIndex = DotController.DotIndex.Bleed,
                        duration = duration * report.damageInfo.procCoefficient,
                        damageMultiplier = damage * .2f * stack,
                    };
                    DotController.InflictDot(ref dotInfoVictim);

                    HealthComponent[] components = PickNextTarget(report.victimBody.corePosition, victim);
                    if (components != null)
                    {
                        foreach (HealthComponent component in components)
                        {
                            var dotInfo = new InflictDotInfo()
                            {
                                attackerObject = victim.gameObject,
                                victimObject = component.gameObject,
                                dotIndex = DotController.DotIndex.Bleed,
                                duration = duration * report.damageInfo.procCoefficient,
                                damageMultiplier = damage * stack,
                            };
                            DotController.InflictDot(ref dotInfo);
                        }
                    }
                }
            }



            public HealthComponent[] PickNextTarget(Vector3 position, HealthComponent currentVictim)
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
                this.search.origin = position;
                this.search.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;




                this.search.RefreshCandidates();
                this.search.FilterCandidatesByHurtBoxTeam(mask);
                this.search.FilterCandidatesByDistinctHurtBoxEntities();

                HurtBox[] hurtBoxes = this.search.GetHurtBoxes();
                HealthComponent[] healthComponents = new HealthComponent[hurtBoxes.Length];
                for (int i = 0; i < hurtBoxes.Length; i++)
                {
                    healthComponents[i] = hurtBoxes[i].healthComponent;
                }
                if (healthComponents.Length == 0 || healthComponents == null)
                {
                    DefNotSS2Log.Message("Search is null");
                    return null;
                }
                return healthComponents;



            }
            private SphereSearch search;

            public float baseRange = radiusBase;
        }

            
    }
}