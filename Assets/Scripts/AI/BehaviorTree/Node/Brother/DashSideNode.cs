using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSideNode : ActionNode
{
    public float dashForce = 20f;
    public float disdiv = 2.5f;

    private bool prepare = false;

    protected override void OnStart()
    {
        if (blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            prepare = true;

        Vector3 player = GameInstance.Instance.mbPlayer.transform.position;
        player.y = blackboard.owner.transform.position.y;
        blackboard.owner.transform.LookAt(player);

        bool direction = Random.Range(0, 1) % 2 == 0;
        float distance = (player - blackboard.owner.transform.position).magnitude;
        float theta = Mathf.Acos(Mathf.Clamp(distance / disdiv, -1f, 1f));
        blackboard.owner.transform.Rotate(Vector3.up, theta - Mathf.PI * (direction ? .5f : -.5f));
        blackboard.rb.AddForce(blackboard.owner.transform.right * dashForce * (direction ? .5f : -.5f), ForceMode.VelocityChange);

        blackboard.animator.SetTrigger(direction ? "DashRight" : "DashLeft");
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
