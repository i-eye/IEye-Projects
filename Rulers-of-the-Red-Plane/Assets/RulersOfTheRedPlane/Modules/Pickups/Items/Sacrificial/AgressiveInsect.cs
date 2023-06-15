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
    public class AgressiveInsect : ItemBase
    {

        public const string token = "RRP_ITEM_AGROINSECT_DESC";
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("AgressiveInsect", RRPBundle.Items);

        public static float duration = 5f;
        public static float bloodyInsectDamageCripple = .1f;
        public static float bloodyInsectArmorCripple = 25f;
        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver
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
                applyPoision(damageReport.victimBody);

            }
            private void applyPoision(CharacterBody cb)
            {
                int buffCount = cb.GetBuffCount(RRPContent.Buffs.InsectPoison);

                if (buffCount > 0)
                {
                    cb.RemoveOldestTimedBuff(RRPContent.Buffs.InsectPoison.buffIndex);
                }
                cb.AddTimedBuffAuthority(RRPContent.Buffs.InsectPoison.buffIndex, duration);
            }

        }
    }
}