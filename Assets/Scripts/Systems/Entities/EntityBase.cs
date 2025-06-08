using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public string Id;
    public bool IsAlive;

    public EntityType Type;

    public Collider2D MainCollider;
    public EntityCooldowns Cooldowns;
    public EntityHealth Health;
    public EntityStats Stats;
    public Animator Animator;
    public EntityGCD GCD;

    private void Start()
    {
        Id = IdentifierService.GetNextId();
    }


    void Awake()
    {
        Initialize();
    }
    internal IEnumerator NotifyNextFrame()
    {
        yield return null;
        GameEvents.OnEntityInitialized.Invoke(this);
    }

    public virtual Collider2D GetCollider() => MainCollider;
    protected virtual void Initialize()
    {
        Id = IdentifierService.GetNextId();

        Stats = GetComponent<EntityStats>();
        Stats.Initialize(this);

        Health = GetComponent<EntityHealth>();
        Health.Initialize(Stats);
        Health.EntityDied += Die;

        Cooldowns = GetComponent<EntityCooldowns>();
        Animator = GetComponentInChildren<Animator>();
    }

    public virtual void Die()
    {
        IsAlive = false;
        //Debug.Log("Entity died: " + Id);
    }
}

