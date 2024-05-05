using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WolfMeleState : WolfBaseState
{
    //private float chaseSpeed = 4f;
    //private float turnSpeed = 200f;

    public override void EnterState(WolfStateManager wolf)
    {
        Debug.Log("Wolf is chasing player");

        PlayAnimation(wolf);
    }

    public override void UpdateState(WolfStateManager wolf)
    {

        //if (wolf.PlayerInMeleRange())
        //{
        //    wolf.SwitchState(wolf.PounceState);
        //}
    }

    public override void ExitState(WolfStateManager wolf)
    {

    }
    public override void PlayAnimation(WolfStateManager wolf)
    {
        wolf.animator.SetBool("isRunning", true);

        wolf.animator.SetBool("isIdling", false);
        wolf.animator.SetBool("isWalking", false);
        wolf.animator.SetBool("isSneaking", false);

        wolf.animator.ResetTrigger("attack1");
    }

}
