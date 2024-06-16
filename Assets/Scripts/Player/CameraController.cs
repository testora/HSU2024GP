using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    void Awake()
    {
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        pivotDistance = (parent.position - transform.position).magnitude;
    }

    void LateUpdate()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

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
}
