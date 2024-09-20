using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using MSU;
using RoR2;
using R2API;
using RoR2.ContentManagement;

namespace IEye.RRP.Scenes
{
    //[DisabledContent]
    public sealed class ProvidenceGarden: RRPScene
    {
        public override RRPAssetRequest<SceneAssetCollection> assetRequest => RRPAssets.LoadAssetAsync<SceneAssetCollection>("acProvidenceGarden", RRPBundle.Indev);
        
        public static MusicTrackDef musicBoss { get; } = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muSong23.asset").WaitForCompletion();
        public static MusicTrackDef musicReg { get; } = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muFULLSong19.asset").WaitForCompletion();
        public override void Initialize()
        {
            sceneDef.mainTrack = musicReg;
            sceneDef.bossTrack = musicBoss;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        private void Stage_onStageStartGlobal(Stage obj)
        {
            if (obj.sceneDef == sceneDef)
            {
                SceneDirector.onPostPopulateSceneServer += SceneDirector_onPostPopulateSceneServer;
            }
        }

        private void SceneDirector_onPostPopulateSceneServer(SceneDirector obj)
        {
            
        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }
    }
}
