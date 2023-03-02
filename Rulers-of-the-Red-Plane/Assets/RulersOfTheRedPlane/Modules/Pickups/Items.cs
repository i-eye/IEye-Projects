using BepInEx;
using BepInEx.Configuration;
using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;

namespace IEye.RulersOfTheRedPlane.Modules
{
    public sealed class Items : ItemModuleBase
    {
        public static Items Instance { get; private set; }

        public BaseUnityPlugin MainClass => RulersOfTheRedPlaneMain.Instance;
        public override R2APISerializableContentPack SerializableContentPack => RRPContent.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            DefNotSS2Log.Info($"Initializing Items...");
            GetItemBases();
        }

        protected override IEnumerable<ItemBase> GetItemBases()
        {
            base.GetItemBases()
                .ToList()
                .ForEach(item => AddItem(item));

            base.GetItemBases().ToList(); //.ForEach(item => CheckEnabledStatus(item))

            return null;
        }
        /*
        void CheckEnabledStatus(ItemBase item)
        {
            if (item.ItemDef._itemTierDef.tier != RoR2.ItemTier.NoTier)
            {
                string niceName = MSUtil.NicifyString(item.GetType().Name);
                ConfigEntry<bool> enabled = RulersOfTheRedPlaneMain.Instance.Config.Bind<bool>(niceName, "Enabled", true, "Should this item be enabled?");

                if (!enabled.Value)
                {
                    item.ItemDef._itemTierDef.tier = RoR2.ItemTier.NoTier;
                }
            }
        }
        */

    }
}
