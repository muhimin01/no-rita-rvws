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
				Log.LogDebug($"{clip.name} loaded from {bundle.name}");
			}

			foreach (var clip in AudioMap.clipMap)
			{
				string noClip = clip.Key;
				string ritaClip = clip.Value;

				if (bundleClips.TryGetValue(ritaClip, out AudioClip replacement))
				{
					AudioMap._ritaClips[noClip] = replacement;
					Log.LogDebug($"Mapped: {noClip} => {ritaClip}");
				}
				else Log.LogWarning($"Bundle clip not found: {ritaClip} (expected for game clip: {noClip})");

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
				PatchAudio(false);
				_harmony.UnpatchSelf();
				Log.LogDebug("Harmony patches unapplied");
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
			Log.LogDebug($"RITA {(RITAEnabled ? "enabled" : "disabled")}");
			PatchAudio(RITAEnabled);
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			AudioMap._noClips.Clear();
			PatchAudio(Configuration.RITAEnabled.Value);
		}

		internal static void LogClips()
		{

			AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
			foreach (var clip in clips)
			{
				Log.LogDebug($"[LogClips]: Found AudioClip in Resources: {clip.name}");
			}
		}

		internal static void PatchAudio(bool pass)
		{
			Log.LogDebug($"[PatchAudio] Pass: {pass}");
			if (pass)
			{
				/*
				foreach ((AudioSource source, AudioClip clip) in AudioMap._noClips)
				{
					Log.LogDebug("[PatchAudio] _noClips");
					Log.LogDebug($"{source}, {clip}");
				}
				foreach ((string ritaClip, AudioClip clip) in AudioMap._ritaClips)
				{
                    Log.LogDebug("[PatchAudio] _ritaClips");
                    Log.LogDebug($"{ritaClip}, {clip}");
                }

				// Find all AudioSources in the scene
				foreach (AudioSource source in FindObjectsOfType<AudioSource>())
				{
					if (source.clip == null) continue;
					Log.LogDebug(source.clip.name);
					if (!AudioMap._ritaClips.TryGetValue(source.clip.name, out AudioClip ritaClip)) continue;

					// Only store original clips once - avoid overwriting if already patched
					if (!AudioMap._noClips.ContainsKey(source)) AudioMap._noClips[source] = source.clip;

					source.clip = ritaClip;
					Log.LogDebug("[PatchAudio]: AudioClips patched");
				}
				*/

				AudioSource[] allSources = Resources.FindObjectsOfTypeAll<AudioSource>();
                Log.LogDebug($"[PatchAdio] Found {allSources.Length} AudioSources (including inactive).");

                foreach (AudioSource source in allSources)
                {
                    // Check 1 - does the AudioSource have a clip at all?
                    if (source.clip == null)
                    {
                        Log.LogDebug($"[PatchAdio] {source.gameObject.name} has no clip, skipping.");
                        continue;
                    }

                    Log.LogDebug($"[PatchAdio] AudioSource: {source.gameObject.name}, Clip: {source.clip.name}");

                    // Check 2 - does the clip name exist in Replacements?
                    if (!AudioMap._ritaClips.TryGetValue(source.clip.name, out AudioClip replacement))
                    {
                        Log.LogDebug($"[PatchAdio] No replacement found for: {source.clip.name}");
                        continue;
                    }

                    if (!AudioMap._noClips.ContainsKey(source))
                        AudioMap._noClips[source] = source.clip;

                    source.clip = replacement;
                    Log.LogDebug($"[PatchAdio] Swapped: {source.clip.name} → {replacement.name}");
                }

                Log.LogDebug($"[PatchAdio] OriginalSources count after swap: {AudioMap._noClips.Count}");

            }
			else
			{
				// Restore original clips
				foreach (var noClip in AudioMap._noClips)
				{
					AudioSource source = noClip.Key;
					AudioClip clip = noClip.Value;

					// Guard against destroyed AudioSources
					if (source != null) source.clip = clip;
				}

				AudioMap._noClips.Clear();
				Log.LogDebug("[PatchAudio]: AudioClips restored");
			}
		}
	}
}
