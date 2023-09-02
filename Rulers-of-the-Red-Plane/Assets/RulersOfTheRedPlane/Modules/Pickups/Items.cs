using BepInEx;
using BepInEx.Configuration;
using Moonstorm;
using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace IEye.RRP.Modules
{
    public sealed class Items : ItemModuleBase
    {
        public static Items Instance { get; private set; }

        public BaseUnityPlugin MainClass => RRPMain.Instance;
        public override R2APISerializableContentPack SerializableContentPack => RRPContent.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            RRPMain.logger.LogInfo($"Initializing Items...");
            GetItemBases();
        }

        protected override IEnumerable<ItemBase> GetItemBases()
        {
            base.GetItemBases()
                .ToList()
                .ForEach(item => AddItem(item));

            base.GetItemBases().ToList().ForEach(item => CheckEnabledStatus(item));

            return null;
        }

        protected void CheckEnabledStatus(ItemBase item)
        {
            if (item.ItemDef.deprecatedTier != RoR2.ItemTier.NoTier)
            {
                string niceName = MSUtil.NicifyString(item.GetType().Name);
                ConfigEntry<bool> enabled = RRPMain.Instance.Config.Bind<bool>(niceName, "Enabled", true, "Should this item be enabled?");

                if (!enabled.Value)
                {
                    item.ItemDef.deprecatedTier = RoR2.ItemTier.NoTier;
                }
            }
        }

    }
}
