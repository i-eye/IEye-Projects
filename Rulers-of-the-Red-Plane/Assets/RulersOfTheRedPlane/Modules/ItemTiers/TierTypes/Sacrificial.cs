using RoR2;
using UnityEngine;
using Moonstorm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Linq;

namespace IEye.RRP.ItemTiers
{
    public class Sacrificial : ItemTierBase
    {

        [SerializeField] int index;
        public override ItemTierDef ItemTierDef => RRPAssets.LoadAsset<ItemTierDef>("Sacrificial", RRPBundle.Base);
        public static ItemTierDef def = RRPAssets.LoadAsset<ItemTierDef>("Sacrificial", RRPBundle.Base);
        public override GameObject PickupDisplayVFX => RRPAssets.LoadAsset<GameObject>("SacrificialPickupDisplayVFX", RRPBundle.Base);

        

        public override void Initialize()
        {
            base.Initialize();
            
            ItemTierDef.highlightPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/HighlightTier1Item.prefab").WaitForCompletion();
            ItemTierDef.dropletDisplayPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/VoidOrb.prefab").WaitForCompletion();
        }
        public static List<ItemDef> ItemDefsWithTier()
        {
            List<ItemDef> items = new List<ItemDef>();
            foreach(ItemDef itemDef in ItemCatalog.allItemDefs){
                if(itemDef.tier == def.tier && !itemDef.tags.Contains(ItemTag.WorldUnique))
                {
                    items.AddIfNotInCollection(itemDef);
                }
            }
            return items;
        }
    }
}