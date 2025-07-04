﻿using UnityEngine;

public class DeadState : State
{
    public State chaseState;

    public DeadState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Dead!");
    }

    public override AIState? GetNextState()
    {
        return null;
    }
}