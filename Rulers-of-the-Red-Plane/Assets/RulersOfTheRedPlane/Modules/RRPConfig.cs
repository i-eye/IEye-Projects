using BepInEx;
using BepInEx.Configuration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm.Loaders;

namespace IEye.RulersOfTheRedPlane
{


    public class RRPConfig : ConfigLoader<RRPConfig>
    {
        public const string items = "RRP.Items";
        public const string equips = "RRP.Equips";

        public override BaseUnityPlugin MainClass => RulersOfTheRedPlaneMain.Instance;

        public override bool CreateSubFolder => true;

        public static ConfigFile itemConfig;
        public static ConfigFile equipsConfig;

        internal static ConfigEntry<bool> UnlockAll;
        internal static List<ConfigEntry<bool>> ItemToggles;

        public void Init()
        {
            itemConfig = CreateConfigFile(items);
            //equipsConfig = CreateConfigFile(equips);

            SetConfigs();
        }

        private static void SetConfigs()
        {
            UnlockAll =
                RulersOfTheRedPlaneMain.Instance.Config.Bind("Starstorm 2 :: Unlock All",
                            "false",
                            false,
                            "Setting this to true unlocks all the content in Starstorm 2, excluding skin unlocks.");
        }
    }
}