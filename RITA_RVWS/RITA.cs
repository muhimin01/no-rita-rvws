using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RITA_RVWS
{
    [BepInPlugin("com.betanonymous.rita_rvws", "RITA: Russian Voice Warning System", "1.1.0")]
    public class RITA: BaseUnityPlugin
    {
        internal static RITA Instance { get; private set; }
        internal static new ManualLogSource Log;
        private Harmony _harmony;

        private void Awake()
        {
            Instance = this;

            Log = BepInEx.Logging.Logger.CreateLogSource("RITA_RVWS");

            // Initialize Configuration Settings
            Configuration.InitConfig(Config);
            Config.SettingChanged += OnSettingChanged;

            // Load Asset Bundle
            BundleLoader.LoadBundle();

            // Apply all Harmony patches
            _harmony = new Harmony("com.betanonymous.rita_rvws");
            _harmony.PatchAll();
            Log.LogDebug($"[Awake] Harmony patches applied: {_harmony.GetPatchedMethods().Count()}");

            Log.LogInfo("[Awake] Initialized");
        }

        private void OnDestroy()
        {
            try
            {
                StopAllCoroutines();
                _harmony.UnpatchSelf();
                Log.LogDebug("[OnDestroy] Harmony patches unapplied");
            }
            catch (Exception ex)
            {
                Log.LogError(ex);
            }
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            // TODO: Implement faction config setting
            
            Log.LogDebug($"[OnSettingChanged] RITA {(Configuration.RITAEnabled.Value ? "enabled" : "disabled")}");
        }

        internal IEnumerator RestoreClipAfterPlay(AudioSource source, AudioClip original, AudioClip replacement)
        {
            Log.LogDebug($"[RestoreClipAfterPlay] Waiting {replacement.length}s before restoring {original.name}, Loop: {source.loop}");

            // Wait until the clip has finished playing before restoring
            yield return new WaitForSeconds(replacement.length);

            if (source == null || !source.gameObject.activeInHierarchy)
            {
                Log.LogDebug($"[RestoreClipAfterPlay] Source destroyed, cannot restore {original.name}");
                yield break;
            }
            
            bool wasLooping = source.loop;

            source.Stop();
            source.loop = wasLooping; // Preserve original loop setting
            source.clip = original;

            if (wasLooping)
            {
                source.Play(); // Resume looping with original clip
                Log.LogDebug($"[RestoreClipAfterPlay] Restored and resumed loop: {original.name}");
            }
            else
            {
                Log.LogDebug($"[RestoreClipAfterPlay] Restored: {original.name}");
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
