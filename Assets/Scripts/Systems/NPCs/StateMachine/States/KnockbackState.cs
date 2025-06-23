using UnityEngine;

public class KnockbackState : State
{
    public State chaseState;

    public KnockbackState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override void Enter()
    {
    }

    public override void Tick()
    {
    }

    public override AIState? GetNextState()
    {
        var interrupt = CheckGlobalInterrupts();
        if (interrupt != null)
            return interrupt;

        if (!context.Entity.Knockback.IsKnockBack)
            return AIState.Chase;

        return null;
    }
}