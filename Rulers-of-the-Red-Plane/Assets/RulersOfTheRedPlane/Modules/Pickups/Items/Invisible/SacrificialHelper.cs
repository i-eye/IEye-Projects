using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using RoR2.UI;
using R2API;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using HG.Coroutines;
using IEye.RRP.ItemTiers;

namespace IEye.RRP.Items
{

    
    public class SacrificialHelper : ItemBase
    {
        /*
        private static HUD hudInstance;

        [SystemInitializer(typeof(SacrificialHelper))]
        private static void SystemInit()
        {
            
            On.RoR2.UI.HUD.Awake += GetHUD;
            DefNotSS2Log.Info("BOOOOOOOOOOM SYSTEMINIT");
        }
        private static void GetHUD(On.RoR2.UI.HUD.orig_Awake orig, HUD self)
        {
            orig(self);
            hudInstance = self;
        }
        */

        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("SacrificialHelper", RRPBundle.Items);

        public sealed class Behavior: BaseItemBodyBehavior, IOnKilledOtherServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.SacrificialHelper;

            public static GameObject SacrificalAnnouncer = RRPAssets.LoadAsset<GameObject>("SacrifcialAnnouncer", RRPBundle.Base);
           

            // Introspective Insect
            int num1 = 0;
            // Aggresive Insect
            int num1Bloody = 0;
            int num1KillCount = 0;
            bool count1Going = false;
            
            //Energy Drink
            int num2 = 0;
            // Adrenaline Frenzy
            int num2Bloody = 0;
            int num2KillCount = 0;
            bool count2Going = false;

            // tri-tip
            int num3one = 0;
            // 4d dagger
            int num3two = 0;
            // focused hemorrhage
            int num3Bloody = 0;
            int num3KillCount = 0;
            bool count3Going = false;

            // predatory vanilla
            int num4 = 0;
            // predatory blood
            int num4Bloody = 0;
            int num4KillCount = 0;
            bool count4Going = false;

            

            private void Start()
            {
                SacrificalAnnouncer = RRPAssets.LoadAsset<GameObject>("SacrifcialAnnouncer", RRPBundle.Base);
                body.onInventoryChanged += CheckForSacrifice;
                CheckForSacrifice();
                if (num1 > 0 && num1Bloody > 0)
                {
                    StartCoroutine(SacrificeItem(1));
                }

                if (num2 > 0 && num2Bloody > 0)
                {
                    StartCoroutine(SacrificeItem(2));
                }

                if (num3one > 0 && num3two > 0 && num3Bloody > 0)
                {
                    StartCoroutine(SacrificeItem(3));
                }

                if (num4 > 0 && num4Bloody > 0)
                {
                    StartCoroutine(SacrificeItem(4));
                }
            }
            
            private void CheckForSacrifice()
            {

                num1 = body.inventory.GetItemCount(RRPContent.Items.IntrospectiveInsect);
                num1Bloody = body.inventory.GetItemCount(RRPContent.Items.AgressiveInsect);
                //DefNotSS2Log.Message("num1: " + num1 + " num1Bloody: " + num1Bloody);
                num2 = body.inventory.GetItemCount(RoR2Content.Items.SprintBonus);
                num2Bloody = body.inventory.GetItemCount(RRPContent.Items.AdrenalineFrenzy);
                //DefNotSS2Log.Message("num2: " + num2 + " num2Bloody: " + num2Bloody);
                num3one = body.inventory.GetItemCount(RoR2Content.Items.BleedOnHit);
                num3two = body.inventory.GetItemCount(RRPContent.Items.FourDimensionalDagger);
                num3Bloody = body.inventory.GetItemCount(RRPContent.Items.FocusedHemorrhage);

                num4 = body.inventory.GetItemCount(RoR2Content.Items.AttackSpeedOnCrit);
                num4Bloody = body.inventory.GetItemCount(RRPContent.Items.PredatorySavagery);

                if (num1 > 0 && num1Bloody > 0 && !count1Going)
                {
                    StartCoroutine(SacrificeItem(1));
                }
                
                if (num2 > 0 && num2Bloody > 0 && !count2Going)
                {
                    StartCoroutine(SacrificeItem(2));
                }

                if(num3one > 0 && num3two > 0 && num3Bloody > 0 && !count3Going)
                {
                    StartCoroutine(SacrificeItem(3));
                }

                if(num4 > 0 && num4Bloody > 0 && !count4Going)
                {
                    StartCoroutine(SacrificeItem(4));
                }
                
            }

            public void OnKilledOtherServer(DamageReport report)
            {
                if (num1 > 0 && num1Bloody > 0)
                {
                    num1KillCount++;
                }
                
                if (num2 > 0 && num2Bloody > 0)
                {
                    num2KillCount++;
                }
                if (num3one > 0 && num3two > 0 && num3Bloody > 0)
                {
                    num3KillCount++;
                }
                if(num4 > 0 && num4Bloody > 0)
                {
                    num4KillCount++;
                }


            }


            IEnumerator SacrificeItem(int num)
            {
                
                int numSacrifice = calcSacrifices();
                DefNotSS2Log.Message("Starting Sacrifice, int = "+num+" and kills needed = "+numSacrifice);
                switch (num){
                    case 1:
                        
                        count1Going = true;
                        yield return new WaitUntil(() => num1KillCount == numSacrifice);
                        //StartCoroutine(RunSacrificialAnnouncer(RRPContent.Items.IntrospectiveInsect, RRPContent.Items.AgressiveInsect));
                        body.inventory.GiveItem(RRPContent.Items.AgressiveInsect, 1);
                        body.inventory.RemoveItem(RRPContent.Items.IntrospectiveInsect, 1);
                        SacrificeChat(RRPContent.Items.IntrospectiveInsect.nameToken, RRPContent.Items.AgressiveInsect.nameToken);
                        count1Going = false;
                        num1KillCount = 0;
                        CheckForSacrifice();
                        break;
                        
                    case 2:
                        
                        count2Going = true;
                        yield return new WaitUntil(() => num2KillCount == numSacrifice);
                        //StartCoroutine(RunSacrificialAnnouncer(RoR2Content.Items.SprintBonus, RRPContent.Items.AdrenalineFrenzy));
                        body.inventory.GiveItem(RRPContent.Items.AdrenalineFrenzy, 1);
                        body.inventory.RemoveItem(RoR2Content.Items.SprintBonus, 1);
                        SacrificeChat(RoR2Content.Items.SprintBonus.nameToken, RRPContent.Items.AdrenalineFrenzy.nameToken);
                        count2Going = false;
                        num2KillCount = 0;
                        CheckForSacrifice();
                        break;
                    case 3:
                        
                        count3Going = true;
                        yield return new WaitUntil(() => num3KillCount == numSacrifice);
                        body.inventory.GiveItem(RRPContent.Items.FocusedHemorrhage, 1);
                        body.inventory.RemoveItem(RoR2Content.Items.BleedOnHit, 1);
                        body.inventory.RemoveItem(RRPContent.Items.FourDimensionalDagger, 1);
                        SacrificeChat(RoR2Content.Items.BleedOnHit.nameToken, RRPContent.Items.FourDimensionalDagger.nameToken, RRPContent.Items.FocusedHemorrhage.nameToken);
                        num3KillCount = 0;
                        count3Going = false;
                        CheckForSacrifice();
                        break;
                    case 4:
                        
                        count4Going = true;
                        yield return new WaitUntil(() => num4KillCount == numSacrifice);
                        body.inventory.GiveItem(RRPContent.Items.PredatorySavagery, 1);
                        body.inventory.RemoveItem(RoR2Content.Items.AttackSpeedOnCrit, 1);
                        SacrificeChat(RoR2Content.Items.AttackSpeedOnCrit.nameToken, RRPContent.Items.PredatorySavagery.nameToken);
                        num4KillCount = 0;
                        count4Going = false;
                        CheckForSacrifice();
                        break;
                }
                
            }

            private int calcSacrifices()
            {
                
                float i = Run.instance.difficultyCoefficient;
                //DefNotSS2Log.Message(i + " is difficulty");
                int needed = (int)(Sacrificial.multiplier * i);
                if (needed > Sacrificial.cap)
                {
                    return Sacrificial.cap;
                } else
                {
                    return needed;
                }
            }

            private void SacrificeChat(string one, string two)
            {
                Chat.SubjectFormatChatMessage subjectFormatChatMessage = new Chat.SubjectFormatChatMessage();
                subjectFormatChatMessage.subjectAsCharacterBody = body;
                subjectFormatChatMessage.baseToken = "RRP_SACRIFICE_ITEM_REG";
                subjectFormatChatMessage.paramTokens = new string[2] { one, two };
                Chat.SendBroadcastChat(subjectFormatChatMessage);
            }

            private void SacrificeChat(string one, string two, string three)
            {
                Chat.SubjectFormatChatMessage subjectFormatChatMessage = new Chat.SubjectFormatChatMessage();
                subjectFormatChatMessage.subjectAsCharacterBody = body;
                subjectFormatChatMessage.baseToken = "RRP_SACRIFICE_ITEM_DOUBLE";
                subjectFormatChatMessage.paramTokens = new string[3] { one, two, three};
                Chat.SendBroadcastChat(subjectFormatChatMessage);
            }

            /*
            private IEnumerator RunSacrificialAnnouncer(ItemDef item1, ItemDef item2)
            {
                DefNotSS2Log.Message("Instatizating Announcer");
                
                //DefNotSS2Log.Message(hudInstance.mainContainer.transform);
                DefNotSS2Log.Message(SacrificalAnnouncer);
                GameObject announcer = Instantiate(SacrificalAnnouncer, hudInstance.mainContainer.transform, false);
                DefNotSS2Log.Message("Getting SpriteRenderers");
                SpriteRenderer[] renderers = announcer.GetComponentsInChildren<SpriteRenderer>();
                DefNotSS2Log.Message("Foreach loop");
                foreach(SpriteRenderer renderer in renderers)
                {
                    if(renderer.name == "Normal")
                    {
                        renderer.sprite = item1.pickupIconSprite;
                    }
                    else if(renderer.name == "Sacrificial")
                    {
                        renderer.sprite = item2.pickupIconSprite;
                    }
                }
                yield return new WaitForSeconds(5f);
                Destroy(announcer);
                
            }
            */






        }
    }
}
