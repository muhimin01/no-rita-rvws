using HarmonyLib;
using NuclearOption.Networking;
using System;
using UnityEngine;

namespace RITA_RVWS.Patches
{
    internal class PatchAircraft
    {
        [HarmonyPatch(typeof(Player), "SetAircraft")]
        internal static class PatchSetAircraft
        {
            private static void Postfix(Player __instance, Aircraft aircraft)
            {
                if (!Configuration.RITAEnabled.Value || !GameManager.IsLocalPlayer(__instance)) return;
                try
                {
                    RITA.Log.LogDebug($"{__instance}, {__instance.HQ}, {aircraft}");
                    //RITA.LogClips();
                }
                catch (Exception ex)
                {
                    Debug.Log($"[RITA.RVWS.Patches.PatchAircraft]: {ex}");
                }
            }
        }
    }
}
