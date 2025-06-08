using UnityEngine;


public class PlayerEntity : EntityBase
{

    void Awake()
    {
        Initialize();
        StartCoroutine(NotifyNextFrame());
    }

    protected override void Initialize()
    {
        base.Initialize();
        Stats = GetComponent<EntityStats>();

        var movement = gameObject.GetComponent<PlayerMovement>();
        movement.Initialize();

    }

    public override void Die()
    {
        Debug.Log("Player died");
    }
}
