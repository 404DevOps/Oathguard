using UnityEngine;

public class AttackState : State
{
    public State chaseState;

    public AttackState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override void Enter()
    {
        context.Agent.ResetPath();
        Debug.Log("Attack!");
        // play animation here
    }

    public override void Tick()
    {

    }
    public override AIState? GetNextState()
    {
        if (context.DistanceToPlayer > context.AttackRange)
        {
            return AIState.Chase;
        }
        return null;
    }
}