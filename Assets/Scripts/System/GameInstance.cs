using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    public MonoBehaviour mbCamera;
    public MonoBehaviour mbPlayer;
    public CameraController camController;

    static public float minPitch = -90f;
    static public float maxPitch = 90f;
    static public float minYaw = -90f;
    static public float maxYaw = 90f;

    static public float minRadPitch = -90f * Mathf.Deg2Rad;
    static public float maxRadPitch = 90f * Mathf.Deg2Rad;
    static public float minRadYaw = -90f * Mathf.Deg2Rad;
    static public float maxRadYaw = 90f * Mathf.Deg2Rad;

    private void Start()
    {

    }
}
