using RoR2.Items;
using Moonstorm;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R2API;

namespace IEye.RRP.Items
{
    [DisabledContent]
    public class LevelGiver : ItemBase
    {
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("LevelGiver", RRPBundle.Artifacts);

        public sealed class Behavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.LevelGiver;
            private void Start()
            {
                RRPMain.logger.LogMessage("Start");
                body.experience += 2500f;
            }
            
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                RRPMain.logger.LogMessage("ModifyStatsArguments");
            }
        }

    }
    
    
}
