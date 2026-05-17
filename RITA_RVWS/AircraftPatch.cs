using HarmonyLib;
using NuclearOption.Networking;
using System;
using UnityEngine;

namespace RITA_RVWS.Patches
{
    internal class AircraftPatch
    {
        [HarmonyPatch(typeof(Player), "SetAircraft")]
        internal static class PatchSetAircraft
        {
            private static void Postfix(Player __instance, Aircraft aircraft)
            {
                bool RITAEnabled = Configuration.RITAEnabled.Value;
                if (!RITAEnabled || !GameManager.IsLocalPlayer(__instance)) return;
                try
                {
                    RITA.Log.LogDebug($"[AircraftPatch]: {__instance}, {__instance.HQ}, {aircraft}");
                    if (RITA.RITADevMode)
                    {
                        RITA.Log.LogDebug($"[AircraftPatch]: Player: {__instance}");
                        Dev.RITADebug.LogObjectContents(__instance);

                        RITA.Log.LogDebug($"[AircraftPatch]: Aircraft: {aircraft}");
                        Dev.RITADebug.LogObjectContents(aircraft);
                    }
                }
                catch (Exception ex)
                {
                    RITA.Log.LogError($"[AircraftPatch]: {ex}");
                }
            }
        }
    }
}
