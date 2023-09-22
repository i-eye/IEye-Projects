using R2API;
using RoR2;
using Moonstorm;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using R2API.ScriptableObjects;
using System.Collections;
using System.Security.Cryptography;

namespace IEye.RRP.Artifacts
{
    public sealed class Looper : ArtifactBase
    {
        public override ArtifactDef ArtifactDef { get; } = RRPAssets.LoadAsset<ArtifactDef>("Loop", RRPBundle.Artifacts);
        public override ArtifactCode ArtifactCode => RRPAssets.LoadAsset<ArtifactCode>("LoopCode", RRPBundle.Artifacts);
        
        public override void OnArtifactDisabled()
        {
            
            
            
            Run.onRunStartGlobal -= ChangeRun;
            //Run.onRunStartGlobal -= AddStuff;
            
        }

        public override void OnArtifactEnabled()
        {
            
            
            Run.onRunStartGlobal += ChangeRun;
            //Run.onRunStartGlobal += AddStuff;
            
            
            
        }
       


        public static void ChangeRun(Run run)
        {
            
            run.stageClearCount += 5;
            RRPMain.logger.LogMessage("Stage Increased");
            run.fixedTime += Random.Range(2100f, 2520f);
            RRPMain.logger.LogMessage("Time Increased");

            RRPMain.logger.LogMessage("onPlayerFirstCreatedServer run");

            if (PlayerCharacterMasterController.instances != null)
            {
                foreach (PlayerCharacterMasterController instance in PlayerCharacterMasterController.instances)
                {
                    if (instance != null)
                    {
                        instance.master.inventory.GiveRandomItems(Random.Range(34, 42), false, false);
                        instance.master.inventory.GiveItem(RRPContent.Items.LevelGiver, 1);
                        instance.master.onBodyStart += AddExperience;
                        
                        //instance.master.GiveExperience((ulong)Random.Range(180000, 230000));
                        //instance.master.GetBody().RecalculateStats();
                    }

                }

            }

            RRPMain.logger.LogMessage("Items Given");


        }
        public static void AddExperience(CharacterBody body)
        {
            RRPMain.logger.LogMessage("Adding Experience");
            RRPMain.logger.LogMessage("Body" + body);
            body.master.GiveExperience((ulong)Random.Range(100000, 150000));
        }

        
    }
    
}