using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static NuclearOption.Chat.AllowBanListTabs;

namespace RITA_RVWS
{
	[BepInPlugin("com.betanonymous.rita_rvws", "RITA: Russian Voice Warning System", "1.1.0")]
	public class RITA: BaseUnityPlugin
	{
		internal static RITA Instance { get; private set; }
		internal static new ManualLogSource Log;
		private Harmony _harmony;
		
		internal static bool RITADevMode = false;

		private void Awake()
		{
			Log = Logger;

			// Initialize Configuration Settings
			Configuration.InitConfig(Config);
			Config.SettingChanged += OnSettingChanged;

			// Load Asset Bundle
			AssetBundle? bundle = BundleLoader.LoadBundle();
			if (bundle == null) return;
			Log.LogDebug($"{bundle.name} loaded");

			Dictionary<string, AudioClip> bundleClips = new Dictionary<string, AudioClip>();
			foreach (AudioClip clip in bundle.LoadAllAssets<AudioClip>())
			{
				bundleClips[clip.name] = clip; 
				Log.LogDebug($"[Awake] Loaded: {clip.name} from {bundle.name}");
			}

			foreach (var clip in AudioMap.clipMap)
			{
				string noClip = clip.Key;
				string ritaClip = clip.Value;

				if (bundleClips.TryGetValue(ritaClip, out AudioClip replacement))
				{
					AudioMap._ritaClips[noClip] = replacement;
					Log.LogDebug($"[Awake] Mapped: {noClip} => {ritaClip}");
				}
				else Log.LogWarning($"[Awake] Clip not found: {ritaClip} (expected for game clip: {noClip})");

            }

			bundle.Unload(false);

			// Patch all patches in RITA_RVWS.Patches
			_harmony = new Harmony("com.betanonymous.rita_rvws");
			_harmony.PatchAll();

            Log.LogInfo("RITA initialized");
			if (RITADevMode) LogClips();
		}

		private void OnDestroy()
		{	try
			{
				RestoreClips();
				_harmony.UnpatchSelf();
				Log.LogDebug("[OnDestroy] Harmony patches unapplied");
			}
			catch(Exception ex)
			{
				Log.LogError(ex);
			}
		}

		private void OnSettingChanged(object sender, EventArgs e)
		{
			if (RITADevMode)
			{
				Log.LogDebug(sender);
				Dev.RITADebug.LogObjectContents(sender);
				Log.LogDebug(e);
				Dev.RITADebug.LogObjectContents(e);
			}
			
			bool RITAEnabled = Configuration.RITAEnabled.Value;
			Log.LogDebug($"[OnSettingChanged] RITA {(RITAEnabled ? "enabled" : "disabled")}");

			if (!RITAEnabled) RestoreClips();
		}

		internal static void RestoreClips()
		{
			foreach (var clip in AudioMap._noClips)
			{
				AudioSource source = clip.Key;
				AudioClip noClip = clip.Value;

				if (source != null)
				{
					source.clip = noClip;
					Log.LogDebug($"[RestoreClips] Restored: {noClip.name}");
				}
			}
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
