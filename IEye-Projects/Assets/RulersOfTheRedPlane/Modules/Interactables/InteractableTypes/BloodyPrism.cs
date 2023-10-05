using RoR2;
using Moonstorm;
using Moonstorm.AddressableAssets;
using R2API;
using IEye.RRP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RewiredConsts;
using System;
using HG;
using System.Linq;
using Mono.Cecil;
using RiskOfOptions;
using BepInEx.Configuration;
using Moonstorm.Config;
using RoR2.Hologram;
using EntityStates.Headstompers;
using EntityStates.BeetleQueenMonster;
using UnityEngine.AddressableAssets;

namespace IEye.RRP.Interactables
{
    //[DisabledContent]
    
    public class BloodyPrism : InteractableBase
    {
        public override GameObject Interactable { get; } = RRPAssets.LoadAsset<GameObject>("BloodyPrismGameObject", RRPBundle.Interactables);

        public override List<MSInteractableDirectorCard> InteractableDirectorCards { get; } = new List<MSInteractableDirectorCard>();

        /*
        [RooConfigurableField(RRPConfig.IDInteractable, ConfigDesc = "Credits for the Imp Stuff interactable catagory(default 3.2)")]
        public static ConfigurableFloat impStuffCredits = new ConfigurableFloat(3.2f);
        */


        [RooConfigurableField(RRPConfig.IDInteractable, ConfigDesc = "Credits multiplied by difficulty for prism combat director on use(default 100)")]
        public static int creditsCoef = 100;

        //[RooConfigurableField(RRPConfig.IDInteractable, ConfigDesc = "Chance for for spawning(1.9 is default, 3 is void catagory(Probably requires restart)(may not work)")]
        //public static float catagoryWeight = 1.9f;

        /*
        [RooConfigurableField(RRPConfig.IDInteractable, ConfigDesc = "Weight of bloody prism(default 15).")]
        public int weight = 15;
        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Director cost(default 30).")]
        public int directorCost = 30;
        [RooConfigurableField(RRPConfig.IDItem, ConfigDesc = "Stages needed to complete before encountering(default 2).")]
        public int minimumStages = 2;
        */
        public override void Initialize()
        {
            InteractableDirectorCards.Add(RRPAssets.LoadAsset<MSInteractableDirectorCard>("BloodyPrism", RRPBundle.Interactables));
            //impStuffCredits.SetUseStepSlider(true);
            //DefNotSS2Log.Message("InitializePrism");
            var interactableToken = Interactable.AddComponent<PrismInteractableToken>();
            interactableToken.combatDirector = Interactable.AddComponent<CombatDirector>();
            interactableToken.combatSquad = Interactable.AddComponent<CombatSquad>();
            interactableToken.Interaction = Interactable.GetComponent<PurchaseInteraction>();
            interactableToken.dropTransform = Interactable.GetComponent<Transform>();
            interactableToken.symbolTranform = null;
            interactableToken.behavior = Interactable.GetComponent<ShopTerminalBehavior>();
            
            

            /*
            InteractableDirectorCard.directorCard.selectionWeight = weight;
            InteractableDirectorCard.directorCreditCost = directorCost;
            InteractableDirectorCard.directorCard.minimumStageCompletions = minimumStages;
            */
        }
        //[RequireComponent(typeof(PurchaseInteraction))]

        
        public class PrismInteractableToken : NetworkBehaviour
        {
            bool hasActivated = false;
            public CharacterBody Activator;
            public PurchaseInteraction Interaction;
            public CombatDirector combatDirector;
            public CombatSquad combatSquad;
            public Transform symbolTranform;
            public Xoroshiro128Plus rng;
            public ShopTerminalBehavior behavior;
            
            public PickupIndex index;
            public Transform dropTransform;
            public float dropUpVelocityStrength = 25f;
            public float dropForwardVelocityStrength = 3f;

            public GameObject destroyVFX;

            DirectorCard impCard;
            DirectorCard impBoss;

            public ExplicitPickupDropTable dropTable { get; } = RRPAssets.LoadAsset<ExplicitPickupDropTable>("PrismDroptable", RRPBundle.Interactables);

            public static event Action<PrismInteractableToken> onDefeatedServer;
            
            public void Start()
            {

                destroyVFX = RRPAssets.LoadAsset<GameObject>("PrismVFX", RRPBundle.Interactables);
                
                if (NetworkServer.active && Run.instance)
                {
                    Interaction.SetAvailableTrue();
                }
                //DefNotSS2Log.Message("Getting Combat Squad");
                combatDirector.combatSquad = combatSquad;
                //DefNotSS2Log.Message("Got Combat Squad");
                combatSquad.onDefeatedServer += OnDefeatedServer;
                GetComponent<Highlight>().enabled = true;
                Interaction.onPurchase.AddListener(PrismInteractAttempt);
                rng = new Xoroshiro128Plus(Run.instance.treasureRng.nextUlong);
                combatDirector.expRewardCoefficient = .1f;
                combatDirector.goldRewardCoefficient = .1f;

                List<ItemDef> items = ItemTiers.Sacrificial.ItemDefsWithTier();
                Array.Resize(ref dropTable.pickupEntries, items.Count);
                //DefNotSS2Log.Message(dropTable.pickupEntries.IsFixedSize);
                //DefNotSS2Log.Message(dropTable.pickupEntries.Length);

                impCard = new DirectorCard()
                {
                    spawnCard = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/Base/Imp/cscImp.asset").WaitForCompletion(),
                    selectionWeight = 10,
                    spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                    preventOverhead = false,
                    minimumStageCompletions = 0,
                };
                impBoss = new DirectorCard()
                {
                    spawnCard = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/Base/ImpBoss/cscImpBoss.asset").WaitForCompletion(),
                    selectionWeight = 10,
                    spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                    preventOverhead = false,
                    minimumStageCompletions = 0,
                };


                for (int i = 0; i < dropTable.pickupEntries.Length; i++)
                {
                    ItemDef item = items[i];
                    ExplicitPickupDropTable.PickupDefEntry entry = new ExplicitPickupDropTable.PickupDefEntry();

                    entry.pickupDef = item;
                    
                    if (item == RRPContent.Items.FocusedHemorrhage)
                    {
                        entry.pickupWeight = .15f;
                    }
                    else
                    {
                        entry.pickupWeight = 1f;
                    }
                    
                    dropTable.pickupEntries[i] = entry;
                    //DefNotSS2Log.Message(dropTable.pickupEntries[i].pickupDef);
                }
                dropTable.Regenerate(Run.instance);

                RRPMain.logger.LogMessage("About to generate pickups");
                behavior.GenerateNewPickupServer();
                behavior.UpdatePickupDisplayAndAnimations();
                RRPMain.logger.LogMessage("PickupIndex is:" + behavior.pickupIndex);

                
                
                //DefNotSS2Log.Message(dropTable.pickupEntries);

            }

            public void OnDefeatedServer()
            {
                if (hasActivated)
                {

                    //DefNotSS2Log.Message(index);
                    //DefNotSS2Log.Message(index.pickupDef.itemIndex);
                    behavior.DropPickup();

                    Chat.SimpleChatMessage message = new Chat.SimpleChatMessage();
                    message.baseToken = "RRP_INTERACT_PRISM_END";
                    Chat.SendBroadcastChat(message);
                    
                    if (destroyVFX)
                    {
                        RRPMain.logger.LogMessage("Bloody VFX about to instantiate");
                        EffectManager.SimpleEffect(destroyVFX, dropTransform.position, dropTransform.rotation, true);

                        RRPMain.logger.LogMessage("Bloody VFX instantiated");
                    }
                    
                    Destroy(this.gameObject.transform.GetChild(0).gameObject);
                    Destroy(this.gameObject.transform.GetChild(1).gameObject);
                    this.GetComponent<MeshRenderer>().enabled = false;
                    GetComponent<MeshCollider>().enabled = false;
                }
                
                
            }
            public void PrismInteractAttempt(Interactor interactor)
            {

                //DefNotSS2Log.Message("Attempting Interaction");
                if (!interactor) { return; }
                Interaction.SetAvailable(false);

                float monsterCredit = (float)(Run.instance.difficultyCoefficient * creditsCoef);

                if(NetworkServer.active)
                {
                    DirectorCard card = combatDirector.SelectMonsterCardForCombatShrine(monsterCredit);
                    float rand = UnityEngine.Random.Range(0f, 1f);
                    if (rand > .5f)
                    {
                        RRPMain.logger.LogMessage("Imp Selected");
                        card = impCard;
                    } 
                    if (monsterCredit > 800 && rand > .77f)
                    {
                        card = impBoss;
                    }
                    
                    
                    
                    SacrificeActivation(interactor, monsterCredit, card);

                    /*foreach(CharacterMaster master in combatSquad.membersList)
                    {
                        master.gameObject.AddComponent<PositionIndicator>();
                    } */
                    hasActivated = true;
                }
            }

            public void SacrificeActivation(Interactor interactor, float monsterCredit, DirectorCard card)
            {
                combatDirector.enabled = true;
                combatDirector.monsterCredit += monsterCredit;
                combatDirector.OverrideCurrentMonsterCard(card);
                combatDirector.monsterSpawnTimer = 0f;
                //combatDirector.CombatShrineActivation
                CharacterMaster component = card.spawnCard.prefab.GetComponent<CharacterMaster>();
                if ((bool)(UnityEngine.Object)(object)component)
                {
                    CharacterBody component2 = component.bodyPrefab.GetComponent<CharacterBody>();
                    if ((bool)(UnityEngine.Object)(object)component2)
                    {
                        Chat.SubjectFormatChatMessage subjectFormatChatMessage = new Chat.SubjectFormatChatMessage();
                        subjectFormatChatMessage.subjectAsCharacterBody = ((Component)(object)interactor).GetComponent<CharacterBody>();
                        subjectFormatChatMessage.baseToken = "RRP_INTERACT_PRISM_USE";
                        subjectFormatChatMessage.paramTokens = new string[1] { component2.baseNameToken };
                        Chat.SendBroadcastChat(subjectFormatChatMessage);
                    }
                }
                //combatDirector.enabled = false;
            }
        }
    }

}