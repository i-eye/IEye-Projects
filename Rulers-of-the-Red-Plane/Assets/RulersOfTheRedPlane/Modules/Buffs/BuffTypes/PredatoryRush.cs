using Moonstorm.Components;
using IEye.RRP.Items;
using R2API;
using RoR2;
using Moonstorm;
using System;

namespace IEye.RRP.Buffs
{
    public class PredatoryRush : BuffBase
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("PredatoryRush", RRPBundle.Items);


        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier, IOnTakeDamageServerReceiver
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.PredatoryRush;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                int stack = body.inventory.GetItemCount(RRPContent.Items.PredatorySavagery);
                args.damageMultAdd += PredatorySavagery.damage / 100 * stack;
                if((PredatorySavagery.cooldownReduction * stack) < 50)
                {
                    args.cooldownMultAdd -= PredatorySavagery.cooldownReduction / 100 * stack;
                } else
                {
                    args.cooldownMultAdd -= .5f;
                }
                
                args.armorAdd += PredatorySavagery.armor * stack;

                args.jumpPowerMultAdd += PredatorySavagery.jumpMult / 100;

                args.critAdd += PredatorySavagery.critChance;
                args.critDamageMultAdd += PredatorySavagery.critDamage / 100 * stack;
            }

            public void OnTakeDamageServer(DamageReport damageReport)
            {
                //body.RemoveBuff(RRPContent.Buffs.PredatoryRush);
            }
        }
    }

}