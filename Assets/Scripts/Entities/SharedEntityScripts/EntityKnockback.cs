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
    Coroutine _knockbackRoutine;

    public void Initialize(EntityBase entity)
    {
        Entity = entity;
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        GameEvents.OnEntityDied.AddListener(OnEntityDied);
        GameEvents.OnEntityDamageReceived.AddListener(OnDamageReceived);
    }
    private void OnDisable()
    {
        GameEvents.OnEntityDied.RemoveListener(OnEntityDied);
        GameEvents.OnEntityDamageReceived.RemoveListener(OnDamageReceived);
    }

    private void OnEntityDied(EntityBase entity)
    {
        if (entity.Id != Entity.Id) return;
        if (IsKnockBack) 
        {
            if(_knockbackRoutine != null)
                StopCoroutine(_knockbackRoutine);

            StopKnockback();
        }
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
        //only start if not already knocked back
        if (!IsKnockBack)
        {
            _knockbackRoutine = StartCoroutine(KnockbackRoutine(direction, force, duration));
        }      
    }
    private IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
    {
        //enable physics + stop navmeshagent
        StartKnockback(direction * force);

        yield return new WaitForSeconds(duration);

        StopKnockback();
    }

    private void StartKnockback(Vector3 directionalForce)
    {
        IsKnockBack = true;
        _agent.enabled = false;
        _rb.isKinematic = false;
        _rb.linearVelocity = directionalForce;

    }

    private void StopKnockback()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.isKinematic = true;
        _agent.enabled = true;
        IsKnockBack = false;
    }
}
