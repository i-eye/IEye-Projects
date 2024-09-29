
using RoR2;
using UnityEngine;
namespace IEye.RRP
{
    [CreateAssetMenu(fileName = "BodyAssetCollection", menuName = "RRP/AssetCollections/BodyAssetCollection")]
    public class BodyAssetCollection : ExtendedAssetCollection
    {
        public GameObject bodyPrefab;
        public GameObject masterPrefab;
    }
}