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

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Duration of the insect blood debuff per stack(default 3s)")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float duration = 10f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Armor taken away from the insect blood debuff victim(default 20)")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static int bloodyInsectExtraDamage = 25;

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
                if (damageReport.victimBody.GetBuffCount(RRPContent.Buffs.InsectBloody) > 0)
                {
                    SpawnMissile(damageReport);
                }
                

            }

            private void SpawnMissile(DamageReport damageReport)
            {
                DamageInfo damageInfo = damageReport.damageInfo;
                var missileVoidOrb = new MissileVoidOrb();
                missileVoidOrb.origin = body.aimOrigin;
                missileVoidOrb.damageValue = damageReport.damageDealt * .25f;
                missileVoidOrb.isCrit = damageInfo.crit;
                missileVoidOrb.teamIndex = damageReport.attackerTeamIndex;
                missileVoidOrb.attacker = damageInfo.attacker;
                missileVoidOrb.procChainMask = damageInfo.procChainMask;
                missileVoidOrb.procChainMask.AddProc(ProcType.Missile);
                missileVoidOrb.procCoefficient = 0.2f;
                missileVoidOrb.damageColorIndex = DamageColorIndex.Void;
                HurtBox mainHurtBox = damageReport.victimBody.mainHurtBox;
                if ((bool)mainHurtBox)
                {
                    missileVoidOrb.target = mainHurtBox;
                    OrbManager.instance.AddOrb(missileVoidOrb);
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