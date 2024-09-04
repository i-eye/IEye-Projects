using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using MSU;
/*
namespace IEye.RRP.Modules
{
    public sealed class Buffs : BuffModuleBase
    {
        public static Buffs Instance { get; set; }
        public override R2APISerializableContentPack SerializableContentPack { get; } = RRPContent.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            RRPLog.Info($"Initializing Buffs.");
            GetBuffBases();
        }
        protected override IEnumerable<BuffBase> GetBuffBases()
        {
            base.GetBuffBases()
                .ToList()
                .ForEach(buff => AddBuff(buff));
            return null;
        }
    }
}
*/