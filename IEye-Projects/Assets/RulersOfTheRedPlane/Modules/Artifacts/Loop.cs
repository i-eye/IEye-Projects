using R2API;
using RoR2;
using MSU;
using R2API.AddressReferencedAssets;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using R2API.ScriptableObjects;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Security.Cryptography;
using RoR2.ContentManagement;

namespace IEye.RRP.Artifacts
{
    public sealed class Loop : RRPArtifact
    {
        public override RRPAssetRequest LoadAssetRequest() => RRPAssets.LoadAssetAsync<ArtifactAssetCollection>("acLoop", RRPBundle.Artifacts);

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }

        public override void Initialize()
        {
            //ArtifactDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Teleporters/LunarTeleporter Variant.prefab").WaitForCompletion();
        }
        public override void OnArtifactDisabled()
        {
            
            
            
            Run.onRunStartGlobal -= ChangeRun;
            
            //Run.onRunStartGlobal -= AddStuff;
            
        }

        public override void OnArtifactEnabled()
        {

            if (Run.instance.stageClearCount != 0)
            {
                Run.instance.runStopwatch.isPaused = false;
                ChangeRun(Run.instance);
         
            } else
            {
                Run.onRunStartGlobal += ChangeRun;
            }
            
            //Run.onRunStartGlobal += AddStuff;
            
            
            
        }
       


        public static void ChangeRun(Run run)
        {
            
            run.stageClearCount += 5;
            RRPLog.Debug("Stage Increased");
            float randTime = Random.Range(2100f, 2520f);
            //run.difficultyCoefficient += Random.Range(8.8f,10f);
            run.time = randTime;
            run.NetworkfixedTime = randTime;
            
            

            RRPLog.Message("Time Increased");

            //RRPLog.Message("onPlayerFirstCreatedServer run");

            if (PlayerCharacterMasterController.instances != null)
            {
                foreach (PlayerCharacterMasterController instance in PlayerCharacterMasterController.instances)
                {
                    if (instance != null)
                    {
                        instance.master.inventory.GiveRandomItems(Random.Range(35, 45), false, false);
                        instance.master.inventory.GiveRandomEquipment();
                        instance.master.onBodyStart += AddExperience;
                        
                        
                        //instance.master.GiveExperience((ulong)Random.Range(180000, 230000));
                        //instance.master.GetBody().RecalculateStats();
                    }

                }

            }

            RRPLog.Message("Items Given");


        }
        public static void AddExperience(CharacterBody body)
        {
            RRPLog.Message("Adding Experience");
            RRPLog.Message("Body" + body);
            body.master.GiveExperience((ulong)Random.Range(110000, 160000));
            body.master.onBodyStart -= AddExperience;
        }

        
    }
    
}