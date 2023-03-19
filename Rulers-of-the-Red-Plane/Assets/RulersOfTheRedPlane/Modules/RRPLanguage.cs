using Moonstorm.Loaders;

namespace IEye.RulersOfTheRedPlane {
    public class RRPLanguage : LanguageLoader<RRPLanguage>
    {
        public override string AssemblyDir => RRPAssets.Instance.AssemblyDir;

        public override string LanguagesFolderName => "RRPLang";

        internal void Init()
        {
            LoadLanguages();
            TMProEffects.Init();
        }
    }
}