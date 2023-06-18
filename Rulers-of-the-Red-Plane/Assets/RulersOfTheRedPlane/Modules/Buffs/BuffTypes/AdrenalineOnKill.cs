using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using Moonstorm.Components;
using R2API;
using IEye.RulersOfTheRedPlane.Items;

namespace IEye.RulersOfTheRedPlane.Buffs
{
    
    public class AdrenalineOnKill : BuffBase
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("AdrenalineOnKill");

        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.AdrenalineOnKill;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                int stack = body.inventory.GetItemCount(RRPContent.Items.AdrenalineFrenzy);
                args.sprintSpeedAdd += (AdrenalineFrenzy.killSpeed / 100 + (AdrenalineFrenzy.killSpeedStack / 100 * (stack - 1))) * buffStacks;
            }
        }

    }
}
