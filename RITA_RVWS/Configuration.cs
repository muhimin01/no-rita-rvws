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

        private static ConfigurationManagerAttributes _factions = new ConfigurationManagerAttributes { Browsable = true, ReadOnly = false };

        internal static void InitConfig(ConfigFile config)
        {
            RITAEnabled = config.Bind("Settings", "RITA", true, "Enable RITA");
            
            RITAFactionBDF = config.Bind(
                "Factions", 
                "BDF", 
                true, 
                new ConfigDescription("Enable RITA for BDF", null, _factions)
            );
            
            RITAFactionPALA = config.Bind(
                "Factions", 
                "PALA", 
                true, 
                new ConfigDescription("Enable RITA for PALA", null, _factions)
            );
        }

        internal static void SetFactionSettingsState(bool enabled)
        {
            RITA.Log.LogDebug("[SetFactionSettingsState] Triggered");

            _factions.ReadOnly = !enabled;
            //_factions.Browsable = enabled;

            RITA.Log.LogDebug("[SetFactionSettingsState] Faction settings set to read-only");

            //RITAFactionBDF.Value = RITAFactionBDF.Value;
            //RITAFactionPALA.Value = RITAFactionPALA.Value;

        }
    }

#pragma warning disable 0169, 0414, 0649
    internal sealed class ConfigurationManagerAttributes
    {
        public string? DispName;
        public string? Category;
        public int? Order;
        public bool? Browsable;
        public bool? IsAdvanced;
        public bool? ReadOnly;
        public bool? ShowRangeAsPercent;
        public bool? HideDefaultButton;
        public object? DefaultValue;
    }
}
