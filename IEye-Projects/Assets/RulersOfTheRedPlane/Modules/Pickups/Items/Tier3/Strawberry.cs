   using IL.RoR2.DispatachableEffects;
using MSU;
using RoR2;
using RoR2.ContentManagement;
using RoR2.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IEye.RRP.Items
{
    public class Strawberry : RRPItem
    {
        CharacterSpawnCard gupSpawnCard;
        public static CharacterSpawnCard friendlyGupSpawnCard = new CharacterSpawnCard();
        //public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("Strawberry", RRPBundle.Items);

        public override RRPAssetRequest AssetRequest => RRPAssets.LoadAssetAsync<ItemAssetCollection>("acStrawberry", RRPBundle.Items);

        public override void Initialize()
        {
            //ItemDef.requiredExpansion = 
            gupSpawnCard = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC1/Gup/cscGupBody.asset").WaitForCompletion();
            friendlyGupSpawnCard.directorCreditCost = 0;
            friendlyGupSpawnCard.prefab = gupSpawnCard.prefab;
            friendlyGupSpawnCard.sendOverNetwork = gupSpawnCard.sendOverNetwork;
            friendlyGupSpawnCard.hullSize = gupSpawnCard.hullSize;
            friendlyGupSpawnCard.nodeGraphType = gupSpawnCard.nodeGraphType;
            friendlyGupSpawnCard.requiredFlags = gupSpawnCard.requiredFlags;
            friendlyGupSpawnCard.forbiddenFlags = gupSpawnCard.forbiddenFlags;
            friendlyGupSpawnCard.eliteRules = gupSpawnCard.eliteRules;
            

        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }

        public sealed class Behavior : BaseItemBodyBehavior
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.Strawberry;

            float timeBetween = 60f;
            float cooldown;
            private void FixedUpdate()
            {
                CharacterMaster bodyMaster = body.master;
                if (!bodyMaster)
                {
                    return;
                }
                cooldown -= Time.fixedDeltaTime;
                if (cooldown <= 0f)
                {
                    DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(friendlyGupSpawnCard, new DirectorPlacementRule
                    {
                        placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                        minDistance = 3f,
                        maxDistance = 40f,
                        spawnOnTarget = base.transform
                    }, RoR2Application.rng);
                    directorSpawnRequest.summonerBodyObject = base.gameObject;
                    directorSpawnRequest.onSpawnedServer = OnSpawned;
                    DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
                    cooldown = timeBetween;
                }

                void OnSpawned(SpawnCard.SpawnResult spawnResult)
                {
                    GameObject spawnedInstance = spawnResult.spawnedInstance;
                    if ((bool)spawnedInstance)
                    {
                        CharacterMaster component = spawnedInstance.GetComponent<CharacterMaster>();
                        if ((bool)component)
                        {
                            Inventory inventory = component.inventory;
                            if (inventory)
                            {

                            }
                            component.teamIndex = TeamIndex.Player;
                            //component.inventory.GiveItem(RoR2Content.Items.BoostDamage, 30);
                            //component.inventory.GiveItem(RoR2Content.Items.BoostHp, 10);
                            component.inventory.GiveItem(RoR2Content.Items.HealthDecay, 17);
                            component.inventory.GiveItem(RRPContent.Items.Kamikaze, 1);
                            RRPLog.Debug("Index is" + RRPContent.Items.Kamikaze.itemIndex);
                            Deployable component2 = component.GetComponent<Deployable>();
                            if ((bool)component2)
                            {
                                bodyMaster.AddDeployable(component2, (DeployableSlot)3454);
                            }
                        }
                    }
                }
            }

        }
    }
}
