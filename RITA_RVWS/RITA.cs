using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using RITA_RVWS.Patches;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RITA_RVWS
{
	[BepInPlugin(RITAInfo.PLUGIN_GUID, RITAInfo.PLUGIN_DISPLAY, RITAInfo.PLUGIN_VERSION)]

	[BepInDependency(RITAInfo.BEPINEX_CONFIG_MANAGER, BepInDependency.DependencyFlags.SoftDependency)]
	[BepInIncompatibility("com.nikkorap.WSOYappinator")]
	[BepInIncompatibility("com.JUSTJ7780.globalsoundreplacerno")]

	public class RITA: BaseUnityPlugin
	{
		internal static RITA Instance { get; private set; } = null!;
		internal static ManualLogSource Log = null!;
		private Harmony _harmony = null!;

		private void Awake()
		{
			Instance = this;

			Log = BepInEx.Logging.Logger.CreateLogSource(RITAInfo.PLUGIN_NAME);

			// Initialize Configuration Settings
			Configuration.InitConfig(Config);
			Configuration.RITAEnabled.SettingChanged += OnRITAToggle;
			Configuration.SetFactionSettingsState(Configuration.RITAEnabled.Value); // Set initial state based on the value RITAEnabled on game launch
			if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(RITAInfo.BEPINEX_CONFIG_MANAGER))
				Log.LogInfo("BepInEx Configuration Manager not installed. Recommend install to access RITA settings in-game.");

			// Load Asset Bundle
			BundleLoader.LoadBundle();

			// Apply all Harmony patches
			_harmony = new Harmony(RITAInfo.PLUGIN_GUID);
			_harmony.PatchAll();
			//Log.LogDebug($"[Awake] Harmony patches applied: {_harmony.GetPatchedMethods().Count()}");

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;

			Log.LogInfo("[Awake] RITA Initialized");
		}

		private void OnDestroy()
		{
			//Log.LogDebug("[OnDestroy] Triggered");

			try
			{
				StopAllCoroutines();
				_harmony.UnpatchSelf();
				//Log.LogDebug("[OnDestroy] Harmony patches unapplied");
			}
			catch (Exception ex)
			{
				Log.LogError(ex);
			}
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			//Log.LogDebug($"[OnSceneLoaded] Triggered");
			PlayerPatch.currentFaction = null;
		}

		private void OnSceneUnloaded(Scene scene)
		{
			//Log.LogDebug($"[OnSceneUnloaded] Triggered");
			StopAllCoroutines();
			PlayerPatch.currentFaction = null;
		}

		private void OnRITAToggle(object sender, EventArgs e)
		{
			Configuration.SetFactionSettingsState(Configuration.RITAEnabled.Value);
			//Log.LogDebug($"[OnRITAToggle] RITA {(Configuration.RITAEnabled.Value ? "enabled" : "disabled")}");
		}

		internal static bool CheckFaction()
		{
			//Log.LogDebug("[CheckFaction] Triggered");

			string? currentFaction = PlayerPatch.currentFaction;
			//Log.LogDebug($"[CheckFaction] Current faction: {currentFaction}");

			if (currentFaction == null)
			{
				//Log.LogDebug("[CheckFaction] Skipped: currentFaction is null — aircraft not yet selected");
				return false;
			}

			// Check for official factions
			if (Configuration.OfficialFactions.TryGetValue(currentFaction, out ConfigEntry<bool> factionEntry))
			{
				//Log.LogDebug($"[CheckFaction] Official faction matched: {currentFaction}, Enabled: {factionEntry.Value}");
				return factionEntry.Value;
			}

			// Check for custom factions
			if (Configuration.RITAFactionCustom.Value)
			{
				//Log.LogDebug($"[CheckFaction] Custom faction matched: {currentFaction}");
				return true;
			}

			//Log.LogDebug($"[CheckFaction] No faction matched: {currentFaction}");
			return false;
		}

		// Restore the original AudioClip on an AudioSource after the replacement clip finishes playing.
		internal IEnumerator RestoreClipAfterPlay(AudioSource source, AudioClip noClip, AudioClip ritaClip)
		{
			//Log.LogDebug($"[RestoreClipAfterPlay] Waiting {ritaClip.length}s before restoring {noClip.name}, Loop: {source.loop}");

			// Wait until the clip has finished playing before restoring
			yield return new WaitForSeconds(ritaClip.length);

			if (source == null || !source.gameObject.activeInHierarchy)
			{
				//Log.LogDebug($"[RestoreClipAfterPlay] Source destroyed, cannot restore {noClip.name}");
				yield break;
			}

			if (source.clip != ritaClip)
			{
				source.clip = noClip;
				yield break;
			}

			bool wasLooping = source.loop;

			source.loop = wasLooping; // Preserve original loop setting
			source.clip = noClip;

			// Resume looping with original clip
			if (wasLooping)
			{
				source.Play();
				//Log.LogDebug($"[RestoreClipAfterPlay] Restored and resumed loop: {noClip.name}");
			}
			//else Log.LogDebug($"[RestoreClipAfterPlay] Restored: {noClip.name}");
		}

		internal static void LogClips()
		{
			AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
			foreach (var clip in clips)
			{
				Log.LogDebug($"[LogClips] Found AudioClip in Resources: {clip.name}");
			}
		}
	}
}
