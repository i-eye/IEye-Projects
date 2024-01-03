using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using IEye.RRP.Buffs;
using R2API;
using Mono.Cecil;
using System.Linq;
using RoR2.Projectile;
using RoR2.Orbs;

namespace IEye.RRP.Items
{
    //[DisabledContent]
    public class AgressiveInsect : ItemBase
    {

        public const string token = "RRP_ITEM_AGROINSECT_DESC";
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("AgressiveInsect", RRPBundle.Items);

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Duration of the insect blood debuff(default 8s)")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float duration = 8f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Armor taken away from the insect blood debuff victim(default 50)")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static int bloodyInsectExtraDamage = 50;

        readonly static ProcType basedProc = (ProcType)382143;

        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver, IOnIncomingDamageServerReceiver
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

            public void OnIncomingDamageServer(DamageInfo damageInfo)
            {
                applyPoision(damageInfo.attacker.GetComponent<CharacterBody>());
            }
        }
    }
}