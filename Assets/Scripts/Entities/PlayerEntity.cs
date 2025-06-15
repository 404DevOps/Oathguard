using UnityEngine;


public class PlayerEntity : EntityBase
{
    [Header("Player Attributes")]
    public bool CanMove;
    public bool CanRotate;
    public bool CanUseAbilities;
    public PlayerAbilityController AbilityController;

    void Awake()
    {
        Initialize();
        StartCoroutine(NotifyNextFrame());
    }

    protected override void Initialize()
    {
        base.Initialize();

        AbilityController = GetComponent<PlayerAbilityController>();
        AbilityController.Initialize(this);

        var movement = gameObject.GetComponent<PlayerMovement>();
        movement.Initialize();

        CanMove = true;
        CanRotate = true;
        CanUseAbilities = true;

    }

    public override void OnEntityDied(string entityId)
    {
        if (entityId != Id) return;

        CanMove = false;
        CanRotate = false;
        CanUseAbilities = false;

        base.OnEntityDied(entityId);
        Debug.Log("Player died");
    }
}
