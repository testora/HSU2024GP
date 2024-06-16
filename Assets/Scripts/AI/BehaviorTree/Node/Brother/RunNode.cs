using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNode : ActionNode
{
    public float runPower = 20f;

    protected override void OnStart()
    {
        Vector3 player = GameInstance.Instance.mbPlayer.transform.position;
        player.y = blackboard.owner.transform.position.y;
        blackboard.owner.transform.LookAt(player);

        blackboard.rb.AddForce(blackboard.owner.transform.forward * runPower, ForceMode.Acceleration);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Running;
    }
}
