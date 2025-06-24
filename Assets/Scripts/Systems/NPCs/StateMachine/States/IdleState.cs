using System;

public class IdleState : State
{
    public State nextState;

    public IdleState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override void Tick()
    {
    }

    public override AIState? GetNextState()
    {
        if (context.DistanceToPlayer < context.ChaseRange)
            return AIState.Chase;
        else return null;
    }
}