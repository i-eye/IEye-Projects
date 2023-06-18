using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using IEye.RulersOfTheRedPlane.Buffs;
using R2API;
using Mono.Cecil;
using System.Linq;

namespace IEye.RulersOfTheRedPlane.Items
{
    //[DisabledContent]
    public class InnerPiece : ItemBase
    {

        public const string token = "RRP_ITEM_INNERPIECE_DESC";
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("InnerPiece", RRPBundle.Items);

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Duration of the insect blood debuff(default 3s)")]
        public static float speed = 50;

        public sealed class Behavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.InnerPiece;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                int numDebuff = 0;
                BuffIndex[] debuffBuffIndices = BuffCatalog.debuffBuffIndices;
                foreach (BuffIndex buffType in debuffBuffIndices)
                {
                    if (body.HasBuff(buffType))
                    {
                        numDebuff++;
                    }
                }
                DotController dotController = DotController.FindDotController(body.gameObject);
                for (DotController.DotIndex dotIndex = DotController.DotIndex.Bleed; dotIndex < DotController.DotIndex.Count; dotIndex++)
                {
                    if (dotController.HasDotActive(dotIndex))
                    {
                        numDebuff++;
                    }
                }
                if(numDebuff > 0 && !body.healthComponent.isHealthLow)
                {
                    args.moveSpeedMultAdd += speed / 100;
                }
                
            }
        }
    }
}