using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EntityKnockback : MonoBehaviour
{
    EntityBase Entity;
    Rigidbody _rb;
    NavMeshAgent _agent;

    public float Force;
    public float Duration;

    public bool IsKnockBack = false;

    public void Initialize(EntityBase entity)
    {
        Entity = entity;
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();

        GameEvents.OnEntityDamageReceived.AddListener(OnDamageReceived);
    }

    private void OnDamageReceived(DamageContext context)
    {
        if (context.Target.Id != Entity.Id) return;
        if (context.IgnoreHurt) return;

        var direction = context.Target.transform.position - context.Origin.transform.position;
        Knockback(direction, Force, Duration);
    }

    public void Knockback(Vector3 direction, float force, float duration)
    {
        StartCoroutine(KnockbackRoutine(direction, force, duration));
    }
    private IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
    {
        //enable physics + stop navmeshagent
        IsKnockBack = true;
        _agent.enabled = false;
        _rb.isKinematic = false;
        _rb.linearVelocity = (direction.normalized * force);

        yield return new WaitForSeconds(duration);

        //stop physics & reenable agent;
        _rb.linearVelocity = Vector3.zero;
        _rb.isKinematic = true;
        _agent.enabled = true;
        IsKnockBack = false;
    }
}
