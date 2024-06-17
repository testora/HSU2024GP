using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    public MonoBehaviour mbCamera;
    public MonoBehaviour mbPlayer;
    public MonoBehaviour mbBrother;
    public CameraController camController;
    AudioSource audiosource;

    public List<MonoBehaviour> mbMonsters = new List<MonoBehaviour>();

    static public float minPitch = -90f;
    static public float maxPitch = 90f;
    static public float minYaw = -90f;
    static public float maxYaw = 90f;

    static public float minRadPitch = -90f * Mathf.Deg2Rad;
    static public float maxRadPitch = 90f * Mathf.Deg2Rad;
    static public float minRadYaw = -90f * Mathf.Deg2Rad;
    static public float maxRadYaw = 90f * Mathf.Deg2Rad;

    bool mute = false;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.Play();
    }

    private void Update()
    {
        if (mute && audiosource.volume > 0f)
        {
            audiosource.volume -= Time.deltaTime;
            audiosource.volume = Mathf.Clamp(audiosource.volume, 0f, 1f);
        }
    }

    public void Mute()
    {
        mute = true;
    }
}
