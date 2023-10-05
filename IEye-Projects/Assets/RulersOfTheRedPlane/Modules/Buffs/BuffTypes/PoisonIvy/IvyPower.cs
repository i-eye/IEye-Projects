using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using R2API;
using Moonstorm.Components;
using IEye.RRP.Items;

namespace IEye.RRP.Buffs
{
    public class IvyPower : BuffBase
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("IvyPower", RRPBundle.Items);

        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.IvyPower;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.damageMultAdd += 20f;
                args.critDamageMultAdd += 25f;
                args.attackSpeedMultAdd += 20f;
            }
        }
    }
}
