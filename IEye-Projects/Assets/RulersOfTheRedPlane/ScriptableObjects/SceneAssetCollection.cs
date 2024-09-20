using MSU;
using RoR2;
using UnityEngine;

namespace IEye.RRP
{
    [CreateAssetMenu(fileName = "SceneAssetCollection", menuName = "RRP/AssetCollections/SceneAssetCollection")]
    public class SceneAssetCollection : ExtendedAssetCollection
    {
        public SceneDef sceneDef;
        public NullableRef<MusicTrackDef> mainTrack;
        public NullableRef<MusicTrackDef> bossTrack;

        [Header("Stage Registration Metadata")]
        public float stageWeightRelativeToSiblings;
        public bool appearsPreLoop;
        public bool appearsPostLoop;
    }
}
