using Moonstorm.Components;
using IEye.RulersOfTheRedPlane.Items;
using R2API;
using RoR2;
using Moonstorm;

namespace IEye.RulersOfTheRedPlane.Buffs
{
    public class InsectBloody : BuffBase
    {
        public override BuffDef BuffDef { get; } = RRPAssets.LoadAsset<BuffDef>("InsectBloody", RRPBundle.Items);


        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier, IOnDamageInflictedServerReceiver
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.InsectPoison;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.damageMultAdd -= AgressiveInsect.bloodyInsectDamageCripple;
                args.armorAdd -= AgressiveInsect.bloodyInsectArmorCripple;
            }

            public void OnDamageInflictedServer(DamageReport damageReport)
            {
                float damageDealt = damageReport.damageDealt;
                float healAmount = damageDealt * .1f * damageReport.victimBody.inventory.GetItemCount(RRPContent.Items.AgressiveInsect);
                damageReport.attackerBody.healthComponent.Heal(healAmount, new ProcChainMask());
            }

        }
    }

}