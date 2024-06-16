using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintSmash : ActionNode
{
    public AudioClip clip;

    public float sprintForce = 40f;

    private bool prepare = false;

    protected override void OnStart()
    {
        blackboard.audioSource.clip = clip;
        blackboard.audioSource.Play();

        if (blackboard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            prepare = true;

        Vector3 player = GameInstance.Instance.mbPlayer.transform.position;
        player.y = blackboard.owner.transform.position.y;
        blackboard.owner.transform.LookAt(player);

        blackboard.animator.SetTrigger("SprintSmash");
        blackboard.rb.AddForce(blackboard.owner.transform.forward * sprintForce, ForceMode.VelocityChange);
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
