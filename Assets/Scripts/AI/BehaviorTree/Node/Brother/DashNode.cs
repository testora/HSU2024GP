using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashNode : ActionNode
{
    enum DashDirection
    {
        FORWARD, BACKWARD, LEFT, RIGHT
    }

    public float dashForce = 50f;

    public bool forward = false;
    public bool backward = false;
    public bool left = false;
    public bool right = false;

    public AudioClip clip;

    private bool prepare = false;

    List<DashDirection> directionList = new List<DashDirection>();
    DashDirection direction = DashDirection.FORWARD;

    protected override void OnStart()
    {
        blackboard.audioSource.clip = clip;
        blackboard.audioSource.Play();

        if (blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            prepare = true;

        Vector3 player = GameInstance.Instance.mbPlayer.transform.position;
        player.y = blackboard.owner.transform.position.y;
        blackboard.owner.transform.LookAt(player);

        directionList.Clear();
        if (forward) directionList.Add(DashDirection.FORWARD);
        if (backward) directionList.Add(DashDirection.BACKWARD);
        if (left) directionList.Add(DashDirection.LEFT);
        if (right) directionList.Add(DashDirection.RIGHT);
        direction = directionList[Random.Range(0, directionList.Count)];

        switch (direction)
        {
            case DashDirection.FORWARD:
                blackboard.animator.SetTrigger("DashForward");
                blackboard.rb.AddForce(blackboard.owner.transform.forward * dashForce, ForceMode.VelocityChange);
                break;
            case DashDirection.BACKWARD:
                blackboard.animator.SetTrigger("DashBackward");
                blackboard.rb.AddForce(-blackboard.owner.transform.forward * dashForce, ForceMode.VelocityChange);
                break;
            case DashDirection.LEFT:
                blackboard.animator.SetTrigger("DashLeft");
                blackboard.rb.AddForce(-blackboard.owner.transform.right * dashForce, ForceMode.VelocityChange);
                break;
            case DashDirection.RIGHT:
                blackboard.animator.SetTrigger("DashRight");
                blackboard.rb.AddForce(blackboard.owner.transform.right * dashForce, ForceMode.VelocityChange);
                break;
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (prepare)
        {
            prepare = blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
            return State.Running;
        }

        if (blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            return State.Running;
        else
            return State.Success;
    }
}
