using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashNode : ActionNode
{
    public AudioClip clip;

    private bool prepare = false;

    private Quaternion targetRotation;

    protected override void OnStart()
    {
        blackboard.audioSource.clip = clip;
        blackboard.audioSource.Play();

        if (blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            prepare = true;

        Vector3 player = GameInstance.Instance.mbPlayer.transform.position;
        player.y = blackboard.owner.transform.position.y;
        blackboard.owner.transform.LookAt(player);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (prepare)
        {
            blackboard.animator.SetTrigger("SmashForward");

            prepare = blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
            return State.Running;
        }

        if (blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            return State.Running;
        else
            return State.Success;
    }
}
