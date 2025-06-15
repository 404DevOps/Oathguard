using UnityEngine;


public class PlayerEntity : EntityBase
{
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

    }

    public override void Die()
    {
        Debug.Log("Player died");
    }
}
