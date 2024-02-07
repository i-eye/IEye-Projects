﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Moonstorm;
using RoR2;
using R2API;

namespace IEye.RRP.Scenes
{
    //[DisabledContent]
    public sealed class ProvidenceGarden: SceneBase
    {
        public override SceneDef SceneDef { get; } = RRPAssets.LoadAsset<SceneDef>("rrp_ProvidenceGarden", RRPBundle.Indev);
        public static MusicTrackDef musicBoss { get; } = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muSong23.asset").WaitForCompletion();
        public static MusicTrackDef musicReg { get; } = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muFULLSong19.asset").WaitForCompletion();
        public override void Initialize()
        {
            base.Initialize();
            SceneDef.mainTrack = musicReg;
            SceneDef.bossTrack = musicBoss;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        private void Stage_onStageStartGlobal(Stage obj)
        {
            if (obj.sceneDef == SceneDef)
            {
                SceneDirector.onPostPopulateSceneServer += SceneDirector_onPostPopulateSceneServer;
            }
        }

        private void SceneDirector_onPostPopulateSceneServer(SceneDirector obj)
        {
            
        }
    }
}