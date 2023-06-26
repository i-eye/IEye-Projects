using Moonstorm.Loaders;

namespace IEye.RRP
{
    public class RRPLanguage : LanguageLoader<RRPLanguage>
    {
        public override string AssemblyDir => RRPAssets.Instance.AssemblyDir;

        public override string LanguagesFolderName => "RRPLang";

        ///Due to the nature of the language system in ror, we cannot load our language file using system initializer, as its too late.
        internal void Init()
        {
            LoadLanguages();
            TMProEffects.Init();
        }
    }
}