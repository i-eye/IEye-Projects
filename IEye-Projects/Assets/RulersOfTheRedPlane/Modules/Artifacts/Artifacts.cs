using R2API;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Moonstorm;

namespace IEye.RRP.Modules
{
    public sealed class Artifacts : ArtifactModuleBase
    {
        public static Artifacts Instance { get; private set; }
        public override R2APISerializableContentPack SerializableContentPack { get; } = RRPContent.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            RRPMain.logger.LogInfo($"Initializing Artifacts");
            GetArtifactBases();
            

            //var compound = RRPAssets.LoadAsset<ArtifactCompoundDef>("acdStar", RRPBundle.Artifacts);
            //compound.decalMaterial.shader = Resources.Load<ArtifactCompoundDef>("artifactcompound/acdCircle").decalMaterial.shader;
            //ArtifactCodeAPI.AddCompound(compound);
        }

        protected override IEnumerable<ArtifactBase> GetArtifactBases()
        {
            base.GetArtifactBases()
                .ToList()
                .ForEach(artifact => AddArtifact(artifact));
            return null;
        }
    }
}