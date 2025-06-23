using System.Collections.Generic;

public class StateMachine
{
    private Dictionary<AIState, State> _states = new();
    public State CurrentState { get; private set; }
    public AIState CurrentStateID { get; private set; }

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
        CurrentState.Enter();
    }

    public void Tick()
    {
        if (CurrentState == null) return;

        var next = CurrentState.GetNextState();
        if (next != null && next != CurrentStateID)
        {
            CurrentState.Exit();
            CurrentState = GetState(next.Value);
            CurrentState.Enter();
        }

        CurrentState.Tick();
    }
}