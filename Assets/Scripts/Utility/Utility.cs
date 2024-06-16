using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI;
public class Utility 
{
    internal static float ProportionalRatio(float value, float min, float max)
    {
        if (min == max) return 0f;
        return (value - min) / (max - min);
    }
}
