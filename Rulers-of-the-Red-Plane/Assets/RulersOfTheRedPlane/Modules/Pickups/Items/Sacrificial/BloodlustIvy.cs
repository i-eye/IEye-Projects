using RoR2;
using RoR2.Items;
using Moonstorm;
using R2API;

namespace IEye.RRP.Items
{
    public class BloodlustIvy : ItemBase
    {
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("BloodlustIvy", RRPBundle.Items);

        public sealed class Behavior : BaseItemBodyBehavior
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.BloodyIvy;
        }
    }
}

