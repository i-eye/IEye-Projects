using Moonstorm;
using RoR2;
using RoR2.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IEye.RRP.Items
{
    public class Kamikaze : ItemBase
    {
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("Kamikaze", RRPBundle.Items);

        public override void Initialize()
        {
            base.Initialize();
        }
        public sealed class Behavior : BaseItemBodyBehavior, IOnKilledServerReceiver
        {
            private GameObject explodeOnDeathPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ExplodeOnDeath/WilloWispDelay.prefab").WaitForCompletion();

            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.Kamikaze;
            public void OnKilledServer(DamageReport damageReport)
            {
                Vector3 vector = Vector3.zero;
                if (damageReport.victim)
                {
                    vector = damageReport.victim.transform.position;
                }
                GameObject obj = Instantiate(explodeOnDeathPrefab, vector, Quaternion.identity);
                DelayBlast blast = obj.GetComponent<DelayBlast>();
                float damageCoefficient = 4f + (2.5f * (stack-1));
                float damage = Util.OnKillProcDamage(damageReport.victimBody.damage, damageCoefficient);
                if (blast)
                {
                    blast.position = vector;
                    blast.baseDamage = damage;
                    blast.baseForce = 2000f;
                    blast.bonusForce = Vector3.up * 1000f;
                    blast.radius = 25f + (stack-1);
                    blast.attacker = damageReport.victim.gameObject;
                    blast.inflictor = damageReport.victim.gameObject;
                    blast.crit = Util.CheckRoll(damageReport.victimBody.crit, damageReport.victimMaster);
                    blast.maxTimer = 1f;
                    blast.damageColorIndex = DamageColorIndex.Item;
                    blast.falloffModel = BlastAttack.FalloffModel.Linear;
                }
            }
        }
    }
}

