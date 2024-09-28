using RoR2.Orbs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IEye.RRP.Orbs {
    public class AgressiveInsectMissileOrb : GenericDamageOrb
    {
        public override void Begin()
        {
            speed = 25f;
            base.Begin();
        }

        public override GameObject GetOrbEffect()
        {
            return RRPAssets.LoadAsset<GameObject>("InsectOrbEffect", RRPBundle.Items);
        }
    }
}


