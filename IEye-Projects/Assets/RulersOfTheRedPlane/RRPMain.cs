/*
Generated from a ROR2EK Template. Feel free to remove this comment section.
0 = modName; 1 = Nicified mod name; 2 = authorName; 3 = using clauses; 4 = attributes; 
*/

using BepInEx;
using R2API;
using R2API.ScriptableObjects;
using R2API.Utils;
using R2API.Networking;
using R2API.ContentManagement;
using UnityEngine;
using MSU;
using IEye.RRP.Items;
using R2API.AddressReferencedAssets;
namespace IEye.RRP
{
    #region R2API
    [BepInDependency("com.bepis.r2api.dot")]
    [BepInDependency("com.bepis.r2api.networking")]
    [BepInDependency("com.bepis.r2api.prefab")]
    [BepInDependency("com.bepis.r2api.difficulty")]
    [BepInDependency("com.rune580.riskofoptions")]
    #endregion
    [BepInDependency(MSU.MSUMain.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    //[R2APISubmoduleDependency(
    //    nameof(DotAPI),
    //    nameof(PrefabAPI),
    //    nameof(NetworkingAPI))]
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class RRPMain : BaseUnityPlugin
	{
        //hrrrng hamburger, colonel
		public const string GUID = "com.I_Eye.RulersOfTheRedPlane";
		public const string MODNAME = "Rulers of the Red Plane";
		public const string VERSION = "0.1.6";

        public static RRPMain instance;
        public static PluginInfo pluginInfo;
        

		private void Awake()
		{
			instance = this;
            pluginInfo = Info;
            new RRPLog(Logger);
            Logger.LogInfo("Logger created");

            new RRPConfig(this);
            Logger.LogInfo("About to do content");
            new RRPContent();
            Logger.LogInfo("RRP loading for MSU 2.0");

            LanguageFileLoader.AddLanguageFilesFromMod(this, "languages");
            
		}	
        private void Start()
        {
            SoundBankManager.Init();
        }

    }
}