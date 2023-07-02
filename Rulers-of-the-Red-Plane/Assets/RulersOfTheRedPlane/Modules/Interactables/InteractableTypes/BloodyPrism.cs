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

namespace IEye.RRP.Interactables
{
    //[DisabledContent]
    
    public class BloodyPrism : InteractableBase
    {
        public override GameObject Interactable { get; } = RRPAssets.LoadAsset<GameObject>("BloodyPrismGameObject", RRPBundle.Interactables);

        public override MSInteractableDirectorCard InteractableDirectorCard { get; } = RRPAssets.LoadAsset<MSInteractableDirectorCard>("BloodyPrism", RRPBundle.Interactables);


        [RooConfigurableField(RRPConfig.IDInteractable, ConfigDesc = "Credits multiplied by difficulty for prism on use(default 110)")]
        public static int creditsCoef = 110;
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
            
            //DefNotSS2Log.Message("InitializePrism");
            var interactableToken = Interactable.AddComponent<PrismInteractableToken>();
            interactableToken.combatDirector = Interactable.AddComponent<CombatDirector>();
            interactableToken.combatSquad = Interactable.AddComponent<CombatSquad>();
            interactableToken.Interaction = Interactable.GetComponent<PurchaseInteraction>();
            interactableToken.dropTransform = Interactable.GetComponent<Transform>();
            interactableToken.symbolTranform = null;
            InteractableDirectorCard.DirectorCardHolder.InteractableCategorySelectionWeight = 4;
            

            /*
            InteractableDirectorCard.directorCard.selectionWeight = weight;
            InteractableDirectorCard.directorCreditCost = directorCost;
            InteractableDirectorCard.directorCard.minimumStageCompletions = minimumStages;
            */
        }
        //[RequireComponent(typeof(PurchaseInteraction))]
        public class PrismInteractableToken : NetworkBehaviour
        {
            public CharacterBody Activator;
            public PurchaseInteraction Interaction;
            public CombatDirector combatDirector;
            public CombatSquad combatSquad;
            public Transform symbolTranform;
            public Xoroshiro128Plus rng;
            public PickupIndex index;
            public Transform dropTransform;
            public float dropUpVelocityStrength = 25f;
            public float dropForwardVelocityStrength = 3f;

            public ExplicitPickupDropTable dropTable { get; } = RRPAssets.LoadAsset<ExplicitPickupDropTable>("PrismDroptable", RRPBundle.Interactables);

            public static event Action<PrismInteractableToken> onDefeatedServer;

            public void Start()
            {
                
                if (NetworkServer.active && Run.instance)
                {
                    Interaction.SetAvailableTrue();
                }
                //DefNotSS2Log.Message("Getting Combat Squad");
                combatDirector.combatSquad = combatSquad;
                //DefNotSS2Log.Message("Got Combat Squad");
                combatSquad.onDefeatedServer += OnDefeatedServer;
                Interaction.onPurchase.AddListener(PrismInteractAttempt);
                rng = new Xoroshiro128Plus(Run.instance.treasureRng.nextUlong);
                combatDirector.expRewardCoefficient = .1f;
                combatDirector.goldRewardCoefficient = .1f;

                List<ItemDef> items = ItemTiers.Sacrificial.ItemDefsWithTier();
                Array.Resize(ref dropTable.pickupEntries, items.Count);
                //DefNotSS2Log.Message(dropTable.pickupEntries.IsFixedSize);
                //DefNotSS2Log.Message(dropTable.pickupEntries.Length);
                for(int i = 0; i < dropTable.pickupEntries.Length; i++)
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
                //DefNotSS2Log.Message(dropTable.pickupEntries);
                
            }

            public void OnDefeatedServer()
            {
                index = dropTable.GenerateDrop(rng);
                //DefNotSS2Log.Message(index);
                //DefNotSS2Log.Message(index.pickupDef.itemIndex);
                Vector3 val = Vector3.up * dropUpVelocityStrength + dropTransform.forward * dropForwardVelocityStrength;
                PickupDropletController.CreatePickupDroplet(index, dropTransform.position + Vector3.up * 1.5f, val);

                Chat.SimpleChatMessage message = new Chat.SimpleChatMessage();
                message.baseToken = "RRP_INTERACT_PRISM_END";
                Chat.SendBroadcastChat(message);
                
                Destroy(this.gameObject);
                
                
                
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
                    SacrificeActivation(interactor, monsterCredit, card);
                    
                    /*foreach(CharacterMaster master in combatSquad.membersList)
                    {
                        master.gameObject.AddComponent<PositionIndicator>();
                    } */
                }
            }

            public void SacrificeActivation(Interactor interactor, float monsterCredit, DirectorCard card)
            {
                combatDirector.enabled = true;
                combatDirector.monsterCredit += monsterCredit;
                combatDirector.OverrideCurrentMonsterCard(card);
                combatDirector.monsterSpawnTimer = 0f;
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