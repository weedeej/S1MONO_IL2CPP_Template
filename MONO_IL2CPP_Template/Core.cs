using MelonLoader;

[assembly: MelonInfo(typeof(MONO_IL2CPP_Template.Core), "MONO_IL2CPP_Template", "1.0.0", "Dixie", null)] // Change this
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MONO_IL2CPP_Template
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}