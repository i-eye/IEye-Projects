using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MSU;
using MSU.Config;
using RoR2;
using RoR2.Items;
using R2API;
using Mono.Cecil;
using System.Linq;
using RoR2.Projectile;
using RoR2.Orbs;
using RoR2.ContentManagement;
using static MSU.BaseBuffBehaviour;

namespace IEye.RRP.Items
{
    //[DisabledContent]
    public class AgressiveInsect : RRPItem
    {

        public const string token = "RRP_ITEM_AGROINSECT_DESC";
        //public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("AgressiveInsect", RRPBundle.Items);

        public override RRPAssetRequest AssetRequest => RRPAssets.LoadAssetAsync<ItemAssetCollection>("acAInsect", RRPBundle.Items);

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Duration of the insect blood debuff(default 8s)")]
        [FormatToken(token, 0)]
        public static float duration = 8f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Percent damage per orb per stack(default 25%)")]
        [FormatToken(token, 1)]
        public static int bloodyInsectExtraDamage = 25;

        readonly static ProcType basedProc = (ProcType)382143;

        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver, IOnTakeDamageServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.AgressiveInsect;
            
            private void Start()
            {
                
                if(body.inventory.GetItemCount(RRPContent.Items.SacrificialHelper) == 0)
                {
                    body.inventory.GiveItem(RRPContent.Items.SacrificialHelper);
                }
            }
            
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                if (damageReport.victimBody.GetBuffCount(RRPContent.Buffs.InsectBloody) > 0 && !damageReport.damageInfo.procChainMask.HasProc(basedProc))
                {
                    SpawnMissile(damageReport);
                }
                

            }

            private void SpawnMissile(DamageReport damageReport)
            {
                float damageCoef = bloodyInsectExtraDamage * stack / 100f;
                DamageInfo damageInfo = damageReport.damageInfo;
                var insectOrb = new Orbs.AgressiveInsectMissileOrb();
                insectOrb.origin = body.aimOrigin;
                insectOrb.damageValue = Util.OnHitProcDamage(damageInfo.damage, damageReport.attackerBody.damage, damageCoef);
                insectOrb.isCrit = damageInfo.crit;
                insectOrb.teamIndex = damageReport.attackerTeamIndex;
                insectOrb.attacker = damageInfo.attacker;
                insectOrb.procChainMask = damageInfo.procChainMask;
                insectOrb.procChainMask.AddProc(basedProc);
                insectOrb.procCoefficient = 0.2f;
                insectOrb.damageColorIndex = DamageColorIndex.Bleed;
                HurtBox mainHurtBox = damageReport.victimBody.mainHurtBox;
                if ((bool)mainHurtBox)
                {
                    insectOrb.target = mainHurtBox;
                    OrbManager.instance.AddOrb(insectOrb);
                }
            }

            private void applyPoision(CharacterBody cb)
            {
                int buffCount = cb.GetBuffCount(RRPContent.Buffs.InsectBloody);

                if (buffCount > 0)
                {
                    cb.RemoveOldestTimedBuff(RRPContent.Buffs.InsectBloody.buffIndex);
                }
                cb.AddTimedBuffAuthority(RRPContent.Buffs.InsectBloody.buffIndex, duration * stack);
            }

            public void OnTakeDamageServer(DamageReport report)
            {
                var attacker = report.attacker;
                var cb = attacker.GetComponent<CharacterBody>();
                if (cb && report.damageInfo.procCoefficient > 0)
                {
                    applyPoision(cb);
                }
            }
        }

        public sealed class BloodBehavior : BaseBuffBehaviour
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.InsectBloody;



        }

        public override void Initialize()
        {

        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }
    }
}