using RoR2;
using RoR2.Achievements.Artifacts;

namespace IEye.RRP.Unlocks.Artifacts
{
    public sealed class LoopUnlockable : BaseObtainArtifactAchievement
    {
        public override ArtifactDef artifactDef => RRPContent.Artifacts.Loop;
    }
}