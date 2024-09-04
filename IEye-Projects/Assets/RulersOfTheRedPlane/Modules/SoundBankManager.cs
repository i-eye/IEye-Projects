using RoR2;
using UnityEngine;
using Path = System.IO.Path;

namespace IEye.RRP
{
    public static class SoundBankManager
    {
        public static string soundBankDirectory
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(RRPMain.instance.Info.Location), "soundbanks");
            }
        }

        public static void Init()
        {
            AkSoundEngine.AddBasePath(soundBankDirectory);
            AkSoundEngine.LoadFilePackage("RRP_Soundbank.pck", out var packageID);
            AkSoundEngine.LoadBank("RRP_Soundbank", out var bank);
        }
    }
}