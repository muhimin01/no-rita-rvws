// RITA Settings for BepInEx Configuration Manager
using BepInEx.Configuration;

namespace RITA_RVWS
{
    internal static class Configuration
    {
        internal static ConfigEntry<bool> RITAEnabled = null!;
        internal static ConfigEntry<bool> RITAFactionBDF = null!;
        internal static ConfigEntry<bool> RITAFactionPALA = null!;

        internal static void InitConfig(ConfigFile config)
        {
            RITAEnabled = config.Bind("Settings", "RITA", true, "Enable RITA");
            RITAFactionBDF = config.Bind("Factions", "BDF", true, "Enable RITA for BDF");
            RITAFactionPALA = config.Bind("Factions", "PALA", true, "Enable RITA for PALA");
        }
    }
}
