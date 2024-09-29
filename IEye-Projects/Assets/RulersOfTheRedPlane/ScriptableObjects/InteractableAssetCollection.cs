using RoR2;
using UnityEngine;
using System.Collections.Generic;
using MSU;

namespace IEye.RRP
{
    [CreateAssetMenu(fileName = "InteractableAssetCollection", menuName = "RRP/AssetCollections/InteractableAssetCollection")]
    public class InteractableAssetCollection : ExtendedAssetCollection
    {
        public GameObject interactablePrefab;
        public InteractableCardProvider interactableCardProvider;
    }
}