using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SprintNode : ActionNode
{
    public float sprintPower = 10f;

    Animator animator;
    Rigidbody rb;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 player = GameInstance.Instance.mbPlayer.transform.position;
        player.y = blackboard.owner.transform.position.y;
        blackboard.owner.transform.LookAt(player);
        animator.SetTrigger("SprintForward");
        rb.AddForce(blackboard.owner.transform.forward * sprintPower, ForceMode.VelocityChange);

        return State.Running;
    }
}
