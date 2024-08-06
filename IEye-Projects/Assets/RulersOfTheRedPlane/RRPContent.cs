using MSU;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

namespace IEye.RRP
{
    public class RRPContent : IContentPackProvider
    {
        
        public string identifier => RRPMain.GUID;

        public static ReadOnlyContentPack readOnlyContentPack => new ReadOnlyContentPack(RRPContentPack);
        internal static ContentPack RRPContentPack { get; } = new ContentPack();

        internal static ParallelMultiStartCoroutine _parallelPreLoadDispatchers = new ParallelMultiStartCoroutine();
        private static Func<IEnumerator>[] _loadDispatchers;
        internal static ParallelMultiStartCoroutine _parallelPostLoadDispatchers = new ParallelMultiStartCoroutine();

        private static Action[] _fieldAssignDispatchers;
        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            var enumerator = RRPAssets.Initialize();
            while (enumerator.MoveNext())
                yield return null;

            _parallelPreLoadDispatchers.Start();
            while (!_parallelPreLoadDispatchers.IsDone()) yield return null;

            for (int i = 0; i < _loadDispatchers.Length; i++)
            {
                args.ReportProgress(Util.Remap(i + 1, 0f, _loadDispatchers.Length, 0.1f, 0.2f));
                enumerator = _loadDispatchers[i]();

                while (enumerator?.MoveNext() ?? false) yield return null;
            }

            _parallelPostLoadDispatchers.Start();
            while (!_parallelPostLoadDispatchers.IsDone) yield return null;

            for (int i = 0; i < _fieldAssignDispatchers.Length; i++)
            {
                args.ReportProgress(Util.Remap(i + 1, 0f, _fieldAssignDispatchers.Length, 0.95f, 0.99f));
                _fieldAssignDispatchers[i]();
            }
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(RRPContentPack, args.output);
            args.ReportProgress(1f);
            yield return null;
        }

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }

        private void AddSelf(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(this);
        }

        private static IEnumerator LoadFromAssetBundles()
        {
            yield break;
        }

        private IEnumerator AddRRPExpansionDef()
        {
            yield break;
        }

        internal RRPContent()
        {
            ContentManager.collectContentPackProviders += AddSelf;
            RRPAssets.onRRPAssetsInitialized += () =>
            {
                _parallelPreLoadDispatchers.Add(AddRRPExpansionDef);
            };
        }
        static RRPContent()
        {
            RRPMain main = RRPMain.instance;
            _loadDispatchers = new Func<IEnumerator>[]
            {
                () =>
                {
                    ItemModule.AddProvider(main, ContentUtil.CreateContentPieceProvider<ItemDef>(main, RRPContentPack));
                    return ItemModule.InitializeItems(main);
                },
                () =>
                {
                    EquipmentModule.AddProvider(main, ContentUtil.CreateContentPieceProvider<EquipmentDef>(main, RRPContentPack));
                    return ItemTierModule.InitializeTiers(main);
                },
                () =>
                {
                    EquipmentModule.AddProvider(main, ContentUtil.CreateContentPieceProvider<EquipmentDef>(main, RRPContentPack));
                    return CharacterModule.InitializeCharacters(main);
                },
                () =>
                {
                    EquipmentModule.AddProvider(main, ContentUtil.CreateContentPieceProvider<EquipmentDef>(main, RRPContentPack));
                    return ArtifactModule.InitializeArtifacts(main);
                },
                () =>
                {
                    EquipmentModule.AddProvider(main, ContentUtil.CreateContentPieceProvider<EquipmentDef>(main, RRPContentPack));
                    return SceneModule.InitializeScenes(main);
                },
                LoadFromAssetBundles
            };

            _fieldAssignDispatchers = new Action[]
            {
                () => ContentUtil.PopulateTypeFields(typeof(Items), RRPContentPack.itemDefs),
                () => ContentUtil.PopulateTypeFields(typeof(ItemTierDefs), RRPContentPack.itemTierDefs),
                () => ContentUtil.PopulateTypeFields(typeof(Artifacts), RRPContentPack.artifactDefs),
                () => ContentUtil.PopulateTypeFields(typeof(Buffs), RRPContentPack.buffDefs),
                () => ContentUtil.PopulateTypeFields(typeof(Scenes), RRPContentPack.sceneDefs)
            };
        }



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
            public static ItemDef Strawberry;
            //public static ItemDef InnerPiece;

            //lunar
            public static ItemDef DoubleSidedSword;

            //bloody
            public static ItemDef Leech;

            public static ItemDef AgressiveInsect;
            public static ItemDef AdrenalineFrenzy;
            public static ItemDef FocusedHemorrhage;
            public static ItemDef PredatorySavagery;
            public static ItemDef BloodyIvy;

            //hidden
            public static ItemDef SacrificialHelper;
            public static ItemDef Kamikaze;

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
    }
}