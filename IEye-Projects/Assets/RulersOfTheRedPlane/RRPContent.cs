using Moonstorm.Loaders;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Linq;
using UnityEngine;

namespace IEye.RRP
{
    public class RRPContent : ContentLoader<RRPContent>
    {
        public static class Artifacts
        {
            public static ArtifactDef Loop;
        }
        public static class Items
        {
            //white
            public static ItemDef FourDimensionalDagger;
            
            //green
            public static ItemDef IntrospectiveInsect;
            public static ItemDef PoisonIvy;
            
            //red
            //public static ItemDef InnerPiece;

            //lunar
            public static ItemDef DoubleSidedSword;

            //bloody
            public static ItemDef AgressiveInsect;
            public static ItemDef AdrenalineFrenzy;
            public static ItemDef FocusedHemorrhage;
            public static ItemDef PredatorySavagery;
            public static ItemDef BloodyIvy;

            //hidden
            public static ItemDef SacrificialHelper;
            
        }
        public static class Scenes
        {
            public static SceneDef ProvidenceGarden;
        }
        public static class Buffs
        {
            //debuffs
            public static BuffDef InsectPoison;
            public static BuffDef InsectBloody;

            //buffs
            public static BuffDef AdrenalineOnGettingHit;
            public static BuffDef AdrenalineOnKill;
            public static BuffDef PredatoryRush;
            public static BuffDef IvyPower;
            public static BuffDef IvyBlight;
        }
        public static class ItemTierDefs
        {
            public static ItemTierDef Sacrificial;
        }
        public override string identifier => RRPMain.GUID;

        public override R2APISerializableContentPack SerializableContentPack { get; protected set; } = RRPAssets.LoadAsset<R2APISerializableContentPack>("ContentPack", RRPBundle.Main);

        public override Action[] LoadDispatchers { get; protected set; }

        public override Action[] PopulateFieldsDispatchers { get; protected set; }

        public override void Init()
        {
            base.Init();

            LoadDispatchers = new Action[] 
            {
            delegate
            {
                new Modules.Items().Initialize();
            },
            delegate
            {
                new Modules.Buffs().Initialize();
            },
            delegate
            {
                new Modules.ItemTiers().Initialize();
            },
            delegate
            {
                new Modules.Interactables().Initialize();   
            },
            delegate
            {
                new Modules.Artifacts().Initialize();
            },
            delegate
            {
                new Modules.Unlockables().Initialize();
            },
            delegate
            {
                new Modules.Scenes().Initialize();
            },
            delegate
            {
                RRPMain.logger.LogInfo($"Populating entity state array");
                GetType().Assembly.GetTypes()
                                      .Where(type => typeof(EntityStates.EntityState).IsAssignableFrom(type))
                                      .ToList()
                                      .ForEach(state => HG.ArrayUtils.ArrayAppend(ref SerializableContentPack.entityStateTypes, new EntityStates.SerializableEntityStateType(state)));
            },
            
            delegate
            {
                    RRPMain.logger.LogInfo($"Populating EntityStateConfigurations");
                    SerializableContentPack.entityStateConfigurations = RRPAssets.LoadAllAssetsOfType<EntityStateConfiguration>(RRPBundle.All);
            },
            delegate{
                RRPMain.logger.LogInfo($"Populating effect prefabs");
                SerializableContentPack.effectPrefabs = SerializableContentPack.effectPrefabs.Concat(RRPAssets.LoadAllAssetsOfType<GameObject>(RRPBundle.All)
                .Where(go => go.GetComponent<EffectComponent>()))
                .ToArray();
            },
            delegate
            {
                RRPMain.logger.LogInfo($"Swapping material shaders");
                RRPAssets.Instance.SwapMaterialShaders();
                RRPAssets.Instance.FinalizeCopiedMaterials();
            }
            };
            PopulateFieldsDispatchers = new Action[]
            {
                delegate
                {
                    PopulateTypeFields(typeof(Artifacts), ContentPack.artifactDefs);
                },
                delegate
                {
                    PopulateTypeFields(typeof(Items), ContentPack.itemDefs);
                },
                delegate
                {
                    PopulateTypeFields(typeof(Buffs), ContentPack.buffDefs);
                },
                delegate
                {
                    PopulateTypeFields(typeof(ItemTierDefs), ContentPack.itemTierDefs);
                },
                delegate
                {
                    PopulateTypeFields(typeof(Scenes), ContentPack.sceneDefs);
                }
            };
        }
    }
}