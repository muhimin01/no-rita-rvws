// RITA Settings for BepInEx Configuration Manager
using BepInEx.Configuration;
using System.Collections.Generic;

namespace RITA_RVWS
{
    internal static class Configuration
    {
        internal static ConfigEntry<bool> RITAEnabled = null!;
        internal static ConfigEntry<bool> RITAFactionBDF = null!;
        internal static ConfigEntry<bool> RITAFactionPALA = null!;
        internal static ConfigEntry<bool> RITAFactionCustom = null!;

        private static ConfigurationManagerAttributes _factionBDFAttributes = new ConfigurationManagerAttributes { Order = 3 };
        private static ConfigurationManagerAttributes _factionPALAAttributes = new ConfigurationManagerAttributes { Order = 2 };
        private static ConfigurationManagerAttributes _factionCustomAttributes = new ConfigurationManagerAttributes { Order = 1 };

        // Add new official factions here alongside their ConfigEntry
        internal static Dictionary<string, ConfigEntry<bool>> OfficialFactions = null!;

        internal static void InitConfig(ConfigFile config)
        {
            RITAEnabled = config.Bind("Settings", "RITA", true, "Enable RITA");

            RITAFactionBDF = config.Bind(
                "Factions",
                "BDF",
                true,
                new ConfigDescription("Enable RITA for BDF", null, _factionBDFAttributes)
            );

            RITAFactionPALA = config.Bind(
                "Factions",
                "PALA",
                true,
                new ConfigDescription("Enable RITA for PALA", null, _factionPALAAttributes)
            );

            RITAFactionCustom = config.Bind(
                "Factions",
                "Custom Factions",
                true,
                new ConfigDescription("Enable RITA for custom factions", null, _factionCustomAttributes)
            );

            // Built once after all entries are bound
            OfficialFactions = new Dictionary<string, ConfigEntry<bool>>
            {
                { "BoscaliHQ",  RITAFactionBDF  },
                { "PrimevaHQ",  RITAFactionPALA },
            };
        }

        internal static void SetFactionSettingsState(bool enabled)
        {
            //RITA.Log.LogDebug("[SetFactionSettingsState] Triggered");

            _factionBDFAttributes.ReadOnly = !enabled;
            _factionPALAAttributes.ReadOnly = !enabled;
            _factionCustomAttributes.ReadOnly = !enabled;

            _factionBDFAttributes.Browsable = enabled;
            _factionPALAAttributes.Browsable = enabled;
            _factionCustomAttributes.Browsable = enabled;

            //RITA.Log.LogDebug("[SetFactionSettingsState] Faction settings set to read-only");
        }
    }

    internal sealed class ConfigurationManagerAttributes
    {
        public int? Order;
        public bool Browsable = true;
        public bool ReadOnly = false;
    }
}
