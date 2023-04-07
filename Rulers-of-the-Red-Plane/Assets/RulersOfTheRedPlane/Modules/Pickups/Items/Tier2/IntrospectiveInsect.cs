using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using R2API;
namespace IEye.RulersOfTheRedPlane.Items
{
    [DisabledContent]
    public class IntrospectiveInsect : ItemBase
    {
        public static float healCoef = .25f;
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("IntrospectiveInsect", RRPBundle.Items);

        public sealed class Behavior: BaseItemBodyBehavior, IOnTakeDamageServerReceiver
        {

            bool healing = false;
            bool hitWhileHealing = false;

            public void OnTakeDamageServer(DamageReport report)
            {
                if (!healing)
                {
                    StartCoroutine(waitToHeal(report.damageDealt));
                }
                else
                {
                    hitWhileHealing = true;
                }
            }

            IEnumerator waitToHeal(float damageTaken)
            {
                yield return new WaitForSecondsRealtime(10f);
                if (!hitWhileHealing)
                {
                    body.healthComponent.Heal(damageTaken * healCoef, default);
                }
                healing = false;
                hitWhileHealing = false;

            }


        }
    }
}