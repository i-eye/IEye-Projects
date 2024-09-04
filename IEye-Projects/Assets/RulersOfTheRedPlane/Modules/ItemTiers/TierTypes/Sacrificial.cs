using RoR2;
using UnityEngine;
using MSU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Linq;
using RoR2.ContentManagement;

namespace IEye.RRP.ItemTiers
{
    public class Sacrificial : RRPItemTier, IContentPackModifier
    {
        //[RiskOfOptionsConfigureField(RRPConfig.IDItemTier, ConfigDescOverride = "Multiplied by difficulty to calculate number of kills needed for sacrifice(default 3.8).")]
        public static float multiplier = 3.8f;

        //[RiskOfOptionsConfigureField(RRPConfig.IDItemTier, ConfigDescOverride = "Max number of kills for a sacrifice(lowers to this when above)(default 34).")]
        public static int cap = 34;



        public override RRPAssetRequest<ItemTierAssetCollection> AssetRequest => RRPAssets.LoadAssetAsync<ItemTierAssetCollection>("acSacrificial", RRPBundle.Base);

        //public override ItemTierDef ItemTierDef => RRPAssets.LoadAsset<ItemTierDef>("Sacrificial", RRPBundle.Base);
        public static ItemTierDef def = RRPAssets.LoadAsset<ItemTierDef>("Sacrificial", RRPBundle.Base);
        //public override GameObject PickupDisplayVFX => RRPAssets.LoadAsset<GameObject>("SacrificialPickupDisplayVFX", RRPBundle.Base);

        

        public override void Initialize()
        {
        }

        public override IEnumerator LoadContentAsync()
        {
            var enumerator = base.LoadContentAsync();
            while (!enumerator.IsDone())
                yield return null;

            var request = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/VoidOrb.prefab");
            var request2 = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/HighlightTier1Item.prefab");
            while (!request.IsDone && !request2.IsDone)
                yield return null;

            ItemTierDef.dropletDisplayPrefab = request.Result;
            ItemTierDef.highlightPrefab = request2.Result;
            yield break;
        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }
        public static List<ItemDef> ItemDefsWithTier()
        {
            List<ItemDef> items = new List<ItemDef>();
            foreach(ItemDef itemDef in ItemCatalog.allItemDefs){
                if(itemDef.tier == def.tier && !itemDef.tags.Contains(ItemTag.WorldUnique))
                {
                    if (!items.Contains(itemDef))
                    {
                        items.Add(itemDef);
                    }
                    
                }
            }
            return items;
        }
    }
}