using Moonstorm;
using R2API.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEye.RRP.Modules
{
    public sealed class ItemTiers : ItemTierModuleBase
    {
        public static ItemTiers Instance { get; private set; }
        public override R2APISerializableContentPack SerializableContentPack => RRPContent.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            RRPMain.logger.LogInfo("Initializing Item Tiers...");
            GetItemTierBases();
        }

        protected override IEnumerable<ItemTierBase> GetItemTierBases()
        {
            base.GetItemTierBases()
                .ToList()
                .ForEach(itemTier => AddItemTier(itemTier));
            return null;
        }
    }
}