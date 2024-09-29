using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Artifacts;
using R2API.ScriptableObjects;

namespace IEye.RRP
{
    [CreateAssetMenu(fileName = "ArtifactAssetCollection", menuName = "RRP/AssetCollections/ArtifactAssetCollection")]
    public class ArtifactAssetCollection : ExtendedAssetCollection
    {
        public ArtifactCode artifactCode;
        public ArtifactDef artifactDef;
    }
}