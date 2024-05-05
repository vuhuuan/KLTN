using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WolfBaseState : MonoBehaviour
{
    public abstract void EnterState(WolfStateManager wolf);

    public abstract void UpdateState(WolfStateManager wolf);

    public abstract void ExitState(WolfStateManager wolf);

    public abstract void PlayAnimation(WolfStateManager wolf);

}
