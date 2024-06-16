using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashNode : ActionNode
{
    private bool prepare = false;

    private Quaternion targetRotation;
    private float rotationSpeed = 10f;

    protected override void OnStart()
    {
        if (blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            prepare = true;

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (prepare)
        {
            blackboard.animator.SetTrigger("SmashForward");

            Vector3 lookGoal = blackboard.owner.transform.position - GameInstance.Instance.mbPlayer.transform.position;
            lookGoal.y = 0f;
            Quaternion quat = Quaternion.FromToRotation(blackboard.owner.transform.forward, lookGoal);
            targetRotation = quat * blackboard.owner.transform.rotation;
            blackboard.owner.transform.rotation = Quaternion.Slerp(blackboard.owner.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            quat.ToAngleAxis(out float angle, out Vector3 axis);
            GameInstance.Instance.camController.RotateAround(axis, -angle * Time.deltaTime * rotationSpeed);

            prepare = blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
            return State.Running;
        }

        if (blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            return State.Running;
        else
            return State.Success;
    }
}
