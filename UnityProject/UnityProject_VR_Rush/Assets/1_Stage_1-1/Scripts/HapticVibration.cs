using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HapticVibration : MonoBehaviour
{
    public static HapticVibration instance;
    private void Awake()
    {
        instance = this;
    }

    public SteamVR_Action_Vibration vib;

    public void PlayVibration(SteamVR_Input_Sources hand)
    {
        Pulse(1, 150, 75, hand);
    }

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        vib.Execute(0, duration, frequency, amplitude, source);
    }
}
