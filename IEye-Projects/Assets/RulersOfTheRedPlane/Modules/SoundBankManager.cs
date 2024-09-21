using MonoMod.RuntimeDetour;
using MSU;
using R2API.Utils;
using RoR2;
using UnityEngine;
using System;
using Path = System.IO.Path;

namespace IEye.RRP
{
    public static class SoundBankManager
    {

        public static void Init()
        {
            var hook = new Hook(
            typeof(AkSoundEngineInitialization).GetMethodCached(nameof(AkSoundEngineInitialization.InitializeSoundEngine)),
            typeof(SoundBankManager).GetMethodCached(nameof(AddBanks)));


        }

        private static bool AddBanks(Func<AkSoundEngineInitialization, bool> orig, AkSoundEngineInitialization self)
        {
            var res = orig(self);

            LoadBanks();

            return res;
        }

        private static void LoadBanks()
        {
            //LogCore.LogE(AkSoundEngine.ClearBanks().ToString());
            AkSoundEngine.AddBasePath(soundBankDirectory);
            AkSoundEngine.LoadFilePackage("RRP_Soundbank.pck", out var packageID);
            AkSoundEngine.LoadBank("RRP_Soundbank", out var bank);
        }

        public static string soundBankDirectory
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(RRPMain.instance.Info.Location), "soundbanks");
            }
        }
    }
}