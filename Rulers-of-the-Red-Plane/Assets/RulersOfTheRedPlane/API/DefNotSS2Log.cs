using BepInEx.Logging;
using System.Runtime.CompilerServices;
using Moonstorm.Loaders;

namespace IEye.RRP
{
    public class DefNotSS2Log : LogLoader
    {
        

        public DefNotSS2Log(ManualLogSource logSource) : base(logSource)
        {
            LogSource = logSource;
        }
        
        public override ManualLogSource LogSource { get; protected set; }

        public override BreakOnLog BreakOn { get; }
    }
}
