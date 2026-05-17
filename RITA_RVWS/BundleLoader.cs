// Loads RITA's asset bundle
using BepInEx;
using System;
using System.IO;
using UnityEngine;

namespace RITA_RVWS
{
    internal static class BundleLoader
    {
        internal static AssetBundle? LoadBundle()
        {
            string pluginName = "RITA_RVWS";
            string assetBundle = "rita_rvws.assetbundle";
            string bundlePath = Path.Combine(Paths.PluginPath, pluginName, assetBundle);
            if (!File.Exists(bundlePath))
            {
                RITA.Log.LogError($"[BundleLoader] {assetBundle} not found at path: {bundlePath}");
                return null;
            }
            try
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
                if (bundle == null)
                {
                    RITA.Log.LogError($"[BundleLoader] Failed to load {assetBundle} from path: {bundlePath}");
                }
                return bundle;
            }
            catch (Exception ex)
            {
                RITA.Log.LogError($"[BundleLoader] Exception while loading {assetBundle}: {ex.Message}");
                return null;
            }
        }
    }
}
