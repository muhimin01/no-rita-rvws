// Loads RITA's asset bundle
using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RITA_RVWS
{
	internal static class BundleLoader
	{
		internal static void LoadBundle()
		{
			string assetBundle = "rita_rvws.assetbundle";
			string bundlePath = Path.Combine(Paths.PluginPath, RITAInfo.PLUGIN_NAME, assetBundle);

			if (!File.Exists(bundlePath))
			{
				RITA.Log.LogError($"[LoadBundle] {assetBundle} not found at path: {bundlePath}");
				return;
			}
			try
			{
				AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
				if (bundle == null)
				{
					RITA.Log.LogError($"[LoadBundle] Failed to load {assetBundle} from path: {bundlePath}");
					return;
				}
				//RITA.Log.LogDebug($"[LoadBundle] {assetBundle} loaded");
				LoadAssets(bundle);
			}
			catch (Exception ex)
			{
				RITA.Log.LogError($"[LoadBundle] Exception while loading {assetBundle}: {ex.Message}");
				return;
			}
		}

		internal static void LoadAssets(AssetBundle bundle)
		{
			//RITA.Log.LogDebug("[LoadAssets] Loading assets...");

			// Load AudioClips from AssetBundle
			Dictionary<string, AudioClip> bundleClips = new Dictionary<string, AudioClip>();
			foreach (AudioClip clip in bundle.LoadAllAssets<AudioClip>())
			{
				if (clip.loadState != AudioDataLoadState.Loaded)
				{
					clip.LoadAudioData();
					//RITA.Log.LogDebug($"[LoadAssets] Loading: {clip.name}, State: {clip.loadState}");
				}

				bundleClips[clip.name] = clip;
			}

			// Map AudioClips
			foreach (var clip in AudioMap.ClipMap)
			{
				string noClip = clip.Key;
				string ritaClip = clip.Value;

				if (bundleClips.TryGetValue(ritaClip, out AudioClip replacement))
				{
					AudioMap._ritaClips[noClip] = replacement;
					//RITA.Log.LogDebug($"[LoadAssets] Mapped: {noClip} => {ritaClip}");
				}
				else
				{
					RITA.Log.LogError($"[LoadAssets] {ritaClip} not found, (expected for {noClip})");
				}
			}

			// Unload AssetBundle from memory
			bundle.Unload(false);

			//RITA.Log.LogDebug($"[LoadAssets] Replacements count after load: {AudioMap._ritaClips.Count}");
		}
	}
}
