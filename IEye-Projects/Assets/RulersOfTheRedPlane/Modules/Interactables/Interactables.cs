using Moonstorm.Config;
using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using RiskOfOptions.OptionConfigs;
using Moonstorm;

namespace IEye.RRP.Modules
{
    public sealed class Interactables : InteractableModuleBase
    {
        public static Interactables Instance { get; private set; }

        public override R2APISerializableContentPack SerializableContentPack { get; } = RRPContent.Instance.SerializableContentPack;

        public static ConfigurableBool EnableInteractables = RRPConfig.MakeConfigurableBool(true, (b) =>
        {
            b.Section = "Enable All Interactables";
            b.Key = "Enable All Interactables";
            b.Description = "Enable Interactables";
            b.ConfigFile = RRPConfig.ConfigMain;
            b.CheckBoxConfig = new CheckBoxConfig
            {
                restartRequired = true,
            };
        }).DoConfigure();

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            if (!EnableInteractables) return;
            RRPMain.logger.LogInfo($"Initializing Interactables.");
            GetInteractableBases();
        }

        protected override IEnumerable<InteractableBase> GetInteractableBases()
        {
            base.GetInteractableBases()
                .Where(interactable =>
                {
                    return RRPConfig.MakeConfigurableBool(true, (b) =>
                    {
                        b.Section = "Interactables";
                        b.Key = interactable.Interactable.ToString();
                        b.Description = "Enable/Disable this Interactable";
                        b.ConfigFile = RRPConfig.ConfigMain;
                        b.CheckBoxConfig = new CheckBoxConfig
                        {
                            checkIfDisabled = () => !EnableInteractables,
                            restartRequired = true
                        };
                    }).DoConfigure();
                })
                .ToList()
                .ForEach(interactable => AddInteractable(interactable));
            return null;
        }
    }
}