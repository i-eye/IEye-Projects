using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MSU;
using MSU.Config;
using RoR2;
using RoR2.Items;
using RoR2.Orbs;
//using IEye.RRP.Buffs;
using R2API;
using Mono.Cecil;
using System.Linq;
using RoR2.ContentManagement;
using static MSU.BaseBuffBehaviour;

namespace IEye.RRP.Items
{
    //[DisabledContent]
    public class IntrospectiveInsect : RRPItem
    {

        public const string token = "RRP_ITEM_INTROINSECT_DESC";

        //[FormatToken(token, opType:default, 0)]
        //public static float healCoef = 1.5f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Duration of the insect posion per stack(default 7s)")]
        [FormatToken(token, 0)]
        public static int duration = 7;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Percentage of attack speed slow(default 50%)")]
        [FormatToken(token, 1)]
        public static float insectAttackSpeed = 50f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Percentage of movement speed slow(default 50%)")]
        [FormatToken(token, 2)]
        public static float insectMoveSpeed = 50f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Percent life restored on hitting enemies per stack(default 8%)")]
        [FormatToken(token, 3)]
        public static float insectHealAmount = 8f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Hits needed to heal(default 5)")]
        [FormatToken(token, 4)]
        public static int hitsNeeded = 5;

        public override RRPAssetRequest AssetRequest => RRPAssets.LoadAssetAsync<ItemAssetCollection>("acIInsect", RRPBundle.Items);

        public override void Initialize()
        {
            
        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }

        //public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("IntrospectiveInsect", RRPBundle.Items);

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
                if (cb  && report.damageInfo.procCoefficient > 0)
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
        public sealed class BuffBehavior : BaseBuffBehaviour, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.InsectPoison;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                if (hasAnyStacks)
                {
                    args.attackSpeedReductionMultAdd += IntrospectiveInsect.insectAttackSpeed / 100;
                    args.moveSpeedReductionMultAdd += IntrospectiveInsect.insectMoveSpeed / 100;
                }
                
            }
        }

    }
    
}