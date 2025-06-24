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
    public abstract AIState? GetNextState();
}
