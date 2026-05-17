// Stores and maps audio files
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RITA_RVWS
{
    internal static class AudioMap
    {
        internal static Dictionary<AudioSource, AudioClip> _noClips = new Dictionary<AudioSource, AudioClip>();
        internal static Dictionary<string, AudioClip> _ritaClips = new Dictionary<string, AudioClip>();
        
        internal static readonly Dictionary<string, string> clipMap = new Dictionary<string, string>
        {
            /*
            { "eject1", "actEject" },
            { "raisegear1", "warnGear" },
            { "lowerGearJennifer", "warnGearLower" },
            { "stall1", "critAOA" },
            { "VRSJennifer", "warnVRS" },
            { "overSpeedJennifer", "critSpeed" },
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
            */
            { "eject1", "eject1" },
            { "raisegear1", "raisegear1" },
            { "lowerGearJennifer", "lowerGearJennifer" },
            { "stall1", "stall1" },
            { "VRSJennifer", "VRSJennifer" },
            { "overSpeedJennifer", "overSpeedJennifer" },
            { "engineFire", "engineFire" },
            { "engine1Fire", "engine1Fire" },
            { "engine2Fire", "engine2Fire" },
            { "engine3Fire", "engine3Fire" },
            { "engine4Fire", "engine4Fire" },
            { "mainRotorDamageJennifer", "mainRotorDamageJennifer" },
            { "tailRotorFailureJennifer", "tailRotorFailureJennifer" },
            { "frontLeftEngineFailureJennifer", "frontLeftEngineFailureJennifer" },
            { "frontRightEngineFailureJennifer", "frontRightEngineFailureJennifer" },
            { "rearLeftEngineFailureJennifer", "rearLeftEngineFailureJennifer" },
            { "reaRightEngineFailureJennifer", "reaRightEngineFailureJennifer" },
            { "leftEngineFailureJennifer", "leftEngineFailureJennifer" },
            { "leftEngineFire", "leftEngineFire" },
            { "leftFanFailureJennifer", "leftPropStrike" },
            { "leftPropStrike", "leftPropStrike" },
            { "rightEngineFailureJennifer", "rightEngineFailureJennifer" },
            { "rightEngineFire", "rightEngineFire" },
            { "rightFanFailureJennifer", "rightFanFailureJennifer" },
            { "rightPropStrike", "rightPropStrike" },
        };

        internal static void Init()
        {
            _noClips.Clear();
            _ritaClips.Clear();
        }
    }
}
