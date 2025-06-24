using UnityEngine;

public class KnockbackState : State
{
    public State chaseState;

    public KnockbackState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override AIState? GetNextState()
    {
        if (!context.Entity.Knockback.IsKnockBack)
            return AIState.Chase;

        return null;
    }
}