using RoR2;
using UnityEngine;
using System.Collections.Generic;

namespace IEye.RRP
{
    [CreateAssetMenu(fileName = "ItemAssetCollection", menuName = "RRP/AssetCollections/ItemAssetCollection")]
    public class ItemAssetCollection : ExtendedAssetCollection
    {
        public List<GameObject> itemDisplayPrefabs;
        public ItemDef itemDef;
    }
}