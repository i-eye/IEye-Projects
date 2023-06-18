using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using RoR2.Items;
using R2API;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using HG.Coroutines;

namespace IEye.RulersOfTheRedPlane.Items
{

    
    public class SacrificialHelper : ItemBase
    {
        public override ItemDef ItemDef { get; } = RRPAssets.LoadAsset<ItemDef>("SacrificialHelper", RRPBundle.Items);

        public sealed class Behavior: BaseItemBodyBehavior, IOnKilledOtherServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.SacrificialHelper;

            // Introspective Insect
            int num1 = 0;
            //Aggresive Insect
            int num1Bloody = 0;
            int num1KillCount = 0;
            bool count1Going = false;
            
            //Energy Drink
            int num2 = 0;
            // Adrenaline Frenzy
            int num2Bloody = 0;
            int num2KillCount = 0;
            bool count2Going = false;
            
            
            

            private void Start()
            {
                body.onInventoryChanged += CheckForSacrifice;
                CheckForSacrifice();
            }

            private void CheckForSacrifice()
            {

                num1 = body.inventory.GetItemCount(RRPContent.Items.IntrospectiveInsect);
                num1Bloody = body.inventory.GetItemCount(RRPContent.Items.AgressiveInsect);
                DefNotSS2Log.Message("num1: " + num1 + " num1Bloody: " + num1Bloody);
                num2 = body.inventory.GetItemCount(RoR2Content.Items.SprintBonus);
                num2Bloody = body.inventory.GetItemCount(RRPContent.Items.AdrenalineFrenzy);
                DefNotSS2Log.Message("num2: " + num2 + " num2Bloody: " + num2Bloody);

                if (num1 > 0 && num1Bloody > 0 && !count1Going)
                {
                    StartCoroutine(SacrificeItem(1));
                }
                
                if (num2 > 0 && num2Bloody > 0 && !count2Going)
                {
                    StartCoroutine(SacrificeItem(2));
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
                
            }


            IEnumerator SacrificeItem(int num)
            {
                
                int numSacrifice = calcSacrifices();
                DefNotSS2Log.Message("Starting Sacrifice, int = "+num+" and kills needed = "+numSacrifice);
                switch (num){
                    case 1:
                        num1KillCount = 0;
                        count1Going = true;
                        yield return new WaitUntil(() => num1KillCount == numSacrifice);
                        body.inventory.GiveItem(RRPContent.Items.AgressiveInsect, 1);
                        body.inventory.RemoveItem(RRPContent.Items.IntrospectiveInsect, 1);
                        count1Going = false;
                        CheckForSacrifice();
                        break;
                        
                    case 2:
                        num2KillCount = 0;
                        count2Going = true;
                        yield return new WaitUntil(() => num2KillCount == numSacrifice);
                        body.inventory.GiveItem(RRPContent.Items.AdrenalineFrenzy, 1);
                        body.inventory.RemoveItem(RoR2Content.Items.SprintBonus, 1);
                        count2Going = false;
                        CheckForSacrifice();
                        break;
                        
                }
                
            }

            private int calcSacrifices()
            {
                
                int i = Run.instance.ambientLevelFloor;
                DefNotSS2Log.Message(i + " is ambientLevelFloor");
                return 3 * Run.instance.ambientLevelFloor;
            }

            
        }
    }
}
