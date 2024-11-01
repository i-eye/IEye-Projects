﻿using RoR2;
using UnityEngine;
using MSU;
using System.Collections.Generic;
using IEye.RRP;
namespace IEye.RRP
{
    [CreateAssetMenu(fileName = "MonsterAssetCollection", menuName = "RRP/AssetCollections/MonsterAssetCollection")]
    public class MonsterAssetCollection : BodyAssetCollection
    {
        public MonsterCardProvider monsterCardProvider;
        public DirectorCardHolderExtended dissonanceCardHolder;
    }
}