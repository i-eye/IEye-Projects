using RoR2;
using UnityEngine;
using Moonstorm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace IEye.RulersOfTheRedPlane.ItemTiers
{
    public class Sacrificial : ItemTierBase
    {

        [SerializeField] int index;
        public override ItemTierDef ItemTierDef => RRPAssets.LoadAsset<ItemTierDef>("Sacrificial", RRPBundle.Base);
        public override GameObject PickupDisplayVFX => RRPAssets.LoadAsset<GameObject>("SacrificialPickupDisplayVFX", RRPBundle.Main);

        

        public override void Initialize()
        {
            base.Initialize();
            
            ItemTierDef.highlightPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/HighlightTier1Item.prefab").WaitForCompletion();
            ItemTierDef.dropletDisplayPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/VoidOrb.prefab").WaitForCompletion();
        }
    }
}