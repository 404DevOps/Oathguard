using System.Collections;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [Header("General")]
    public string Id;
    public bool IsDead;
    public EntityType Type;
   
    [Header("References")] 
    public Collider Collider;
    public WeaponSetInstance WeaponInstance;
    public EntityHealth Health;
    public EntityHurt Hurt;
    public EntityShield Shield;
    public EntityResource Resource;
    public EntityStats Stats;
    public Animator Animator;
    public EntityGCD GCD;
    public EntityCooldowns Cooldowns;
    public AbilityExecutor AbilityExecutor;
    public EntityImmunity Immunity;
    

    [Header("Containers")]
    public Transform HandSlotL;
    public Transform HandSlotR;
    public Transform AuraVisualsContainer;
    public Transform CombatTextContainer;

    [Header("Misc")]
    public Vector3 CombatTextOffset;

    private void Start()
    {
        Id = IdentifierService.GetNextId();
    }

    void Awake()
    {
        Animator.enabled = true;
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

        Shield = GetComponent<EntityShield>();
        Shield.Initialize(this);

        Hurt = GetComponent<EntityHurt>();
        Hurt.Initialize(this);

        Resource = GetComponent<EntityResource>();
        Resource.Initialize(AppearanceConfig.Instance().GetResourceData(Stats.ResourceType));

        GCD = GetComponent<EntityGCD>();
        Cooldowns = GetComponent<EntityCooldowns>();

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
        Animator.Play("Death", 0,0);
        StartCoroutine(SetPlayedDeadAnim());
    }

    private IEnumerator SetPlayedDeadAnim()
    {
        yield return null;
        Animator.enabled = false;
        Animator.SetBool("playedDeathAnimation", true);
    }
}