using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SprintNode : ActionNode
{
    public float sprintPower = 10f;


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
        blackboard.animator.SetTrigger("SprintForward");
        blackboard.rb.AddForce(blackboard.owner.transform.forward * sprintPower, ForceMode.Acceleration);

        return State.Running;
    }
}
