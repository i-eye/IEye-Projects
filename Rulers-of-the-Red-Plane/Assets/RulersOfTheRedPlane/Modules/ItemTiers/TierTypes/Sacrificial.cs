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
        [RooConfigurableField(RRPConfig.IDItemTier, ConfigDesc = "Multiplied by difficulty to calculate number of kills needed for sacrifice(default 2.2).")]
        public static float multiplier = 2.2f;

        [RooConfigurableField(RRPConfig.IDItemTier, ConfigDesc = "Max number of kills for a sacrifice(lowers to this when above)(default 33).")]
        public static int cap = 33;




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