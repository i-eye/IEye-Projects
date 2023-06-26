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


        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier, IOnDamageInflictedServerReceiver
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.InsectBloody;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.damageMultAdd -= AgressiveInsect.bloodyInsectDamageCripple / 100;
                args.armorAdd -= AgressiveInsect.bloodyInsectArmorCripple;
            }

            public void OnDamageInflictedServer(DamageReport damageReport)
            {
                float damageDealt = damageReport.damageDealt;
                float healAmount = damageDealt * .1f;
                damageReport.attackerBody.healthComponent.Heal(healAmount, new ProcChainMask());
            }

        }
    }

}