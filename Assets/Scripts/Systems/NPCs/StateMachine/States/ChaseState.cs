using System;
using UnityEngine;

public class ChaseState : State
{
    public ChaseState(EnemyAI context, StateMachine stateMachine) : base(context, stateMachine) { }

    public override void Enter()
    {

    }
    public override void Tick()
    {
        //in update for smoother locomotion
        SetWalkAnimation();

        if(context.Agent.speed != context.Entity.Stats.MoveSpeed)
            context.Agent.speed = context.Entity.Stats.MoveSpeed;

        float dist = context.DistanceToPlayer;

        if (dist >= context.AttackRange)
        {
            context.Agent.SetDestination(context.Player.transform.position);
        }
    }

    private void SetWalkAnimation()
    {
        Vector3 worldVelocity = context.Agent.velocity;
        Vector3 localVelocity = context.Agent.transform.InverseTransformDirection(worldVelocity);

        float moveX = localVelocity.x; // sideways strafe movement
        float moveY = localVelocity.z; // forward/back movement

        context.Entity.Animator.SetFloat("moveX", moveX);
        context.Entity.Animator.SetFloat("moveY", moveY);
    }

    public override void Exit()
    {
        context.Entity.Animator.SetFloat("moveX", 0);
        context.Entity.Animator.SetFloat("moveY", 0);
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
