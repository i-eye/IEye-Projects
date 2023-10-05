using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using IEye.RRP.Buffs;
using R2API;
using Mono.Cecil;
using System.Linq;

namespace IEye.RRP.Items
{
    [DisabledContent]
    
    public class InnerPiece : ItemBase
    {
        
        public const string token = "RRP_ITEM_INNERPIECE_DESC";
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("InnerPiece", RRPBundle.Items);

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Added speed(default 75%).")]
        public static float speed = 75f;

        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Added sprint speed(default 30%).")]
        public static float SprintSpeed = 30f;

        public sealed class Behavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            //[ItemDefAssociation]
            //private static ItemDef GetItemDef() => RRPContent.Items.InnerPiece;

            private bool isGood = true;
            int numDebuff;
            public void Update()
            {
                numDebuff = 0;
                BuffIndex[] debuffBuffIndices = BuffCatalog.debuffBuffIndices;
                foreach (BuffIndex buffType in debuffBuffIndices)
                {
                    if (body.HasBuff(buffType))
                    {
                        numDebuff++;
                    }
                }
                DotController dotController = DotController.FindDotController(body.gameObject);
                if (dotController)
                {
                    for (DotController.DotIndex dotIndex = DotController.DotIndex.Bleed; dotIndex < DotController.DotIndex.Count; dotIndex++)
                    {
                        if (dotController.HasDotActive(dotIndex))
                        {
                            numDebuff++;
                        }
                    }
                }
                //DefNotSS2Log.Message(numDebuff);
                if(body.healthComponent.isHealthLow || numDebuff > 1)
                {
                    isGood = false;
                } else
                {
                    isGood = true;
                }
                
                
            }

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                //DefNotSS2Log.Message("isgood = " + isGood);
                if (isGood)
                {
                    args.moveSpeedMultAdd += speed * stack / 100;
                    args.sprintSpeedAdd += SprintSpeed * stack / 100;
                }

            }
        }
    }
}