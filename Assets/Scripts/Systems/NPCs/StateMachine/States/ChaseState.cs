public class ChaseState : State
{
    public ChaseState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override void Tick()
    {
        float dist = context.DistanceToPlayer;

        if (dist >= context.AttackRange)
        {
            context.Agent.SetDestination(context.Player.position);
        }
    }

    public override AIState? GetNextState()
    {
        if (context.DistanceToPlayer <= context.AttackRange)
        {
            return AIState.Attack;
        }
        else
            return AIState.Chase;
    }
}
