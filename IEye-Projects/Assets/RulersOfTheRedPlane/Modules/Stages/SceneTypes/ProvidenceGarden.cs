using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonstorm;
using RoR2;

namespace IEye.RRP.Scenes
{
    public sealed class ProvidenceGarden: SceneBase
    {
        public override SceneDef SceneDef { get; } = RRPAssets.LoadAsset<SceneDef>("rrp_ProvidenceGarden", RRPBundle.Indev);


    }
}
