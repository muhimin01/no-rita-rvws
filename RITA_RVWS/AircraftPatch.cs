// TODO: Faction config setting implementation
using HarmonyLib;
using NuclearOption.Networking;
using System;

namespace RITA_RVWS.Patches
{
    internal class AircraftPatch
    {
        [HarmonyPatch(typeof(Player), "SetAircraft")]
        internal static class PatchSetAircraft
        {
            private static void Postfix(Player __instance, Aircraft aircraft)
            {
                RITA.Log.LogDebug("[PatchSetAircraft] Triggered");

                if (!Configuration.RITAEnabled.Value || !GameManager.IsLocalPlayer(__instance)) return;
                try
                {
                    RITA.Log.LogDebug($"[PatchSetAircraft]: {__instance}, {__instance.HQ}, {aircraft}");
                }
                catch (Exception ex)
                {
                    RITA.Log.LogError($"[AircraftPatch] Exception: {ex}");
                }
            }
        }

        [HarmonyPatch(typeof(Player), "RemoveAircraft")]
        internal static class PatchRemoveAircraft
        {
            private static void Postfix(Player __instance)
            {
                if (!GameManager.IsLocalPlayer(__instance)) return;
            }
        }
    }
}
