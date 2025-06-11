using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityExecutor : MonoBehaviour
{
    public List<AbilityBase> Abilities;
    private PlayerEntity Player => GetPlayer();
    private PlayerEntity _player;

    private PlayerEntity GetPlayer()
    {
        if (_player != null)
            return _player;
        else
        {
            _player = FindFirstObjectByType<PlayerEntity>();
            return _player;
        }
    }

    private int QueuedAbilityIndex;
    public AbilityBase CurrentAbility;
    private float AbilityQueueTime = 0.5f; //how long ability stays in queue until dropped
    private float QueueTimer;

    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        _player = GetComponent<PlayerEntity>();

        foreach (var ability in Abilities)
        {
            ability.Initialize();
        }
        QueuedAbilityIndex = -1;
        ResetCurrentAbility();
    }
    public void Update()
    {
        HandleQueueTimer();
    }

    //counting down timer and reset queue once it has run out.
    private void HandleQueueTimer()
    {
        if (QueuedAbilityIndex < 0)
            return;

        if (QueueTimer <= 0)
        {
            ResetQueue();
        }
        else
        {
            QueueTimer -= Time.deltaTime;
        }
    }
    public bool CheckForAbilityPressed()
    {
        return false;
        if (UserInput.Instance.PrimaryAttackPressed)
        {
            return SetOrQueueNextAbility(0);
        }
        if (UserInput.Instance.SecondaryAttackPressed)
        {
            return SetOrQueueNextAbility(1);
        }
        if (UserInput.Instance.Seal1Pressed)
        {
            return SetOrQueueNextAbility(2);
        }
        if (UserInput.Instance.Seal2Pressed)
        {
            return SetOrQueueNextAbility(3);
        }
        if (UserInput.Instance.Seal3Pressed)
        {
            return SetOrQueueNextAbility(4);
        }
        if (UserInput.Instance.Seal4Pressed)
        {
            return SetOrQueueNextAbility(5);
        }
        return false;
    }
    private bool SetOrQueueNextAbility(int index)
    {
        if (!Abilities[index].HasAnyCooldown(Player, false) && Abilities[index].CanBeUsed(Player, false))
        {
            CurrentAbility = Abilities[index];
            ResetQueue();
            return true;
        }
        else
        {
            QueueAbility(index);
            return false;
        }
    }
    public bool TryExecuteNextAbility()
    {
        if (CurrentAbility == null)
            return false;
        if (CurrentAbility.TryUseAbility(Player))
        {
            CurrentAbility.OnAbilitiyFinished += StopAttack;
            IsAttacking = true;
            return true;
        }
        return false;
    }
    private void ResetQueue()
    {
        QueuedAbilityIndex = -1;
        QueueTimer = 0;
    }
    private void QueueAbility(int index)
    {
        QueuedAbilityIndex = index;
        QueueTimer = AbilityQueueTime;
    }

    private void StopAttack()
    {
        IsAttacking = false;
    }

    internal bool CanUseQueuedAbility()
    {
        if (QueuedAbilityIndex < 0)
            return false;

        if (!Abilities[QueuedAbilityIndex].HasAnyCooldown(Player, false) && Abilities[QueuedAbilityIndex].CanBeUsed(Player, false))
        {
            CurrentAbility = Abilities[QueuedAbilityIndex];
            ResetQueue();
            return true;
        }
        return false;
    }
    public void ResetCurrentAbility()
    {
        CurrentAbility = null;
    }
}
