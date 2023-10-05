using Moonstorm;
using RoR2;
using RoR2.Achievements.Artifacts;

namespace IEye.RRP.Unlocks.Artifacts
{
    public sealed class CognationUnlockable : UnlockableBase
    {
        public override MSUnlockableDef UnlockableDef { get; } = RRPAssets.LoadAsset<MSUnlockableDef>("rrp.artifact.loop", RRPBundle.Artifacts);

        public override void Initialize()
        {
            
            AddRequiredType<RRP.Artifacts.Loop>();
        }

        public sealed class LoopAchievement : BaseObtainArtifactAchievement
        {
            public override ArtifactDef artifactDef => RRPContent.Artifacts.Loop;
        }
    }
}