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
    public class AgressiveInsect : ItemBase
    {

        public const string token = "RRP_ITEM_AGROINSECT_DESC";
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("AgressiveInsect", RRPBundle.Items);

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Duration of the insect blood debuff per stack(default 3s)")]
        public static float duration = 3f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Percentage of damage taken away from insect blood debuff target(default 5%)")]
        public static float bloodyInsectDamageCripple = .05f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Armor taken away from the insect blood debuff victim(default 20)")]
        public static int bloodyInsectArmorCripple = 25;

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
                int buffCount = cb.GetBuffCount(RRPContent.Buffs.InsectBloody);

                if (buffCount > 0)
                {
                    cb.RemoveOldestTimedBuff(RRPContent.Buffs.InsectBloody.buffIndex);
                }
                cb.AddTimedBuffAuthority(RRPContent.Buffs.InsectBloody.buffIndex, duration * stack);
            }

        }
    }
}