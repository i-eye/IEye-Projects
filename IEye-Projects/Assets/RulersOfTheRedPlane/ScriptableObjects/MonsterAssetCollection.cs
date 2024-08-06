using RoR2;
using UnityEngine;
using MSU;
using System.Collections.Generic;
using IEye.RRP;
namespace IEye.RRP
{
    [CreateAssetMenu(fileName = "MonsterAssetCollection", menuName = "RRPMod/AssetCollections/MonsterAssetCollection")]
    public class MonsterAssetCollection : BodyAssetCollection
    {
        public MonsterCardProvider monsterCardProvider;
        public R2API.DirectorAPI.DirectorCardHolder dissonanceCardHolder;
    }
}