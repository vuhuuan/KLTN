using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStateManager : MonoBehaviour
{
    public WolfBaseState currentState;
    public WolfPounceState PounceState;
    public WolfChaseState ChaseState;

    public WolfIdleState IdleState;
    public WolfWalkState WalkState;

    public Animator animator;

    public float poundAttackDistance = 3f;
    public float meleAttackDistance = 1f;
    [SerializeField] Transform player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        PounceState = GetComponent<WolfPounceState>();
        ChaseState = GetComponent<WolfChaseState>();
        IdleState = GetComponent<WolfIdleState>();
        WalkState = GetComponent<WolfWalkState>();
    }

    void Start()
    {
        currentState = ChaseState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(WolfBaseState state)
    {
        state.EnterState(this);
        currentState = state;
    }


    public bool PlayerInPoundRange ()
    {
        float moveMagnitude = GetPlayerDirection().magnitude;

        if (moveMagnitude <= poundAttackDistance)
        {
            return true;
        }

        return false; 
    }

    public Vector3 GetPlayerDirection ()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        return direction;
    }
}
