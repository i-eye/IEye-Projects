using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using RoR2.Orbs;
using IEye.RRP.Buffs;
using R2API;
using Mono.Cecil;
using System.Linq;

namespace IEye.RRP.Items
{
    //[DisabledContent]
    public class IntrospectiveInsect : ItemBase
    {

        public const string token = "RRP_ITEM_INTROINSECT_DESC";

        //[TokenModifier(token, StatTypes.Default, 0)]
        //public static float healCoef = 1.5f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Duration of the insect posion per stack(default 8s)")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static int duration = 8;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percentage of attack speed slow(default 65%)")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float insectAttackSpeed = 65f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percentage of movement speed slow(default 50%)")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float insectMoveSpeed = 50f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percent life restored on hitting enemies per stack(default 2%)")]
        [TokenModifier(token, StatTypes.Default, 3)]
        public static float insectHealAmount = 5f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Hits needed to heal(default 4)")]
        [TokenModifier(token, StatTypes.Default, 4)]
        public static int hitsNeeded = 5;

        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("IntrospectiveInsect", RRPBundle.Items);

        public sealed class Behavior: BaseItemBodyBehavior, IOnTakeDamageServerReceiver, IOnDamageDealtServerReceiver
        {
            [ItemDefAssociation]

            private static ItemDef GetItemDef() => RRPContent.Items.IntrospectiveInsect;

            
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                var victimBody = damageReport.victimBody;
                
                if (victimBody)
                {
                    GameObject gameObject = victimBody.gameObject;
                    HitCounter component;
                    if (victimBody.GetBuffCount(RRPContent.Buffs.InsectPoison) > 0)
                    {
                        if (!gameObject.TryGetComponent<HitCounter>(out component))
                        {
                            component = gameObject.AddComponent<HitCounter>();
                        }
                        component.AddHit();
                        if (component.CheckForHit())
                        {
                            victimBody.RemoveBuff(RRPContent.Buffs.InsectPoison);
                            SpawnOrb(damageReport, victimBody);
                        }
                    }
                }
            }

            private static void SpawnOrb(DamageReport damageReport, CharacterBody victimBody)
            {
                HealOrb orb = new HealOrb();
                orb.origin = victimBody.aimOrigin;
                orb.healValue = damageReport.attackerBody.maxHealth * (insectHealAmount / 100);
                HurtBox targetHurtBox = damageReport.attackerBody.mainHurtBox;
                if (targetHurtBox)
                {
                    orb.target = targetHurtBox;
                    OrbManager.instance.AddOrb(orb);
                }
            }

            public void OnTakeDamageServer(DamageReport report)
            {
                var attacker = report.attacker;
                var cb = attacker.GetComponent<CharacterBody>();
                if (cb)
                {
                    applyPoision(cb);
                }
            }

            

            private void applyPoision(CharacterBody cb)
            {
                int buffCount = cb.GetBuffCount(RRPContent.Buffs.InsectPoison);

                if(buffCount > 0)
                {
                    cb.RemoveOldestTimedBuff(RRPContent.Buffs.InsectPoison.buffIndex);
                }
                cb.AddTimedBuffAuthority(RRPContent.Buffs.InsectPoison.buffIndex, duration);
            }

        }
        public class HitCounter: MonoBehaviour
        {
            private int hitNumber = 0;

            public void AddHit() { hitNumber++; }
            public void ResetHit() { hitNumber = 0; }
            public bool CheckForHit()
            {
                if(hitNumber == hitsNeeded) 
                {
                    ResetHit();
                    return true; 
                }
                return false;
            }
        }
    }
}