using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{ 
    [SerializeField] private float queueDuration = 0.5f;
    public List<AbilityBase> Abilities;
    private AbilityExecutor _executor;

    private AbilityBase queuedAbility;
    private float queueTimer;
    private PlayerEntity _playerEntity;

    private void Update()
    {
        HandleInput();
        HandleQueue();
    }

    public void Initialize(PlayerEntity entity)
    {
        _playerEntity = entity;
        _executor = entity.AbilityExecutor;
        foreach (var ability in Abilities)
            ability.Initialize();
    }

    private void HandleInput()
    {
        if (UserInput.Instance.PrimaryAttackPressed)
        {
            TrySetOrQueue(0);
        }
        if (UserInput.Instance.SecondaryAttackPressed)
        {
            TrySetOrQueue(1);
        }
        if (UserInput.Instance.Oath1Pressed)
        {
            TrySetOrQueue(2);
        }
        if (UserInput.Instance.Oath2Pressed)
        {
            TrySetOrQueue(3);
        }
        if (UserInput.Instance.Oath3Pressed)
        {
            TrySetOrQueue(4);
        }
        if (UserInput.Instance.Oath4Pressed)
        {
            TrySetOrQueue(5);
        }
    }

    private void HandleQueue()
    {
        if (queuedAbility == null) return;

        queueTimer -= Time.deltaTime;
        if (queueTimer <= 0f)
        {
            Debug.Log("Ability queue expired.");
            queuedAbility = null;
        }
        else if (!_executor.IsAttacking)
        {
            Debug.Log("Executing queued ability.");
            _executor.TryExecuteAbility(queuedAbility);
            queuedAbility = null;
        }
    }

    private void TrySetOrQueue(int index)
    {
        var ability = Abilities[index];

        if (_executor.IsAttacking)
        {
            Debug.Log($"Queueing ability: {ability}");
            queuedAbility = ability;
            queueTimer = queueDuration;
        }
        else
        {
            if(_playerEntity.CanUseAbilities)
                _executor.TryExecuteAbility(ability);
        }
    }
}
