using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using R2API;
using IEye.RRP.Items;

namespace IEye.RRP.Buffs
{
    public class AdrenalineOnGettingHit :  
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("AdrenalineOnGettinghit", RRPBundle.Items);

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
