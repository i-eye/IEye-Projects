using Moonstorm.Components;
using IEye.RRP.Items;
using R2API;
using RoR2;
using Moonstorm;

namespace IEye.RRP.Buffs
{
    public class InsectBloody : BuffBase
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("InsectBloody", RRPBundle.Items);


        public sealed class Behavior : BaseBuffBodyBehavior
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.InsectBloody;

            

        }
    }

}