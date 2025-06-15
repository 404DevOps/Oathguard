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
    public Collider MainCollider;
    public EntityCooldowns Cooldowns;
    public EntityHealth Health;
    public EntityResource Resource;
    public EntityStats Stats;
    public Animator Animator;
    public WeaponHitbox Weapon;
    public EntityGCD GCD;
    public Transform AuraVisualsContainer;
    public Transform CombatTextContainer;

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
    protected virtual void Initialize()
    {
        Id = IdentifierService.GetNextId();

        Stats = GetComponent<EntityStats>();
        Stats.Initialize(this);

        Health = GetComponent<EntityHealth>();
        Health.Initialize(Stats);
        Health.EntityDied += Die;

        Resource = GetComponent<EntityResource>();
        Resource.Initialize(AppearanceConfig.Instance().GetResourceData(Stats.ResourceType));

        GCD = GetComponent<EntityGCD>();
        Cooldowns = GetComponent<EntityCooldowns>();
        Animator = GetComponentInChildren<Animator>();

        Weapon = GetComponentInChildren<WeaponHitbox>();
        Weapon.Initialize(this);

        AuraVisualsContainer = transform.Find("AuraVFX");
        CombatTextContainer = transform.Find("CombatText");
    }

    public virtual void Die()
    {
        IsAlive = false;
        //Debug.Log("Entity died: " + Id);
    }
}

