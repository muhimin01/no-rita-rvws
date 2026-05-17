// Stores and maps audio files
using System.Collections.Generic;
using UnityEngine;

namespace RITA_RVWS
{
    internal static class AudioMap
    {
        //internal static Dictionary<AudioSource, AudioClip> _noClips = new Dictionary<AudioSource, AudioClip>();
        internal static Dictionary<string, AudioClip> _ritaClips = new Dictionary<string, AudioClip>();

        internal static readonly Dictionary<string, string> ClipMap = new Dictionary<string, string>
        {
            { "eject1", "warnEject" },
            { "raisegear1", "warnGear" },
            { "lowerGearJennifer", "warnGearLower" },
            { "stall1", "warnAOA" },
            { "VRSJennifer", "warnVRS" },
            { "overSpeedJennifer", "warnSpeed" },
            { "engineFire", "engineFire" },
            { "engine1Fire", "engineFire1" },
            { "engine2Fire", "engineFire2" },
            { "engine3Fire", "engineFire3" },
            { "engine4Fire", "engineFire4" },
            { "mainRotorDamageJennifer", "rotorFailureMain" },
            { "tailRotorFailureJennifer", "rotorFailureTail" },
            { "frontLeftEngineFailureJennifer", "engineFailureFrontLeft" },
            { "frontRightEngineFailureJennifer", "engineFailureFrontRight" },
            { "rearLeftEngineFailureJennifer", "engineFailureRearLeft" },
            { "reaRightEngineFailureJennifer", "engineFailureRearRight" },
            { "leftEngineFailureJennifer", "engineFailureLeft" },
            { "leftEngineFire", "engineFireLeft" },
            { "leftFanFailureJennifer", "rotorFailureLeft" },
            { "leftPropStrike", "rotorFailureLeft" },
            { "rightEngineFailureJennifer", "engineFailureRight" },
            { "rightEngineFire", "engineFireRight" },
            { "rightFanFailureJennifer", "rotorFailureRight" },
            { "rightPropStrike", "rotorFailureRight" },
        };

        internal static void Init()
        {
            _ritaClips.Clear();
        }
    }
}
