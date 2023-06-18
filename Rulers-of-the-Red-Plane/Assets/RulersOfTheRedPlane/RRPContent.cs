using Moonstorm.Loaders;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Linq;
using UnityEngine;

namespace IEye.RulersOfTheRedPlane
{
    public class RRPContent : ContentLoader<RRPContent>
    {
        public static class Artifacts
        {

        }
        public static class Items
        {
            public static ItemDef FourDimensionalDagger;
            public static ItemDef DoubleSidedSword;
            public static ItemDef IntrospectiveInsect;
            public static ItemDef AgressiveInsect;
            public static ItemDef AdrenalineFrenzy;
            public static ItemDef InnerPiece;

            public static ItemDef SacrificialHelper;
        }
        public static class Buffs
        {
            public static BuffDef InsectPoison;
            public static BuffDef InsectBloody;
            public static BuffDef AdrenalineOnGettingHit;
            public static BuffDef AdrenalineOnKill;
        }
        public static class ItemTierDefs
        {
            public static ItemTierDef Sacrificial;
        }
        public override string identifier => RulersOfTheRedPlaneMain.GUID;

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
                DefNotSS2Log.Info($"Populating entity state array");
                GetType().Assembly.GetTypes()
                                      .Where(type => typeof(EntityStates.EntityState).IsAssignableFrom(type))
                                      .ToList()
                                      .ForEach(state => HG.ArrayUtils.ArrayAppend(ref SerializableContentPack.entityStateTypes, new EntityStates.SerializableEntityStateType(state)));
            },
            delegate{
                    DefNotSS2Log.Info($"Populating effect prefabs");
                    SerializableContentPack.effectPrefabs = SerializableContentPack.effectPrefabs.Concat(RRPAssets.LoadAllAssetsOfType<GameObject>(RRPBundle.All)
                    .Where(go => go.GetComponent<EffectComponent>()))
                    .ToArray();
            },
            delegate
                {
                    DefNotSS2Log.Info($"Populating EntityStateConfigurations");
                    SerializableContentPack.entityStateConfigurations = RRPAssets.LoadAllAssetsOfType<EntityStateConfiguration>(RRPBundle.All);
                },
                delegate
                {
                    DefNotSS2Log.Info($"Swapping material shaders");
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
                }
            };
        }
    }
}