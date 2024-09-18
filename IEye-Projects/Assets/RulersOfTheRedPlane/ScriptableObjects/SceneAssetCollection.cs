using RoR2;
using UnityEngine;

namespace IEye.RRP
{
    [CreateAssetMenu(fileName = "SceneAssetCollection", menuName = "RRP/AssetCollections/SceneAssetCollection")]
    public class SceneAssetCollection : ExtendedAssetCollection
    {
        public SceneDef sceneDef;
    }
}
