using System.Collections.Generic;

public class StateMachine
{
    public State CurrentState { get; set; }
    private AIState CurrentStateID { get; set; }

    private Dictionary<AIState, State> _states = new();
    private EnemyAI _context;

    public StateMachine(EnemyAI context)
    {
        _context = context;
    }

    public void AddState(AIState id, State state)
    {
        _states[id] = state;
    }

    public State GetState(AIState id)
    {
        return _states[id];
    }
    public void SetInitialState(AIState stateId)
    {
        var state = GetState(stateId);
        CurrentState = state;
        CurrentStateID = stateId;
        CurrentState.Enter();
    }

    public void Tick()
    {
        if (CurrentState == null) return;

        var interrupt = CheckGlobalInterrupts();
        if (interrupt != null && interrupt != CurrentStateID)
        {
            SwitchToState(interrupt.Value);
            return;
        }

        var nextState = CurrentState.GetNextState();
        if (nextState != null && nextState != CurrentStateID)
        {
            SwitchToState(nextState.Value);
        }

        CurrentState.Tick();
    }
    private AIState? CheckGlobalInterrupts()
    {
        if (_context.Entity.IsDead)
            return AIState.Dead;

        if (_context.Entity.Knockback.IsKnockBack)
            return AIState.Knockback;

        return null;
    }
    private void SwitchToState(AIState newState)
    {
        CurrentState.Exit();
        CurrentStateID = newState;
        CurrentState = GetState(newState);
        CurrentState.Enter();
    }
}