using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NPCEntity Entity;
    public StateMachine StateMachine;
    public PlayerEntity Player => EntityManager.Instance.Player;
    public NavMeshAgent Agent;

    public float AttackRange = 2.5f;
    public float ChaseRange = 10f;
    public AIType Type;

    private bool _initialized;

    public void Initialize(NPCEntity entity)
    {
        Entity = entity;
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine(this);

        StateMachine.AddState(AIState.Idle, new IdleState(this, StateMachine));
        StateMachine.AddState(AIState.Chase, new ChaseState(this, StateMachine));
        StateMachine.AddState(AIState.Attack, new AttackState(this, StateMachine));
        StateMachine.AddState(AIState.Knockback, new KnockbackState(this, StateMachine));
        StateMachine.AddState(AIState.Dead, new DeadState(this, StateMachine));

        if (Type == AIType.Ranged)
        {
            //add retreat state
        }
        else if(Type == AIType.Necromancer)
        { 
            //add summon state
        }

        StateMachine.SetInitialState(AIState.Chase);
        _initialized = true;
    }

    public void Update()
    {
        if (!_initialized) return;
        StateMachine.Tick();
    }

    public float DistanceToPlayer => Vector3.Distance(transform.position, Player.transform.position);
}
