using HarmonyLib;
using System;
using UnityEngine;

namespace RITA_RVWS.Patches
{
    internal class AudioPatch
    {
        internal static bool RITAEnabled = Configuration.RITAEnabled.Value;

        [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), new Type[] {})]
        internal static class PatchAudioSourcePlay
        {
            private static void Prefix(AudioSource __instance, out AudioClip __state)
            {
                RITA.Log.LogDebug($"[AudioPatch]: Triggered");

                __state = __instance.clip; // Store original in state to pass to Postfix

                if (!RITAEnabled) return;
                if (__instance.clip == null) return;
                if (!AudioMap._ritaClips.TryGetValue(__instance.clip.name, out AudioClip ritaClip)) return;
                if (!AudioMap._noClips.ContainsKey(__instance))
                {
                    AudioMap._noClips[__instance] = __instance.clip;
                    RITA.Log.LogDebug($"[AudioSourcePlay] Stored: {__instance.clip.name}");
                }
                
                __instance.clip = ritaClip;
                RITA.Log.LogDebug($"[AudioSourcePlay] Replaced: {__instance.clip.name} => {ritaClip.name}");
            }
            /*
            private static void Postfix(AudioSource __instance, AudioClip __state)
            {
                // Restore the original clip immediately after Play() so the source isn't permanently modified
                if (__state != null) __instance.clip = __state;
            }
            */
        }

        [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.PlayOneShot), new Type[] { typeof(AudioClip) })]
        internal static class PatchAudioSourcePlayOneShot
        {
            private static void Prefix(ref AudioClip clip)
            {
                if (!RITAEnabled) return;
                if (clip == null) return;

                if (AudioMap._ritaClips.TryGetValue(clip.name, out AudioClip ritaClip))
                {
                    clip = ritaClip;
                    RITA.Log.LogDebug($"[AudioSourcePlayOneShot] Replaced: {clip.name} => {ritaClip.name}");
                }
            }
        }
    }
}
