using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RoR2.ExplicitPickupDropTable;
using MSU;
using RoR2.ContentManagement;

namespace IEye.RRP.Monsters
{
    //[DisabledContent]
    public sealed class Wlurm : RRPMonster
    {
        public override RRPAssetRequest<MonsterAssetCollection> AssetRequest => RRPAssets.LoadAssetAsync<MonsterAssetCollection>("acWlurm", RRPBundle.Monsters);

        public static GameObject _masterPrefab;
        public override void Initialize()
        {
            _masterPrefab = AssetCollection.FindAsset<GameObject>("WlurmMaster");
            
        }
        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }
    }
}
