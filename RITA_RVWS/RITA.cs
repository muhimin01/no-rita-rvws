using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;

namespace RITA_RVWS
{
	[BepInPlugin("com.betanonymous.rita_rvws", "RITA: Russian Voice Warning System", "1.1.0")]
	public class RITA: BaseUnityPlugin
	{
		internal static new ManualLogSource Log;
		private Harmony _harmony;
		
		internal static bool RITADevMode = true;

		private void Awake()
		{
			Log = Logger;

			// Initialize Configuration Settings
			Configuration.InitConfig(Config);
			Config.SettingChanged += OnSettingChanged;

			/*
			// Load Asset Bundle
			AssetBundle? bundle = LoadBundle();
			if (bundle == null) return;
			Logger.LogDebug($"{bundle.name} loaded");

			foreach (var clip in bundle.LoadAllAssets<AudioClip>())
			{
				Logger.LogDebug($"Found AudioClip in {bundle.name}: {clip.name}");
			}
			*/

			// Patch all patches in RITA_RVWS.Patches
			_harmony = new Harmony("com.betanonymous.rita_rvws");
			_harmony.PatchAll();

            Log.LogInfo("RITA initialized");
			LogClips();
		}

		private void OnDestroy()
		{	try
			{
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
			Log.LogDebug($"RITA {(Configuration.RITAEnabled.Value ? "enabled" : "disabled")}");
		}

		internal static void LogClips()
		{

			AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
			foreach (var clip in clips)
			{
				Log.LogDebug($"Found AudioClip in Resources: {clip.name}");
			}
		}
	}
}
