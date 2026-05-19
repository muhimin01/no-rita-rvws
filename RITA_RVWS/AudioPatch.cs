using HarmonyLib;
using System;
using UnityEngine;

namespace RITA_RVWS.Patches
{
    internal class AudioPatch
    {
        [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), new Type[] { })]
        internal static class PatchAudioSourcePlay
        {
            private static void Prefix(AudioSource __instance, out AudioClip __state)
            {
                RITA.Log.LogDebug($"[AudioSourcePlay] Triggered");

                __state = __instance.clip; // Store original in state to pass to Postfix

                if (__instance.clip == null)
                {
                    RITA.Log.LogDebug($"[AudioSourcePlay] Skipped: clip is null");
                    return;
                }

                RITA.Log.LogDebug($"[AudioSourcePlay] Clip: {__instance.clip.name}");

                if (!Configuration.RITAEnabled.Value)
                {
                    RITA.Log.LogDebug($"[AudioSourcePlay] Skipped: Mod is disabled");
                    return;
                }

                if (!RITA.CheckFaction())
                {
                    RITA.Log.LogDebug($"[AudioSourcePlay] Skipped: RITA not enabled for current player faction");
                    return;
                }

                if (!AudioMap._ritaClips.TryGetValue(__instance.clip.name, out AudioClip ritaClip))
                {
                    RITA.Log.LogDebug($"[AudioSourcePlay] Skipped: No replacement found for {__instance.clip.name}");
                    return;
                }

                RITA.Log.LogDebug($"[AudioSourcePlay] Replacing: {__instance.clip.name} => {ritaClip.name}");
                __instance.clip = ritaClip;
            }
            private static void Postfix(AudioSource __instance, AudioClip __state)
            {
                if (__state == null) return;
                if (__instance.clip == __state) return;
                if (RITA.Instance == null)
                {
                    RITA.Log.LogDebug($"[AudioSourcePlay] Postfix: RITA.Instance is null, cannot start coroutine");
                    return;
                }

                RITA.Instance.StartCoroutine(RITA.Instance.RestoreClipAfterPlay(__instance, __state, __instance.clip));
            }
        }

        [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.PlayOneShot), new Type[] { typeof(AudioClip) })]
        internal static class PatchAudioSourcePlayOneShot
        {
            private static void Prefix(ref AudioClip clip)
            {

                RITA.Log.LogDebug($"[AudioSourcePlayOneShot] Triggered");
                RITA.Log.LogDebug($"[PlayOneShot] Incoming clip: {clip?.name ?? "null"}");

                if (!Configuration.RITAEnabled.Value)
                {
                    RITA.Log.LogDebug($"[AudioSourcePlayOneShot] Skipped: Mod is disabled");

                    return;
                }

                if (!RITA.CheckFaction())
                {
                    RITA.Log.LogDebug($"[AudioSourcePlayOneShot] Skipped: RITA not enabled for current player faction");
                    return;
                }

                if (clip == null) return;

                if (AudioMap._ritaClips.TryGetValue(clip.name, out AudioClip ritaClip))
                {
                    RITA.Log.LogDebug($"[AudioSourcePlayOneShot] Replacing: {clip.name} => {ritaClip.name}");
                    clip = ritaClip;
                }
                else RITA.Log.LogDebug($"[AudioSourcePlayOneShot] No replacement found for {clip.name}");
            }
        }
    }
}
