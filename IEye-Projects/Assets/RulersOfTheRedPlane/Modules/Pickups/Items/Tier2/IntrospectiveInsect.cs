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

namespace IEye.RRP.Items
{
    //[DisabledContent]
    public class IntrospectiveInsect : ItemBase
    {

        public const string token = "RRP_ITEM_INTROINSECT_DESC";

        //[TokenModifier(token, StatTypes.Default, 0)]
        //public static float healCoef = 1.5f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Duration of the insect posion per stack(default 10s)")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static int duration = 10;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percentage of attack speed slow(default 65%)")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float insectAttackSpeed = 65f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percentage of movement speed slow(default 50%)")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float insectMoveSpeed = 50f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Life steal on hitting poisoned enemies")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float insectHealAmount = 2f;

        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("IntrospectiveInsect", RRPBundle.Items);

        public sealed class Behavior: BaseItemBodyBehavior, IOnTakeDamageServerReceiver, IOnDamageDealtServerReceiver
        {
            [ItemDefAssociation]

            private static ItemDef GetItemDef() => RRPContent.Items.IntrospectiveInsect;

            
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                if (damageReport.victimBody.GetBuffCount(RRPContent.Buffs.InsectPoison ) > 0)
                {
                    body.healthComponent.Heal(insectHealAmount, new ProcChainMask());
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
    }
}