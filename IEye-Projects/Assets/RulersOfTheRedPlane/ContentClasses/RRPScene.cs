﻿using IEye.RRP;
using MSU;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.ContentManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
namespace IEye.RRP
{
    public abstract class RRPScene : ISceneContentPiece, IContentPackModifier
    {
        public SceneAssetCollection assetCollection { get; private set; }
        public abstract void Initialize();
        public abstract bool IsAvailable(ContentPack contentPack);

        public abstract RRPAssetRequest<SceneAssetCollection> assetRequest { get; }

        NullableRef<MusicTrackDef> ISceneContentPiece.mainTrack => mainTrack;
        NullableRef<MusicTrackDef> ISceneContentPiece.bossTrack => bossTrack;

        public MusicTrackDef mainTrack { get; protected set; }

        public MusicTrackDef bossTrack { get; protected set; }

        public NullableRef<Texture2D> bazaarTextureBase { get; protected set; } // ???

        SceneDef IContentPiece<SceneDef>.asset => sceneDef;

        public SceneDef sceneDef { get; protected set; }

        public virtual float? weightRelativeToSiblings { get; protected set; } = 1;

        public virtual bool? preLoop { get; protected set; } = true;

        public virtual bool? postLoop { get; protected set; } = true;

        public virtual IEnumerator LoadContentAsync()
        {
            RRPAssetRequest<SceneAssetCollection> request = assetRequest;

            request.StartLoad();
            while (!request.isComplete)
                yield return null;

            assetCollection = request.asset;

            sceneDef = assetCollection.sceneDef;
            if(mainTrack) mainTrack = assetCollection.mainTrack;
            if(bossTrack) bossTrack = assetCollection.bossTrack;

            weightRelativeToSiblings = assetCollection.stageWeightRelativeToSiblings;
            preLoop = assetCollection.appearsPreLoop;
            postLoop = assetCollection.appearsPostLoop;

        }


        public virtual void ModifyContentPack(ContentPack contentPack)
        {
            contentPack.AddContentFromAssetCollection(assetCollection);
        }

        public virtual void OnServerStageComplete(Stage stage)
        {
        }

        public virtual void OnServerStageBegin(Stage stage)
        {
        }
    }
}