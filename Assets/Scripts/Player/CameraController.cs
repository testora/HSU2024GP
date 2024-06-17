using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public Transform parent;
    public float sensitivity = 500f;
    public LayerMask cameraCollision;

    public bool hideCursor = true;

    private float minPitch = -90f;
    private float maxPitch = 90f;
    private float curPitch = 0f;
    private float pivotDistance = 0f;

    float timeAcc = 0f;
    bool recoil = false;

    void Awake()
    {
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        pivotDistance = (parent.position - transform.position).magnitude;
    }

    private void Update()
    {
        if (recoil && timeAcc < 0.25f)
            timeAcc += Time.deltaTime;
        if (timeAcc >= 0.25f)
            recoil = false;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.U))
            Recoil();

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (recoil)
            mouseDelta.y += EasingFunction.EaseInSine(0.5f, -0.5f, Utility.ProportionalRatio(timeAcc, 0f, 0.25f));

        float tilt = -mouseDelta.y * sensitivity * Time.deltaTime;
        float pitch = Mathf.Clamp(curPitch + tilt, minPitch, maxPitch);
        tilt = pitch - curPitch;
        curPitch = pitch;

        transform.RotateAround(parent.position, Vector3.up, mouseDelta.x * sensitivity * Time.deltaTime);
        transform.RotateAround(parent.position, transform.right, tilt);

        Vector3 rayDir = (transform.position - parent.position).normalized;
        if (UnityEngine.Physics.Raycast(parent.position, rayDir, out var hit, pivotDistance, cameraCollision))
            transform.position = hit.point - rayDir * .1f;
        else
            transform.position = parent.position + rayDir * pivotDistance;
    }

    public void RotateAround(Vector3 axis, float angle)
    {
        transform.RotateAround(parent.position, axis, angle);
    }

    public void Recoil()
    {
    //  StartCoroutine(Recoil(.5f));
        timeAcc = 0f;
        recoil = true;
    }

    IEnumerator Recoil(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            RotateAround(transform.right, Time.deltaTime * EasingFunction.EaseInSine(-100f, 200f, Utility.ProportionalRatio(Time.time, endTime - duration, endTime)));
            yield return null;
        }
    }
}
