using UnityEngine;

public class AttackState : State
{
    public State chaseState;

    public AttackState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override void Enter()
    {
        //initial attack
        context.Agent.ResetPath();
    }

    public override void Tick()
    {
        //try to attack as many times while in the state while not overlapping attacks & respecting cooldowns
        if (!context.Entity.AbilityExecutor.IsAttacking)
        {
            var ab = context.Entity.AbilityController.GetAbility();
            if (ab != null && !ab.HasAnyCooldown(context.Entity, false))
            {
                Debug.Log("Enemy using Ability: " + ab);
                context.Entity.AbilityExecutor.TryExecuteAbility(ab);
            }
        }
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