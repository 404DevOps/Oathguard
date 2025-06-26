using UnityEngine;

public class AttackState : State
{
    public State chaseState;

    public AttackState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override void Enter()
    {
        context.Agent.ResetPath();
        var ab = context.Entity.AbilityController.GetAbility();
        if (ab != null)
        {
            Debug.Log("Enemy using Ability " + ab);
            context.Entity.AbilityExecutor.TryExecuteAbility(ab);
        }
    }

    public override void Tick()
    {

    }
    public override AIState? GetNextState()
    {
        if (context.Entity.AbilityExecutor.IsAttacking)
            return null; //dont switch while still attacking
        if (context.DistanceToPlayer > context.AttackRange)
        {
            return AIState.Chase;
        }
        return null;
    }
}