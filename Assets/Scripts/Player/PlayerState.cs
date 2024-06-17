using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//change
[Flags]
public enum RG_STATE
{
    SPRINT = 1 << 0,
    AIR = 1 << 1,
    AIM = 1 << 2,
    READY_PISTOL = 1 << 3,
    READY_SNIPER = 1 << 4,
    READY_SUPER = 1 << 5,
    READY_RELOAD = 1 << 6,
    FIRE_PISTOL = 1 << 7,
    FIRE_SNIPER = 1 << 8,
    FIRE_SUPER = 1 << 9,
    MAX = 1 << 10
}

public class PlayerState : MonoBehaviour
{
    private uint bitState = 0;

    Animator animator;
    Rigidbody rb;

    public void SetState(RG_STATE state, bool value = true)
    {
        if (value)
            bitState |= (uint)state;
        else
            bitState &= ~(uint)state;
    }

    public bool IsState(RG_STATE state)
    {
        return (bitState & (uint)state) != 0;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsState(RG_STATE.AIR))
        {
        }

    }

    void OnCollisionEnter(Collision collision)
    {
    //  if (collision.gameObject.layer.Equals(6))
    //  {
    //      SetState(RG_STATE.AIR, false);
    //      animator.SetBool("AirLoopUp", false);
    //  }
    }
    void OnCollisionExit(Collision collision)
    {
    //  if (collision.gameObject.layer.Equals(6))
    //  {
    //      SetState(RG_STATE.AIR);
    //      animator.SetBool("AirLoopUp", true);
    //  }
    }
}
