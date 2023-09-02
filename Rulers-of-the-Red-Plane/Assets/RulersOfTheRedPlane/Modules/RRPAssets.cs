using Moonstorm.Loaders;
using Moonstorm;
using RoR2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;
using Path = System.IO.Path;

namespace IEye.RRP
{
    public enum RRPBundle
    {
        Invalid,
        All,
        Main,
        Base,
        Artifacts,
        Equipments,
        Interactables,
        Items,
        Events,
        Vanilla,
        Indev,
        Shared
    }
    public class RRPAssets : AssetsLoader<RRPAssets>
    {
        private const string ASSET_BUNDLE_FOLDER_NAME = "assetbundles";
        private const string MAIN = "rrpmain";
        private const string BASE = "rrpbase";
        private const string ARTIFACTS = "rrpartifacts";
        private const string EQUIPS = "rrpequipments";
        private const string INTERACTS = "rrpinteractables";
        private const string ITEMS = "rrpitems";
        private const string VANILLA = "rrpvanilla";
        private const string DEV = "rrpdev";
        private const string SHARED = "rrpshared";

        private static Dictionary<RRPBundle, AssetBundle> assetBundles = new Dictionary<RRPBundle, AssetBundle>();

        public new static TAsset LoadAsset<TAsset>(string name) where TAsset : UnityEngine.Object
        {
#if DEBUG
            var stackTrace = new StackTrace();
            var method = stackTrace.GetFrame(1).GetMethod();
            RRPMain.logger.LogWarning($"Assembly {Assembly.GetCallingAssembly()} is trying to load an asset of name {name} and type {typeof(TAsset).Name} without specifying what bundle to use for loading. This causes large performance loss as SS2Assets has to search thru the entire bundle collection. Avoid calling LoadAsset without specifying the AssetBundle. (Method: {method.DeclaringType.FullName}.{method.Name}()");
#endif
            return LoadAsset<TAsset>(name, RRPBundle.All);
        }

        public new static TAsset[] LoadAllAssetsOfType<TAsset>(string name) where TAsset : UnityEngine.Object
        {
#if DEBUG
            var stackTrace = new StackTrace();
            var method = stackTrace.GetFrame(1).GetMethod();
            RRPMain.logger.LogWarning($"Assembly {Assembly.GetCallingAssembly()} is trying to load an asset of name {name} and type {typeof(TAsset).Name} without specifying what bundle to use for loading. This causes large performance loss as SS2Assets has to search thru the entire bundle collection. Avoid calling LoadAsset without specifying the AssetBundle. (Method: {method.DeclaringType.FullName}.{method.Name}()");
#endif
            return LoadAllAssetsOfType<TAsset>(RRPBundle.All);
        }

        public static TAsset LoadAsset<TAsset>(string name, RRPBundle bundle) where TAsset : UnityEngine.Object
        {
            if (Instance == null)
            {
                return null;
            }
            return Instance.LoadAssetInternal<TAsset>(name, bundle);
        }

        public static TAsset[] LoadAllAssetsOfType<TAsset>(RRPBundle bundle) where TAsset : UnityEngine.Object
        {
            if (Instance == null)
            {
                RRPMain.logger.LogError("Cannot load asset when ther's not instance of RRPAssets!");
                return null;
            }
            return Instance.LoadAllAssetsOfTypeInternal<TAsset>(bundle);
        }
        public override AssetBundle MainAssetBundle => GetAssetBundle(RRPBundle.Main);

        public string AssemblyDir => Path.GetDirectoryName(RRPMain.pluginInfo.Location);

        public AssetBundle GetAssetBundle(RRPBundle bundle)
        {
            return assetBundles[bundle];
        }


        internal void Init()
        {
            var bundlePaths = GetAssetBundlePaths();
            foreach (string path in bundlePaths)
            {
                var fileName = Path.GetFileName(path);
                switch (fileName)
                {
                    case MAIN: LoadBundle(path, RRPBundle.Main); break;
                    case BASE: LoadBundle(path, RRPBundle.Base); break;
                    case ARTIFACTS: LoadBundle(path, RRPBundle.Artifacts); break;
                    case EQUIPS: LoadBundle(path, RRPBundle.Equipments); break;
                    case INTERACTS: LoadBundle(path, RRPBundle.Interactables); break;
                    case ITEMS: LoadBundle(path, RRPBundle.Items); break;
                    case VANILLA: LoadBundle(path, RRPBundle.Vanilla); break;
                    case DEV: LoadBundle(path, RRPBundle.Indev); break;
                    case SHARED: LoadBundle(path, RRPBundle.Shared); break;
                    default: RRPMain.logger.LogWarning($"Invalid or Unexpected file in the AssetBundles folder (File name: {fileName}, Path: {path})"); break;
                }
            }

            void LoadBundle(string path, RRPBundle bundleEnum)
            {
                try
                {
                    AssetBundle bundle = AssetBundle.LoadFromFile(path);
                    if (!bundle)
                    {
                        throw new FileLoadException("AssetBundle.LoadFromFile did not return an asset bundle");
                    }

                    if (assetBundles.ContainsKey(bundleEnum))
                    {
                        throw new InvalidOperationException($"AssetBundle in path loaded succesfully, but the assetBundles dictionary already contains an entry for {bundleEnum}.");
                    }

                    assetBundles[bundleEnum] = bundle;
                }
                catch (Exception e)
                {
                    RRPMain.logger.LogError($"Could not load assetbundle at path {path} and assign to enum {bundleEnum}. {e}");
                }
            }
        }

        private TAsset LoadAssetInternal<TAsset>(string name, RRPBundle bundle) where TAsset : UnityEngine.Object
        {
            TAsset asset = null;
            if (bundle == RRPBundle.All)
            {
                asset = FindAsset<TAsset>(name, out RRPBundle foundInBundle);
#if DEBUG
                if (!asset)
                {
                    RRPMain.logger.LogWarning($"Could not find asset of type {typeof(TAsset).Name} with name {name} in any of the bundles.");
                }
                else
                {
                    RRPMain.logger.LogInfo($"Asset of type {typeof(TAsset).Name} was found inside bundle {foundInBundle}, it is recommended that you load the asset directly");
                }
#endif
                return asset;
            }

            asset = assetBundles[bundle].LoadAsset<TAsset>(name);
#if DEBUG
            if (!asset)
            {
                var stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1).GetMethod();

                RRPMain.logger.LogWarning($"The  method \"{method.DeclaringType.FullName}.{method.Name}()\" is calling \"LoadAsset<TAsset>(string, SS2Bundle)\" with the arguments \"{typeof(TAsset).Name}\", \"{name}\" and \"{bundle}\", however, the asset could not be found.\n" +
                    $"A complete search of all the bundles will be done and the correct bundle enum will be logged.");
                return LoadAssetInternal<TAsset>(name, RRPBundle.All);
            }
#endif
            return asset;

            TAsset FindAsset<TAsset>(string assetName, out RRPBundle foundInBundle) where TAsset : UnityEngine.Object
            {
                foreach ((var enumVal, var assetBundle) in assetBundles)
                {
                    var loadedAsset = assetBundle.LoadAsset<TAsset>(assetName);
                    if (loadedAsset)
                    {
                        foundInBundle = enumVal;
                        return loadedAsset;
                    }
                }
                foundInBundle = RRPBundle.Invalid;
                return null;
            }
        }

        private TAsset[] LoadAllAssetsOfTypeInternal<TAsset>(RRPBundle bundle) where TAsset : UnityEngine.Object
        {
            List<TAsset> loadedAssets = new List<TAsset>();
            if (bundle == RRPBundle.All)
            {
                FindAssets<TAsset>(loadedAssets);
#if DEBUG
                if (loadedAssets.Count == 0)
                {
                    RRPMain.logger.LogWarning($"Could not find any asset of type {typeof(TAsset)} inside any of the bundles");
                }
#endif
                return loadedAssets.ToArray();
            }

            loadedAssets = assetBundles[bundle].LoadAllAssets<TAsset>().ToList();
#if DEBUG
            if (loadedAssets.Count == 0)
            {
                RRPMain.logger.LogWarning($"Could not find any asset of type {typeof(TAsset)} inside the bundle {bundle}");
            }
#endif
            return loadedAssets.ToArray();

            void FindAssets<TAsset>(List<TAsset> output) where TAsset : UnityEngine.Object
            {
                foreach ((var _, var bndl) in assetBundles)
                {
                    output.AddRange(bndl.LoadAllAssets<TAsset>());
                }
                return;
            }
        }

        internal void SwapMaterialShaders()
        {
            SwapShadersFromMaterials(LoadAllAssetsOfType<Material>(RRPBundle.All).Where(mat => mat.shader.name.StartsWith("Stubbed")));
        }

        internal void FinalizeCopiedMaterials()
        {
            foreach (var (_, bundle) in assetBundles)
            {
                FinalizeMaterialsWithAddressableMaterialShader(bundle);
            }
        }
        private string[] GetAssetBundlePaths()
        {
            return Directory.GetFiles(Path.Combine(AssemblyDir, ASSET_BUNDLE_FOLDER_NAME))
               .Where(filePath => !filePath.EndsWith(".manifest"))
               .ToArray();
        }


    }
}