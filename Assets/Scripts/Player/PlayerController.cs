using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

[Flags]
public enum DIRECTION
{
    FORWARD = 1 << 0,  // 1
    BACKWARD = 1 << 1, // 2
    LEFT = 1 << 2,     // 4
    RIGHT = 1 << 3,    // 8
    MAX = 1 << 4       // 16
}

public static class Direction
{
    public const uint BITMASK_FORWARD = (uint)DIRECTION.FORWARD;
    public const uint BITMASK_BACKWARD = (uint)DIRECTION.BACKWARD;
    public const uint BITMASK_LEFT = (uint)DIRECTION.LEFT;
    public const uint BITMASK_RIGHT = (uint)DIRECTION.RIGHT;

    public const uint BITMASK_FORWARDLEFT = (uint)DIRECTION.FORWARD | (uint)DIRECTION.LEFT;
    public const uint BITMASK_FORWARDRIGHT = (uint)DIRECTION.FORWARD | (uint)DIRECTION.RIGHT;
    public const uint BITMASK_BACKWARDLEFT = (uint)DIRECTION.BACKWARD | (uint)DIRECTION.LEFT;
    public const uint BITMASK_BACKWARDRIGHT = (uint)DIRECTION.BACKWARD | (uint)DIRECTION.RIGHT;
}

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 100f;
    public float jumpPower = 100f;

    private uint bitDirection = 0;
    private PlayerState playerState;

    Rigidbody rb;
    Animator animator;
    AnimatorStateInfo aimPitchState;
    AnimatorStateInfo aimYawState;

    private Quaternion targetRotation;
    private float rotationSpeed = 10f;

    void Start()
    {
        targetRotation = transform.rotation;

        playerState = GetComponent<PlayerState>();

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        aimPitchState = GetComponent<Animator>().GetNextAnimatorStateInfo(1);
        aimYawState = GetComponent<Animator>().GetNextAnimatorStateInfo(2);

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.inertiaTensor = rb.inertiaTensor;
        rb.inertiaTensorRotation = rb.inertiaTensorRotation;
    }

    void Update()
    {
        //  float moveHorizontal = Input.GetAxis("Horizontal");
        //  float moveVertical = Input.GetAxis("Vertical");

        Handle_Bitset();
        Handle_MouseInput();
        Handle_KeyInput();

//        if (Input.GetKeyDown(KeyCode.A))
//        {
//
//            Debug.Log("0 " + transform.forward);
//            Debug.Log("1 " + transform.position);
//            Debug.Log("2 " + GameInstance.Instance.mbCamera.transform.forward);
//                transform.LookAt(transform.position + GameInstance.Instance.mbCamera.transform.forward);
//         //   transform.LookAt(transform.position - GameInstance.Instance.mbCamera.transform.right);
//            Debug.Log("3 " + transform.forward);
//            Debug.Log("4 " + transform.up);
//            transform.up = Vector3.up;
//            Debug.Log("5 " + transform.forward);
//        }
    }

    void Handle_Bitset()
    {
        if (Input.GetKeyDown(KeyCode.W))
            SetDirection(Direction.BITMASK_FORWARD);
        else if (Input.GetKeyUp(KeyCode.W))
            SetDirection(Direction.BITMASK_FORWARD, false);
        if (Input.GetKeyDown(KeyCode.A))
            SetDirection(Direction.BITMASK_LEFT);
        else if (Input.GetKeyUp(KeyCode.A))
            SetDirection(Direction.BITMASK_LEFT, false);
        if (Input.GetKeyDown(KeyCode.S))
            SetDirection(Direction.BITMASK_BACKWARD);
        else if (Input.GetKeyUp(KeyCode.S))
            SetDirection(Direction.BITMASK_BACKWARD, false);
        if (Input.GetKeyDown(KeyCode.D))
            SetDirection(Direction.BITMASK_RIGHT);
        else if (Input.GetKeyUp(KeyCode.D))
            SetDirection(Direction.BITMASK_RIGHT, false);
    }

    void Handle_MouseInput()
    {
        Vector3 camLook = GameInstance.Instance.mbCamera.transform.forward.normalized;
        float pitch = Mathf.Atan2(camLook.y, Mathf.Sqrt(Mathf.Pow(camLook.x, 2f) + Mathf.Pow(camLook.z, 2f)));
        float yaw = 0.5f * (Vector3.Dot(transform.right.normalized, camLook) + 1f);
        pitch = Mathf.Clamp(Utility.ProportionalRatio(pitch, GameInstance.minRadPitch * .5f, GameInstance.maxRadPitch * .5f), 0f, 1f);

        animator.SetLayerWeight(1, 2f * Mathf.Abs(0.5f - pitch));
        animator.SetLayerWeight(2, 2f * Mathf.Abs(0.5f - yaw));
        animator.Play(aimPitchState.fullPathHash, 1, pitch);
        animator.Play(aimYawState.fullPathHash, 2, yaw);
    }

    void Handle_KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerState.SetState(RG_STATE.AIR);
            rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        }

        bool isAir = playerState.IsState(RG_STATE.AIR);
        bool isAim = playerState.IsState(RG_STATE.AIM);
        bool isSprint = playerState.IsState(RG_STATE.SPRINT);

        switch (bitDirection)
        {
            case Direction.BITMASK_FORWARD:
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    playerState.SetState(RG_STATE.SPRINT);

                Vector3 look = GameInstance.Instance.mbCamera.transform.forward;
                look.y = 0f;
                Quaternion quat = Quaternion.FromToRotation(transform.forward, look);
                targetRotation = quat * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                quat.ToAngleAxis(out float angle, out Vector3 axis);
                GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

                rb.AddForce(moveSpeed * transform.forward * (isSprint ? 1.5f : 1f), ForceMode.Acceleration);

                if (!isAir)
                    animator.SetTrigger(isSprint ? "SprintForward" : "RunForward");
            }
            break;
            case Direction.BITMASK_BACKWARD:
            {
                Vector3 look = GameInstance.Instance.mbCamera.transform.forward * (isAim ? 1f : -1f);
                look.y = 0f;
                Quaternion quat = Quaternion.FromToRotation(transform.forward, look);
                targetRotation = quat * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                quat.ToAngleAxis(out float angle, out Vector3 axis);
                GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

                rb.AddForce(moveSpeed * transform.forward * (isAim ? 1f : -1f), ForceMode.Acceleration);

                if (!isAir)
                    animator.SetTrigger(isAim ? "RunBackward" : "RunForward");
            }
            break;
            case Direction.BITMASK_LEFT:
            {
                Vector3 look = (isAim ? GameInstance.Instance.mbCamera.transform.forward : -GameInstance.Instance.mbCamera.transform.right);
                look.y = 0f;
                Quaternion quat = Quaternion.FromToRotation(transform.forward, look);
                targetRotation = quat * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                quat.ToAngleAxis(out float angle, out Vector3 axis);
                GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

                rb.AddForce(moveSpeed * (isAim ? -transform.right : transform.forward) * (isSprint ? 1.5f : 1f));

                if (!isAir)
                    animator.SetTrigger(isSprint ? "SprintLeft" : isAim ? "RunLeft" : "RunForward");
            }
            break;
            case Direction.BITMASK_RIGHT:
            {
                Vector3 look = (isAim ? GameInstance.Instance.mbCamera.transform.forward : GameInstance.Instance.mbCamera.transform.right);
                look.y = 0f;
                Quaternion quat = Quaternion.FromToRotation(transform.forward, look);
                targetRotation = quat * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                quat.ToAngleAxis(out float angle, out Vector3 axis);
                GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

                rb.AddForce(moveSpeed * (isAim ? transform.right : transform.forward) * (isSprint ? 1.5f : 1f));

                if (!isAir)
                    animator.SetTrigger(isSprint ? "SprintRight" : isAim ? "RunRight" : "RunForward");
            }
            break;
            case Direction.BITMASK_FORWARDLEFT:
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    playerState.SetState(RG_STATE.SPRINT);

                Vector3 dir = (GameInstance.Instance.mbCamera.transform.forward - GameInstance.Instance.mbCamera.transform.right).normalized;
                Vector3 look = (isAim ? GameInstance.Instance.mbCamera.transform.forward : dir);
                look.y = 0f;
                Quaternion quat = Quaternion.FromToRotation(transform.forward, look);
                targetRotation = quat * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                quat.ToAngleAxis(out float angle, out Vector3 axis);
                GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

                rb.AddForce(moveSpeed * (isAim ? dir : transform.forward) * (isSprint ? 1.5f : 1f));

                if (!isAir)
                    animator.SetTrigger(isSprint ? "SprintLeft" : isAim ? "RunLeft" : "RunForward");
            }
            break;
            case Direction.BITMASK_FORWARDRIGHT:
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    playerState.SetState(RG_STATE.SPRINT);

                Vector3 dir = (GameInstance.Instance.mbCamera.transform.forward + GameInstance.Instance.mbCamera.transform.right).normalized;
                Vector3 look = (isAim ? GameInstance.Instance.mbCamera.transform.forward : dir);
                look.y = 0f;
                Quaternion quat = Quaternion.FromToRotation(transform.forward, look);
                targetRotation = quat * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                quat.ToAngleAxis(out float angle, out Vector3 axis);
                GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

                rb.AddForce(moveSpeed * (isAim ? dir : transform.forward) * (isSprint ? 1.5f : 1f));

                if (!isAir)
                    animator.SetTrigger(isSprint ? "SprintRight" : isAim ? "RunRight" : "RunForward");
            }
            break;
            case Direction.BITMASK_BACKWARDLEFT:
            {
                Vector3 dir = (-GameInstance.Instance.mbCamera.transform.forward - GameInstance.Instance.mbCamera.transform.right).normalized;
                Vector3 look = (isAim ? GameInstance.Instance.mbCamera.transform.forward : dir);
                look.y = 0f;
                Quaternion quat = Quaternion.FromToRotation(transform.forward, look);
                targetRotation = quat * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                quat.ToAngleAxis(out float angle, out Vector3 axis);
                GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

                rb.AddForce(moveSpeed * (isAim ? dir : transform.forward));

                if (!isAir)
                    animator.SetTrigger(isSprint ? "SprintLeft" : isAim ? "RunLeft" : "RunForward");
            }
            break;
            case Direction.BITMASK_BACKWARDRIGHT:
            {
                Vector3 dir = (-GameInstance.Instance.mbCamera.transform.forward + GameInstance.Instance.mbCamera.transform.right).normalized;
                Vector3 look = (isAim ? GameInstance.Instance.mbCamera.transform.forward : dir);
                look.y = 0f;
                Quaternion quat = Quaternion.FromToRotation(transform.forward, look);
                targetRotation = quat * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                quat.ToAngleAxis(out float angle, out Vector3 axis);
                GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

                rb.AddForce(moveSpeed * (isAim ? dir : transform.forward));

                if (!isAir)
                    animator.SetTrigger(isSprint ? "SprintRight" : isAim ? "RunRight" : "RunForward");
            }
            break;
            default:
            {
                if (!isAir)
                    animator.SetTrigger("Idle");
            }
            break;
        }
    }

    public void SetDirection(uint bitmask, bool value = true)
    {
        if (value)
            bitDirection |= bitmask;
        else
            bitDirection &= ~bitmask;
    }

    public bool IsDirection(uint direction)
    {
        return (bitDirection & direction) != 0;
    }
}
