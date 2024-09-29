using MSU;
using MSU.Config;
using RoR2;
using RoR2.Items;
using R2API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R2API.Networking;
using RoR2.ContentManagement;
using static MSU.BaseBuffBehaviour;

namespace IEye.RRP.Items
{
    public class AdrenalineFrenzy : RRPItem
    {
        public const string token = "RRP_ITEM_ADRFRENZY_DESC";
        //public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("AdrenalineFrenzy", RRPBundle.Items);

        public override RRPAssetRequest AssetRequest => RRPAssets.LoadAssetAsync<ItemAssetCollection>("acFrenzy", RRPBundle.Items);

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Base percentage speed added on kill(default 6%).")]
        [FormatToken(token, 0)]
        public static float killSpeed = 6f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Base percentage speed added on kill per stack(default 4%).")]
        [FormatToken(token, 1)]
        public static float killSpeedStack = 4f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Base time for on kill speed boost(default 10s).")]
        [FormatToken(token, 2)]
        public static float killSpeedDuration = 10f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Base percentage speed added on getting hit(default 12%).")]
        [FormatToken(token, 3)]
        public static float onHitSpeed = 12f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Base percentage speed added on getting hit per stack(default(7%).")]
        [FormatToken(token, 4)]
        public static float onHitSpeedStack = 7f;

        [RiskOfOptionsConfigureField(RRPConfig.IDItem, configDescOverride = "Base time for on getting hit speed boost(default 8s).")]
        [FormatToken(token, 5)]
        public static float onHitSpeedDuration = 8f;

        public sealed class Behavior : BaseItemBodyBehavior, IOnTakeDamageServerReceiver, IOnKilledOtherServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.AdrenalineFrenzy;


            private void Start()
            {
                if (body.inventory.GetItemCount(RRPContent.Items.SacrificialHelper) == 0)
                {
                    body.inventory.GiveItem(RRPContent.Items.SacrificialHelper);
                }
            }
            public void OnKilledOtherServer(DamageReport report)
            {
                
                if ((report.damageInfo.procCoefficient > 0) && (report.damageInfo.dotIndex.Equals(DotController.DotIndex.None)) && ((int)report.damageInfo.damageType.damageType) != 66)
                {
                    var cb = report.attacker.GetComponent<CharacterBody>();
                    cb.AddTimedBuffAuthority(RRPContent.Buffs.AdrenalineOnKill.buffIndex, killSpeedDuration);
                }
            }

            public void OnTakeDamageServer(DamageReport report)
            {
                if ((report.damageInfo.procCoefficient > 0) && (report.damageInfo.dotIndex.Equals(DotController.DotIndex.None)) && ((int)report.damageInfo.damageType.damageType) != 66){
                    var cb = report.victim.GetComponent<CharacterBody>();
                    cb.AddTimedBuffAuthority(RRPContent.Buffs.AdrenalineOnGettingHit.buffIndex, onHitSpeedDuration);
                }
            }
        }

        public sealed class OnKillBehavior : BaseBuffBehaviour, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.AdrenalineOnKill;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                if (hasAnyStacks)
                {
                    int stack = characterBody.inventory.GetItemCount(RRPContent.Items.AdrenalineFrenzy);
                    args.sprintSpeedAdd += (killSpeed / 100 + (killSpeedStack / 100 * (stack - 1))) * buffCount;
                }
                
            }
        }

        public sealed class OnHitBehavior : BaseBuffBehaviour, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => RRPContent.Buffs.AdrenalineOnGettingHit;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                if (hasAnyStacks)
                {
                    int stack = characterBody.inventory.GetItemCount(RRPContent.Items.AdrenalineFrenzy);
                    args.moveSpeedMultAdd += (onHitSpeed / 100 + (onHitSpeedStack / 100 * (stack - 1))) * buffCount;
                }
                
            }
        }

        public override void Initialize()
        {
            
        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }
    }
}
