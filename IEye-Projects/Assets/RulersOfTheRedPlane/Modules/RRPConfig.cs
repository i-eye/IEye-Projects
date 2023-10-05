using BepInEx;
using BepInEx.Configuration;
using Moonstorm.Config;
using Moonstorm.Loaders;
using System.Collections.Generic;
using UnityEngine;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions;
using System.Data;
//using UnityEngine.XR.WSA.Input;

namespace IEye.RRP
{
    public class RRPConfig : ConfigLoader<RRPConfig>
    {
        public override BaseUnityPlugin MainClass => RRPMain.Instance;

        public const string PREFIX = "RRP.";
        internal const string IDMain = PREFIX + "Main";
        internal const string IDItem = PREFIX + "Items";
        internal const string IDInteractable = PREFIX + "Interactables";
        internal const string IDItemTier = PREFIX + "Item Tier";
        //internal const string IDArtifact = PREFIX + "Artifacts";
        //internal const string IDSurvivor = PREFIX + "Survivors";
        //internal const string IDMisc = PREFIX + "Miscellaneous";

        public override bool CreateSubFolder => true;

        public static ConfigFile ConfigMain;
        public static ConfigFile ConfigItem;
        public static ConfigFile ConfigInteractable;
        public static ConfigFile ConfigItemTier;
        public static ConfigFile ConfigArtifact;
        public static ConfigFile ConfigSurvivor;
        public static ConfigFile ConfigMisc;

        internal static ConfigurableBool UnlockAll = new ConfigurableBool(false)
        {
            Section = "General",
            Key = "Unlock All",
            Description = "Setting this to true unlocks all the content in Rulers of the Red Plane(THERE ARE NO UNLOCKS YET LMAOOO)",
            ModGUID = RRPMain.GUID,
            ModName = RRPMain.MODNAME,
            CheckBoxConfig = new CheckBoxConfig
            {
                restartRequired = true,
            }
        };

        internal static ConfigEntry<KeyCode> RestKeybind;
        internal static KeyCode restKeybind;
        internal static ConfigEntry<KeyCode> TauntKeybind;
        internal static KeyCode tauntKeybind;
        internal static List<ConfigEntry<bool>> ItemToggles;

        public void Init()
        {
            Sprite icon = RRPAssets.LoadAsset<Sprite>("icon", RRPBundle.Main);
            ModSettingsManager.SetModIcon(icon, RRPMain.GUID, RRPMain.MODNAME);
            ModSettingsManager.SetModDescription("A general content mod that is way too overambitioujs", RRPMain.GUID, RRPMain.MODNAME);

            ConfigMain = CreateConfigFile(IDMain, true);
            ConfigItem = CreateConfigFile(IDItem, true);
            ConfigInteractable = CreateConfigFile(IDInteractable, true);
            ConfigItemTier = CreateConfigFile(IDItemTier, true);
            //ConfigSurvivor = CreateConfigFile(IDSurvivor);
            //ConfigArtifact = CreateConfigFile(IDArtifact);
            //ConfigMisc = CreateConfigFile(IDMisc);

            SetConfigs();
        }

        private static void SetConfigs()
        {
            UnlockAll.SetConfigFile(ConfigMain).DoConfigure();
            //emotes
            /*RestKeybind = Starstorm.instance.Config.Bind("Starstorm 2 :: Keybinds", "Rest Emote", KeyCode.Alpha1, "Keybind used for the Rest emote.");
            restKeybind = RestKeybind.Value;// cache it for performance

            TauntKeybind = Starstorm.instance.Config.Bind("Starstorm 2 :: Keybinds", "Taunt Emote", KeyCode.Alpha2, "Keybind used for the Taunt emote.");
            tauntKeybind = TauntKeybind.Value;// cache it for performance*/
        }
    }
}
