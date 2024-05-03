using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RoR2.ExplicitPickupDropTable;
using Moonstorm;

namespace IEye.RRP.Monsters
{
    //[DisabledContent]
    public sealed class LampBoss : MonsterBase
    {
        public override GameObject BodyPrefab { get; } = RRPAssets.LoadAsset<GameObject>("WlurmBossBody", RRPBundle.Indev);
        public override GameObject MasterPrefab { get; } = RRPAssets.LoadAsset<GameObject>("WlurmMaster", RRPBundle.Indev);
        //public override MSMonsterDirectorCardHolder directorCards { get; set; } = Assets.Instance.MainAssetBundle.LoadAsset<MSMonsterDirectorCardHolder>("WayfarerCardHolder");
        //public override MSMonsterDirectorCard MonsterDirectorCard { get; } = SS2Assets.LoadAsset<MSMonsterDirectorCard>("msmdcLampBoss", SS2Bundle.Indev);

        private MSMonsterDirectorCard defaultCard = RRPAssets.LoadAsset<MSMonsterDirectorCard>("msmdcWlurm", RRPBundle.Indev);

        internal static GameObject wayfarerBuffWardPrefab;

        public override void Initialize()
        {
            base.Initialize();
            MonsterDirectorCards.Add(defaultCard);


            
        }
    }
}
