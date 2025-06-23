using System;

public abstract class State
{
    protected EnemyAI context;
    protected StateMachine stateMachine;

    public State(EnemyAI context, StateMachine stateMachine)
    {
        this.context = context;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void Exit() { }

    public AIState? CheckGlobalInterrupts()
    {
        if (context.Entity.IsDead)
        {
            return AIState.Dead;
        }
        else if (context.Entity.Knockback.IsKnockBack)
        {
            return AIState.Knockback;
        }
        else
        {
            return null; // Default: stay in current state
        }
    }
    public abstract AIState? GetNextState();
}
