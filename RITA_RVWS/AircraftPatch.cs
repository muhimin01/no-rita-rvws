// Aircraft related Harmony patches, used to get the instance of the local player, current faction, and aircraft
using HarmonyLib;
using NuclearOption.Networking;
using System;

namespace RITA_RVWS.Patches
{
	internal class AircraftPatch
	{
		internal static string? currentFaction;

		[HarmonyPatch(typeof(Player), "SetAircraft")]
		internal static class PatchSetAircraft
		{
			private static void Postfix(Player __instance, Aircraft aircraft)
			{
				//RITA.Log.LogDebug("[PatchSetAircraft] Triggered");

				if (!GameManager.IsLocalPlayer(__instance)) return;

				try
				{
					//RITA.Log.LogDebug($"[PatchSetAircraft]: {__instance}, {__instance.HQ}, {aircraft}");
					currentFaction = __instance.HQ.name;
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
				//RITA.Log.LogDebug("[PatchRemoveAircraft] Triggered");

				if (!GameManager.IsLocalPlayer(__instance)) return;
				currentFaction = null;
			}
		}
	}
}
