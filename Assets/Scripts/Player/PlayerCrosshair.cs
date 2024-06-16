using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum RG_CROSSHAIR
{
    MAIN = 1 << 0,
    SCOPE = 1 << 1,
    SUPER_CHARGE = 1 << 2,
    SUPER_READY = 1 << 3,
    SUPER_REBOOT = 1 << 4,
    RELOAD = 1 << 5,
    SPRINT = 1 << 6,
    MAX = 1 << 7
}

public class PlayerCrosshair : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
