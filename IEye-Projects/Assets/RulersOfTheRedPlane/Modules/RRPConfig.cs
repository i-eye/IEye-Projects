using BepInEx;
using BepInEx.Configuration;
using MSU;
using System.Collections.Generic;
using UnityEngine;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions;
using System.Data;
using MSU.Config;
//using UnityEngine.XR.WSA.Input;

namespace IEye.RRP
{
    public class RRPConfig
    {

        public const string PREFIX = "RRP.";
        internal const string IDMain = PREFIX + "Main";
        internal const string IDItem = PREFIX + "Items";
        internal const string IDInteractable = PREFIX + "Interactables";
        internal const string IDItemTier = PREFIX + "Item Tier";
        //internal const string IDArtifact = PREFIX + "Artifacts";
        //internal const string IDSurvivor = PREFIX + "Survivors";
        //internal const string IDMisc = PREFIX + "Miscellaneous";

        internal static ConfigFactory configFactory { get; private set; }

  
        public static ConfigFile ConfigMain { get; private set; }
        public static ConfigFile ConfigItem { get; private set; }
        public static ConfigFile ConfigInteractable { get; private set; }
        public static ConfigFile ConfigItemTier { get; private set; }

        internal RRPConfig(BaseUnityPlugin bup)
        {
            configFactory = new ConfigFactory(bup, true);
            ConfigMain = configFactory.CreateConfigFile(IDMain, true);
            ConfigItem = configFactory.CreateConfigFile(IDItem, true);
            ConfigInteractable = configFactory.CreateConfigFile(IDInteractable, true);
            ConfigItemTier = configFactory.CreateConfigFile(IDItemTier, true);
        }


    }
}
