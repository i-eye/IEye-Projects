using MSU;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;
using Path = System.IO.Path;
using UObject = UnityEngine.Object;

namespace IEye.RRP
{
    public enum RRPBundle
    {
        Invalid,
        All,
        Main,
        Base,
        Artifacts,
        //Equipments,
        Interactables,
        Items,
        //Monsters,
        //Events,
        //Vanilla,
        StreamedScene,
        Indev,
        //Shared
    }
    public class RRPAssets
    {
        private const string ASSET_BUNDLE_FOLDER_NAME = "assetbundles";
        private const string MAIN = "rrpmain";
        private const string BASE = "rrpbase";
        private const string ARTIFACTS = "rrpartifacts";
        private const string EQUIPS = "rrpequipments";
        private const string INTERACTS = "rrpinteractables";
        private const string ITEMS = "rrpitems";
        private const string MONSTERS = "rrpmonsters";
        private const string VANILLA = "rrpvanilla";
        private const string STAGES = "rrpstages";
        private const string INDEV = "rrpindev";
        private const string SHARED = "rrpshared";

        //Property for obtaining the Folder where all the asset bundles are located.
        private static string AssetBundleFolderPath => Path.Combine(Path.GetDirectoryName(RRPMain.instance.Info.Location), ASSET_BUNDLE_FOLDER_NAME);

        /*
         * The main system for managing multiple bundles at once, is simply assigning each non streamed scene bundle an
         * Enum value. Streamed Scene bundles are added to the array below, as these asset bundles cant be used for 
         * loading assets.
         */
        private static Dictionary<RRPBundle, AssetBundle> _assetBundles = new Dictionary<RRPBundle, AssetBundle>();
        private static AssetBundle[] _streamedSceneBundles = Array.Empty<AssetBundle>();

        /// <summary>
        /// Fired when all the AssetBundles from the mod are loaded into memory, this in turn gets fired during Content Pack Loading and as such should be used to implement new async loading calls to <see cref="RRPContent._parallelPreLoadDispatchers"/>
        /// </summary>
        public static event Action onRRPAssetsInitialized
        {
            add
            {
                _onRRPAssetsInitialized -= value;
                _onRRPAssetsInitialized += value;
            }
            remove
            {
                _onRRPAssetsInitialized -= value;
            }
        }
        private static Action _onRRPAssetsInitialized;

        /// <summary>
        /// Returns the AssetBundle that's tied to the supplied enum value.
        /// </summary>
        /// <param name="bundle">The bundle to obtain</param>
        /// <returns>The tied assetbundle, null if the enum value is All, Invalid or Streamed Scene</returns>
        public static AssetBundle GetAssetBundle(RRPBundle bundle)
        {
            if (bundle == RRPBundle.All || bundle == RRPBundle.Invalid || bundle == RRPBundle.StreamedScene)
            {
                return null;
            }

            return _assetBundles[bundle];
        }

        /// <summary>
        /// Loads an asset of type <typeparamref name="TAsset"/> and name <paramref name="name"/> from the asset bundle specified by <paramref name="bundle"/>.
        /// <para>See also <see cref="LoadAssetAsync{TAsset}(string, RRPBundle)"/></para>
        /// </summary>
        /// <typeparam name="TAsset">The type of asset</typeparam>
        /// <param name="name">The name of the Asset</param>
        /// <param name="bundle">The bundle to load from. Accepts the value <see cref="RRPBundle.All"/>, but it'll log a warning since using the value <see cref="RRPBundle.All"/> creates unecesary calls.</param>
        /// <returns>The loaded asset if it exists, null otherwise.</returns>
        public static TAsset LoadAsset<TAsset>(string name, RRPBundle bundle) where TAsset : UObject
        {
            TAsset asset = null;
            if (bundle == RRPBundle.All)
            {
                return FindAsset<TAsset>(name);
            }

            asset = _assetBundles[bundle].LoadAsset<TAsset>(name);

#if DEBUG
            if (!asset)
            {
                RRPLog.Warning($"The method \"{GetCallingMethod()}\" is calling \"LoadAsset<TAsset>(string, CommissionBundle)\" with the arguments \"{typeof(TAsset).Name}\", \"{name}\" and \"{bundle}\", however, the asset could not be found.\n" +
                    $"A complete search of all the bundles will be done and the correct bundle enum will be logged.");

                return LoadAsset<TAsset>(name, RRPBundle.All);
            }
#endif
            return asset;
        }

        /// <summary>
        /// Creates an instance of <see cref="RRPAssetRequest{TAsset}"/> which will contain the necesary metadata for loading an Asset asynchronously.
        /// <para>See also <see cref="LoadAsset{TAsset}(string, RRPBundle)"/></para>
        /// </summary>
        /// <typeparam name="TAsset">The type of asset to load</typeparam>
        /// <param name="name">The name of the asset to load</param>
        /// <param name="bundle">The bundle to search thru, accepts the <see cref="RRPBundle.All"/> value but it's not recommended as it creates unecesary calls.</param>
        /// <returns>The <see cref="RRPAssetRequest{TAsset}"/> to use for asynchronous loading.</returns>
        public static RRPAssetRequest<TAsset> LoadAssetAsync<TAsset>(string name, RRPBundle bundle) where TAsset : UObject
        {
            return new RRPAssetRequest<TAsset>(name, bundle);
        }

        /// <summary>
        /// Loads all assets of type <typeparamref name="TAsset"/> from the AssetBundle specified by <paramref name="bundle"/>
        /// <para>See also <see cref="LoadAllAssetsAsync{TAsset}(RRPBundle)"/></para>
        /// </summary>
        /// <typeparam name="TAsset">The type of asset to load</typeparam>
        /// <param name="bundle">The AssetBundle to load from, accepts the <see cref="RRPBundle.All"/> value</param>
        /// <returns>An array of <typeparamref name="TAsset"/> which contains all the loaded assets.</returns>
        public static TAsset[] LoadAllAssets<TAsset>(RRPBundle bundle) where TAsset : UObject
        {
            TAsset[] loadedAssets = null;
            if (bundle == RRPBundle.All)
            {
                return FindAssets<TAsset>();
            }
            loadedAssets = _assetBundles[bundle].LoadAllAssets<TAsset>();

#if DEBUG
            if (loadedAssets.Length == 0)
            {
                RRPLog.Warning($"Could not find any asset of type {typeof(TAsset).Name} inside the bundle {bundle}");
            }
#endif
            return loadedAssets;
        }

        /// <summary>
        /// Creates an instance of <see cref="RRPAssetRequest{TAsset}"/> which will contain the necesary metadata for loading an Asset asynchronously.
        /// <para>See also <see cref="LoadAllAssets{TAsset}(RRPBundle)"/></para>
        /// </summary>
        /// <typeparam name="TAsset">The type of asset to load</typeparam>
        /// <param name="bundle">The AssetBundle to load from, accepts the <see cref="RRPBundle.All"/> value</param>
        /// <returns>The <see cref="RRPAssetRequest{TAsset}"/> to use for asynchronous loading.</returns>
        public static RRPAssetRequest<TAsset> LoadAllAssetsAsync<TAsset>(RRPBundle bundle) where TAsset : UObject
        {
            return new RRPAssetRequest<TAsset>(bundle);
        }

        /// <summary>
        /// Initializes the mod's asset bundles asynchronously, should only be called once and during <see cref="RRPContent.LoadStaticContentAsync(RoR2.ContentManagement.LoadStaticContentAsyncArgs)"/>
        /// </summary>
        /// <returns>A coroutine which can be awaited.</returns>
        internal static IEnumerator Initialize()
        {
            RRPLog.Info($"Initializing Assets...");
            //We need to load the asset bundles first otherwise the rest of the mod wont work.
            var loadRoutine = LoadAssetBundles();

            while (!loadRoutine.IsDone())
            {
                yield return null;
            }

            //We can swap shaders in parallel
            ParallelMultiStartCoroutine multiStartCoroutine = new ParallelMultiStartCoroutine();
            multiStartCoroutine.Add(SwapShaders);
            multiStartCoroutine.Add(SwapAddressableShaders);

            while (!multiStartCoroutine.IsDone()) yield return null;

            //Asset bundles have been loaded and shaders have been swapped, invoke method.
            _onRRPAssetsInitialized?.Invoke();
            yield break;
        }

        //This is a method which is used to load the AssetBundles from the mod asynchronously, it is very complicated but this method should not be touched as if you properly add the new Enum and const string values, managing the new bundles will be easy.
        //look at the method "LoadFromPath", that one contains stuff you should be interested in modifying in the future.
        private static IEnumerator LoadAssetBundles()
        {
            ParallelMultiStartCoroutine helper = new ParallelMultiStartCoroutine();

            List<(string path, RRPBundle bundleEnum, AssetBundle loadedBundle)> pathsAndBundles = new List<(string path, RRPBundle bundleEnum, AssetBundle loadedBundle)>();

            string[] paths = GetAssetBundlePaths();
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                helper.Add(LoadFromPath, pathsAndBundles, path, i, paths.Length);
            }

            helper.Start();
            while (!helper.IsDone())
                yield return null;

            foreach ((string path, RRPBundle bundleEnum, AssetBundle assetBundle) in pathsAndBundles)
            {
                if (bundleEnum == RRPBundle.StreamedScene)
                {
                    HG.ArrayUtils.ArrayAppend(ref _streamedSceneBundles, assetBundle);
                }
                else
                {
                    _assetBundles[bundleEnum] = assetBundle;
                }
            }
        }

        //This method is what actually loads the AssetBundle into memory, and assigns it a Bundle enum value if needed.
        private static IEnumerator LoadFromPath(List<(string path, RRPBundle bundleEnum, AssetBundle loadedBundle)> list, string path, int index, int totalPaths)
        {
            string fileName = Path.GetFileName(path);
            RRPBundle? rrpbundle = null;
            //When you add new AssetBundles, you should add new Cases to this switch clause for your new bundles, for RRP, if you where to add an "Artifacts" bundle, you'd write the following line (which is commented in this scenario.) this is all you need to do to get new asset bundles loading.
            switch (fileName)
            {
                case MAIN: rrpbundle = RRPBundle.Main; break;
                case ITEMS: rrpbundle = RRPBundle.Items; break;
                case BASE: rrpbundle = RRPBundle.Base; break;
                case ARTIFACTS: rrpbundle = RRPBundle.Artifacts; break;
                case INTERACTS: rrpbundle = RRPBundle.Interactables; break;
                //case MONSTERS: rrpbundle = RRPBundle.Monsters; break;
                case INDEV: rrpbundle = RRPBundle.Indev; break;
                case STAGES: rrpbundle = RRPBundle.StreamedScene; break;
                //case ARTIFACTS: RRPBundle = RRPBundle.Artifacts; break;

                //This path does not match any of the non scene bundles, could be a scene, we will mark these on only this ocassion as "StreamedScene".
                default: rrpbundle = RRPBundle.StreamedScene; break;
            }

            var request = AssetBundle.LoadFromFileAsync(path);
            while (!request.isDone)
            {
                yield return null;
            }

            AssetBundle bundle = request.assetBundle;

            //Throw if no bundle was loaded
            if (!bundle)
            {
                throw new FileLoadException($"AssetBundle.LoadFromFile did not return an asset bundle. (Path={path})");
            }

            //The switch statement considered this a streamed scene bundle
            if (rrpbundle == RRPBundle.StreamedScene)
            {
                //supposed bundle is not streamed scene? throw exception.
                if (!bundle.isStreamedSceneAssetBundle)
                {
                    throw new Exception($"AssetBundle in specified path is not a streamed scene bundle, but its file name was not found in the Switch statement. have you forgotten to setup the enum and file name in your assets class? (Path={path})");
                }
                else
                {
                    //bundle is streamed scene, add to the list and break.
                    list.Add((path, RRPBundle.StreamedScene, bundle));
                    yield break;
                }
            }

            //The switch statement considered this to not be a streamed scene bundle, but an assets bundle.
            list.Add((path, rrpbundle.Value, bundle));
            yield break;
        }

        private static string[] GetAssetBundlePaths()
        {
            return Directory.GetFiles(AssetBundleFolderPath).Where(filePath => !filePath.EndsWith(".manifest")).ToArray();
        }

        //Utilize the built in "ShaderUtil" class from MSU to swap both kinds of shaders.
        private static IEnumerator SwapShaders()
        {
            return ShaderUtil.SwapStubbedShadersAsync(_assetBundles.Values.ToArray());
        }

        private static IEnumerator SwapAddressableShaders()
        {
            return ShaderUtil.LoadAddressableMaterialShadersAsync(_assetBundles.Values.ToArray());
        }

        //This method tries to find an asset of type TAsset and of a specific name in all the bundles, it returns the first match.
        //There's usually no need to run this method in Release builds, and it mostly exists for Development purposes.
        private static TAsset FindAsset<TAsset>(string name) where TAsset : UnityEngine.Object
        {
            TAsset loadedAsset = null;
            RRPBundle foundInBundle = RRPBundle.Invalid;
            foreach ((var enumVal, var assetBundle) in _assetBundles)
            {
                loadedAsset = assetBundle.LoadAsset<TAsset>(name);

                if (loadedAsset)
                {
                    foundInBundle = enumVal;
                    break;
                }
            }

#if DEBUG
            if (loadedAsset)
                RRPLog.Info($"Asset of type {typeof(TAsset).Name} with name {name} was found inside bundle {foundInBundle}, it is recommended that you load the asset directly.");
            else
                RRPLog.Warning($"Could not find asset of type {typeof(TAsset).Name} with name {name} in any of the bundles.");
#endif

            return loadedAsset;
        }

        //This method tries to find all assets of type TAsset in all the bundles, it returns a collection of assets.
        private static TAsset[] FindAssets<TAsset>() where TAsset : UnityEngine.Object
        {
            List<TAsset> assets = new List<TAsset>();
            foreach ((_, var bundles) in _assetBundles)
            {
                assets.AddRange(bundles.LoadAllAssets<TAsset>());
            }

#if DEBUG
            if (assets.Count == 0)
                RRPLog.Warning($"Could not find any asset of type {typeof(TAsset).Name} in any of the bundles");
#endif

            return assets.ToArray();
        }

#if DEBUG
        private static string GetCallingMethod()
        {
            var stackTrace = new StackTrace();

            for (int stackFrameIndex = 0; stackFrameIndex < stackTrace.FrameCount; stackFrameIndex++)
            {
                var frame = stackTrace.GetFrame(stackFrameIndex);
                var method = frame.GetMethod();
                if (method == null)
                    continue;

                var declaringType = method.DeclaringType;
                if (declaringType.IsGenericType && declaringType.DeclaringType == typeof(RRPAssets))
                    continue;

                if (declaringType == typeof(RRPAssets))
                    continue;

                var fileName = frame.GetFileName();
                var fileLineNumber = frame.GetFileLineNumber();
                var fileColumnNumber = frame.GetFileColumnNumber();

                return $"{declaringType.FullName}.{method.Name}({GetMethodParams(method)}) (fileName={fileName}, Location=L{fileLineNumber} C{fileColumnNumber})";
            }
            return "[COULD NOT GET CALLING METHOD]";
        }

        private static string GetMethodParams(MethodBase methodBase)
        {
            var parameters = methodBase.GetParameters();
            if (parameters.Length == 0)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var parameter in parameters)
            {
                stringBuilder.Append(parameter.ToString() + ", ");
            }
            return stringBuilder.ToString();
        }
#endif
    }

    /// <summary>
    /// A class that represents a request for loading Assets asynchronously.
    /// <br>You're strongly advised to use and check out <see cref="RRPAssetRequest{TAsset}"/> instead.</br>
    /// </summary>
    public abstract class RRPAssetRequest
    {
        /// <summary>
        /// The loaded asset, boxed as a Unity Object.
        /// </summary>
        public abstract UObject boxedAsset { get; }
        /// <summary>
        /// The loaded assets, boxed as an Enumerable of Unity Object
        /// </summary>
        public abstract IEnumerable<UObject> boxedAssets { get; }

        /// <summary>
        /// The AssetBundle to load from.
        /// </summary>
        public RRPBundle targetBundle => _targetBundle;
        private RRPBundle _targetBundle;

        /// <summary>
        /// The name of the asset to load. Can be null in the scenario this request loads multiple assets.
        /// </summary>
        public NullableRef<string> assetName => _assetName;
        private NullableRef<string> _assetName;

        /// <summary>
        /// Wether this request is loading a single asset, or multiple assets.
        /// </summary>
        public bool singleAssetLoad { get; private set; }
        /// <summary>
        /// Checks if the asynchronous loading operation has completed.
        /// </summary>
        public bool isComplete
        {
            get
            {
                if (internalCoroutine == null)
                    StartLoad();

                return !internalCoroutine.MoveNext();
            }
        }

        /// <summary>
        /// The coroutine that's loading the assets
        /// </summary>
        protected IEnumerator internalCoroutine;

        /// <summary>
        /// The AssetType's Name
        /// </summary>
        protected string assetTypeName;

        /// <summary>
        /// Starts the loading coroutine from this AssetRequest.
        /// </summary>
        public void StartLoad()
        {
            if (singleAssetLoad)
            {
                internalCoroutine = LoadSingleAsset();
            }
            else
            {
                internalCoroutine = LoadMultipleAsset();
            }
        }

        /// <summary>
        /// Implement the method that loads a Single asset asynchronously.
        /// </summary>
        /// <returns>A coroutine</returns>
        protected abstract IEnumerator LoadSingleAsset();

        /// <summary>
        /// Implement the method that loads multiple assets asynchronously.
        /// </summary>
        /// <returns>A coroutine</returns>
        protected abstract IEnumerator LoadMultipleAsset();

        /// <summary>
        /// Constructor for an RRPAssetRequest that'll load a single asset
        /// </summary>
        /// <param name="assetName">The name of the asset</param>
        /// <param name="bundleEnum">The AssetBundle to load from, accepts the value <see cref="RRPBundle.All"/>, but it shouldn't be used as it generates unecesary overhead</param>
        public RRPAssetRequest(string assetName, RRPBundle bundleEnum)
        {
            _assetName = assetName;
            _targetBundle = bundleEnum;
            singleAssetLoad = true;
            assetTypeName = "UnityEngine.Object";
        }

        /// <summary>
        /// Constructor for an RRPAssetRequest that'll load multiple assets
        /// </summary>
        /// <param name="bundleEnum">The AssetBundle to load from, accepts the value <see cref="RRPBundle.All"/></param>
        public RRPAssetRequest(RRPBundle bundleEnum)
        {
            _assetName = string.Empty;
            _targetBundle = bundleEnum;
            singleAssetLoad = false;
            assetTypeName = "UnityEngine.Object";
        }
    }

    public class RRPAssetRequest<TAsset> : RRPAssetRequest where TAsset : UObject
    {
        public override UObject boxedAsset => _asset;
        public TAsset asset => _asset;
        private TAsset _asset;

        public override IEnumerable<UObject> boxedAssets => _assets;
        public IEnumerable<TAsset> assets => _assets;
        private List<TAsset> _assets;

        protected override IEnumerator LoadSingleAsset()
        {
            AssetBundleRequest request = null;

            request = RRPAssets.GetAssetBundle(targetBundle).LoadAssetAsync<TAsset>(assetName); ;
            while (!request.isDone)
                yield return null;

            _asset = (TAsset)request.asset;

#if DEBUG
            //Asset found, dont try to find it.
            if (_asset)
                yield break;

            RRPLog.Warning($"The method \"{GetCallingMethod()}\" is calling a RRPAssetRequest.StartLoad() while the class has the values \"{assetTypeName}\", \"{assetName}\" and \"{targetBundle}\", however, the asset could not be found.\n" +
    $"A complete search of all the bundles will be done and the correct bundle enum will be logged.");

            RRPBundle foundInBundle = RRPBundle.Invalid;
            foreach (RRPBundle bundleEnum in Enum.GetValues(typeof(RRPBundle)))
            {
                if (bundleEnum == RRPBundle.All || bundleEnum == RRPBundle.Invalid || bundleEnum == RRPBundle.StreamedScene)
                    continue;

                request = RRPAssets.GetAssetBundle(bundleEnum).LoadAssetAsync<TAsset>(assetName);
                while (!request.isDone)
                {
                    yield return null;
                }

                if (request.asset)
                {
                    _asset = (TAsset)request.asset;
                    foundInBundle = bundleEnum;
                    break;
                }
            }

            if (_asset)
            {
                RRPLog.Info($"Asset of type {assetTypeName} and name {assetName} was found inside bundle {foundInBundle}. It is recommended to load the asset directly.");
            }
            else
            {
                RRPLog.Fatal($"Could not find asset of type {assetTypeName} and name {assetName} In any of the bundles, exceptions may occur.");
            }
#endif
            yield break;
        }

        protected override IEnumerator LoadMultipleAsset()
        {
            _assets.Clear();

            AssetBundleRequest request = null;
            if (targetBundle == RRPBundle.All)
            {
                foreach (RRPBundle enumVal in Enum.GetValues(typeof(RRPBundle)))
                {
                    if (enumVal == RRPBundle.All || enumVal == RRPBundle.Invalid || enumVal == RRPBundle.StreamedScene)
                        continue;
                    RRPLog.Message(targetBundle);
                    request = RRPAssets.GetAssetBundle(enumVal).LoadAllAssetsAsync<TAsset>();
                    while (!request.isDone)
                        yield return null;

                    _assets.AddRange(request.allAssets.OfType<TAsset>());
                }

#if DEBUG
                if (_assets.Count == 0)
                {
                    RRPLog.Warning($"Could not find any asset of type {assetTypeName} in any of the bundles");
                }
#endif
                yield break;
            }

            request = RRPAssets.GetAssetBundle(targetBundle).LoadAllAssetsAsync<TAsset>();
            while (!request.isDone) yield return null;

            _assets.AddRange(request.allAssets.OfType<TAsset>());

#if DEBUG
            if (_assets.Count == 0)
            {
                RRPLog.Warning($"Could not find any asset of type {assetTypeName} inside the bundle {targetBundle}");
            }
#endif

            yield break;
        }

#if DEBUG
        private static string GetCallingMethod()
        {
            var stackTrace = new StackTrace();

            for (int stackFrameIndex = 0; stackFrameIndex < stackTrace.FrameCount; stackFrameIndex++)
            {
                var frame = stackTrace.GetFrame(stackFrameIndex);
                var method = frame.GetMethod();
                if (method == null)
                    continue;

                var declaringType = method.DeclaringType;
                if (declaringType.IsGenericType && declaringType.DeclaringType == typeof(RRPAssets))
                    continue;

                if (declaringType == typeof(RRPAssets))
                    continue;

                var fileName = frame.GetFileName();
                var fileLineNumber = frame.GetFileLineNumber();
                var fileColumnNumber = frame.GetFileColumnNumber();

                return $"{declaringType.FullName}.{method.Name}({GetMethodParams(method)}) (fileName={fileName}, Location=L{fileLineNumber} C{fileColumnNumber})";
            }
            return "[COULD NOT GET CALLING METHOD]";
        }

        private static string GetMethodParams(MethodBase methodBase)
        {
            var parameters = methodBase.GetParameters();
            if (parameters.Length == 0)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var parameter in parameters)
            {
                stringBuilder.Append(parameter.ToString() + ", ");
            }
            return stringBuilder.ToString();
        }
#endif

        internal RRPAssetRequest(string name, RRPBundle bundle) : base(name, bundle)
        {
            assetTypeName = typeof(TAsset).Name;
        }

        internal RRPAssetRequest(RRPBundle bundle) : base(bundle)
        {
            _assets = new List<TAsset>();
            assetTypeName = typeof(TAsset).Name;
        }
    }
}