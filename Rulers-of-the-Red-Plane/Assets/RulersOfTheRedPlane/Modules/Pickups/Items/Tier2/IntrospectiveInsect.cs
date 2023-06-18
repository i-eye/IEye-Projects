using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using IEye.RulersOfTheRedPlane.Buffs;
using R2API;
using Mono.Cecil;
using System.Linq;

namespace IEye.RulersOfTheRedPlane.Items
{
    //[DisabledContent]
    public class IntrospectiveInsect : ItemBase
    {

        public const string token = "RRP_ITEM_INTROINSECT_DESC";

        //[TokenModifier(token, StatTypes.Default, 0)]
        //public static float healCoef = 1.5f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Duration of the insect posion(default 10s)")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static int duration = 10;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percentage of damage taken away from insect poison target(default 20%)")]
        [TokenModifier(token, StatTypes.MultiplyByN, 2, "100")]
        public static float insectDamageCripple = (.20f * 100);

        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("IntrospectiveInsect", RRPBundle.Items);

        public sealed class Behavior: BaseItemBodyBehavior, IOnTakeDamageServerReceiver, IOnKilledOtherServerReceiver
        {
            [ItemDefAssociation]

            private static ItemDef GetItemDef() => RRPContent.Items.IntrospectiveInsect;

            public void OnTakeDamageServer(DamageReport report)
            {
                var attacker = report.attacker;
                var cb = attacker.GetComponent<CharacterBody>();
                if (cb)
                {
                    applyPoision(cb);
                }
            }

            public void OnKilledOtherServer(DamageReport damageReport)
            {
                var attacker = damageReport.attackerBody;
                var victim = damageReport.victim;
                var cbVictim = victim.GetComponent<CharacterBody>();

                if (cbVictim.activeBuffsList.Contains(RRPContent.Buffs.InsectPoison.buffIndex))
                {
                    float healthValueCoef = 3 - (3f / (1.1f + (.1f * stack)));
                    attacker.healthComponent.Heal(healthValueCoef * cbVictim.damage, damageReport.damageInfo.procChainMask);
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
    }
}