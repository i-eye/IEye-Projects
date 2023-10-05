using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using R2API;
using Moonstorm.Components;
using IEye.RRP.Items;
using R2API.Networking;

namespace IEye.RRP.Buffs
{
    public class IvyBlight : BuffBase
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("IvyBlight", RRPBundle.Items);

        public sealed class Behavior : BaseBuffBodyBehavior, IOnDamageDealtServerReceiver
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.IvyBlight;

            public void OnDamageDealtServer(DamageReport damageReport)
            {
                if(Util.CheckRoll(15f, body.master)){
                    damageReport.victim.ApplyDot(body.gameObject, DotController.DotIndex.Blight, 4f, .75f);
                }
            }
        }
    }
}
