using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfWalkState : WolfBaseState
{
    public override void EnterState(WolfStateManager wolf)
    {
        Debug.Log("Wolf is Walking");
        //wolf.animator.Play("Walk");
        PlayAnimation(wolf);


    }

    public override void UpdateState(WolfStateManager wolf)
    {
 
    }

    public override void ExitState(WolfStateManager wolf)
    {

    }

    public override void PlayAnimation(WolfStateManager wolf)
    {
        wolf.animator.SetBool("isWalking", true);

        wolf.animator.SetBool("isIdling", false);
        wolf.animator.SetBool("isRunning", false);
        wolf.animator.ResetTrigger("attack1");
    }
}
