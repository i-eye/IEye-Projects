﻿using MSU;
using R2API;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RoR2.ContentManagement;
using IEye.RRP;

namespace IEye.RRP
{
    /// <summary>
    /// <inheritdoc cref="IMonsterContentPiece"/>
    /// </summary>
    public abstract class RRPMonster : IMonsterContentPiece, IContentPackModifier
    {
        public NullableRef<MonsterCardProvider> CardProvider { get; protected set; }
        public NullableRef<DirectorCardHolderExtended> DissonanceCard { get; protected set; }
        public MonsterAssetCollection AssetCollection { get; private set; }
        public NullableRef<GameObject> masterPrefab { get; protected set; }

        NullableRef<DirectorCardHolderExtended> IMonsterContentPiece.dissonanceCard => DissonanceCard;
        CharacterBody IGameObjectContentPiece<CharacterBody>.component => CharacterPrefab.GetComponent<CharacterBody>();
        NullableRef<MonsterCardProvider> IMonsterContentPiece.cardProvider => CardProvider;
        GameObject IContentPiece<GameObject>.asset => CharacterPrefab;
        public GameObject CharacterPrefab { get; private set; }

        public abstract void Initialize();
        public abstract bool IsAvailable(ContentPack contentPack);

        public abstract RRPAssetRequest<MonsterAssetCollection> AssetRequest { get; }



        public virtual IEnumerator LoadContentAsync()
        {
            RRPAssetRequest<MonsterAssetCollection> request = AssetRequest;

            request.StartLoad();
            while (!request.isComplete)
                yield return null;

            AssetCollection = request.asset;

            CharacterPrefab = AssetCollection.bodyPrefab;
            masterPrefab = AssetCollection.masterPrefab;
            CardProvider = AssetCollection.monsterCardProvider;
            DissonanceCard = AssetCollection.dissonanceCardHolder;
        }


        public virtual void ModifyContentPack(ContentPack contentPack)
        {
            contentPack.AddContentFromAssetCollection(AssetCollection);
        }
    }
}