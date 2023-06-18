using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using R2API;
using Moonstorm.Components;
using IEye.RulersOfTheRedPlane.Items;

namespace IEye.RulersOfTheRedPlane.Buffs
{
    public class AdrenalineOnGettingHit : BuffBase
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("AdrenalineOnGettinghit");

        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.AdrenalineOnGettingHit;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                int stack = body.inventory.GetItemCount(RRPContent.Items.AdrenalineFrenzy);
                args.moveSpeedMultAdd += (AdrenalineFrenzy.onHitSpeed / 100 + (AdrenalineFrenzy.onHitSpeedStack / 100 * (stack - 1))) * buffStacks;
            }
        }
    }
}
