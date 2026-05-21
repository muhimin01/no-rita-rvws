// Player related Harmony patches, used to get the instance of the local player, current faction, and aircraft
using HarmonyLib;
using NuclearOption.Networking;
using System;

namespace RITA_RVWS.Patches
{
	internal class PlayerPatch
	{
		internal static string? currentFaction;

		[HarmonyPatch(typeof(FactionHQ), "OnPlayersChange")]
		internal static class PatchFactionHQOnPlayersChange
		{
			private static void Postfix(FactionHQ __instance)
			{
				//RITA.Log.LogDebug("[FactionHQOnPlayersChange] Triggered");

				// Check if this FactionHQ belongs to the local player
				if (!GameManager.IsLocalHQ(__instance)) return;
				//RITA.Log.LogDebug($"[FactionHQOnPlayersChange] Local Player faction: {__instance.name}");

				currentFaction = __instance.name;
				//RITA.Log.LogDebug($"[FactionHQOnPlayersChange] Faction set: {currentFaction}");
			}
		}

		[HarmonyPatch(typeof(Player), "SetAircraft")]
		internal static class PatchSetAircraft
		{
			private static void Postfix(Player __instance, Aircraft aircraft)
			{
				//RITA.Log.LogDebug("[SetAircraft] Triggered");

				if (!GameManager.IsLocalPlayer(__instance)) return;

				try
				{
					//RITA.Log.LogDebug($"[SetAircraft]: {__instance}, {__instance.HQ}, {aircraft}");
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
				//RITA.Log.LogDebug("[RemoveAircraft] Triggered");

				if (!GameManager.IsLocalPlayer(__instance)) return;
				currentFaction = null;
			}
		}
	}
}
