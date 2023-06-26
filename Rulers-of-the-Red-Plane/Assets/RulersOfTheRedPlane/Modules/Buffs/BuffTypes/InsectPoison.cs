using Moonstorm.Components;
using IEye.RRP.Items;
using R2API;
using RoR2;
using Moonstorm;

namespace IEye.RRP.Buffs
{
    public class InsectPoison : BuffBase
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("InsectPoison", RRPBundle.Items);
        

        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.InsectPoison;
            
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.damageMultAdd -= IntrospectiveInsect.insectDamageCripple / 100;
            }
        }
    }
    
}