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
    [Header("General")]
    public string Id;
    public bool IsDead;
    public EntityType Type;
   
    [Header("References")] 
    public Collider Collider;
    public WeaponHitbox Weapon;
    public EntityHealth Health;
    public EntityHurt Hurt;
    public EntityResource Resource;
    public EntityStats Stats;
    public Animator Animator;
    public EntityGCD GCD;
    public EntityCooldowns Cooldowns;
    public AbilityExecutor AbilityExecutor;
    public EntityImmunity Immunity;

    [Header("Containers")] 
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
        Animator = GetComponent<Animator>();

        Stats = GetComponent<EntityStats>();
        Stats.Initialize(this);

        Health = GetComponent<EntityHealth>();
        Health.Initialize(this);

        Hurt = GetComponent<EntityHurt>();
        Hurt.Initialize(this);

        Resource = GetComponent<EntityResource>();
        Resource.Initialize(AppearanceConfig.Instance().GetResourceData(Stats.ResourceType));

        GCD = GetComponent<EntityGCD>();
        Cooldowns = GetComponent<EntityCooldowns>();

        Weapon = GetComponentInChildren<WeaponHitbox>();
        Weapon.Initialize(this);

        AbilityExecutor = GetComponent<AbilityExecutor>();
        AbilityExecutor.Initialize(this);

        Immunity = GetComponent<EntityImmunity>();

        AuraVisualsContainer = transform.Find("AuraVFX");
        CombatTextContainer = transform.Find("CombatText");

        GameEvents.OnEntityDied.AddListener(OnEntityDied);
    }

    public virtual void OnEntityDied(string entityId)
    {
        if (entityId != Id) return;

        IsDead = true;
        Animator.SetBool("isDead", true);
        StartCoroutine(SetPlayedDeadAnim());
    }

    private IEnumerator SetPlayedDeadAnim()
    {
        yield return null;
        Animator.SetBool("playedDeathAnimation", true);
    }
}