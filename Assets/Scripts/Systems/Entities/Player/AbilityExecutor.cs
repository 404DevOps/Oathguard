using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityExecutor : MonoBehaviour
{
    public List<AbilityBase> Abilities;
    private EntityBase Entity => GetEntity();
    private EntityBase _entity;

    private EntityBase GetEntity()
    {
        if (_entity != null)
            return _entity;
        else
        {
            _entity = GetComponent<EntityBase>();
            return _entity;
        }
    }

    public AbilityBase CurrentAbility;
    public AbilityBase NextAbility; //queue only 1 ability
    private float AbilityQueueTime = 0.5f; //how long ability stays in queue until dropped
    private float CurrentQueueTimer;

    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        _entity = GetComponent<EntityBase>();

        foreach (var ability in Abilities)
        {
            ability.Initialize();
        }
        CurrentAbility = null;
        NextAbility = null;
    }
    public void Update()
    {
        CheckForAbilityPressed();
        HandleQueueTimer();
        TryExecuteCurrentAbility();
    }

    //counting down timer and reset queue once it has run out.
    private void HandleQueueTimer()
    {
        if (NextAbility == null)
            return;

        if (CurrentQueueTimer <= 0)
        {
            Debug.Log("Reset Ability Queue");
            ResetQueue();
        }
        else
        {
            CurrentQueueTimer -= Time.deltaTime;
        }
    }
    public bool CheckForAbilityPressed()
    {
        if (UserInput.Instance.PrimaryAttackPressed)
        {
            return SetOrQueueNextAbility(0);
        }
        if (UserInput.Instance.SecondaryAttackPressed)
        {
            return SetOrQueueNextAbility(1);
        }
        if (UserInput.Instance.Oath1Pressed)
        {
            return SetOrQueueNextAbility(2);
        }
        if (UserInput.Instance.Oath2Pressed)
        {
            return SetOrQueueNextAbility(3);
        }
        if (UserInput.Instance.Oath3Pressed)
        {
            return SetOrQueueNextAbility(4);
        }
        if (UserInput.Instance.Oath4Pressed)
        {
            return SetOrQueueNextAbility(5);
        }
        return false;
    }
    /// <summary>
    /// Returns true if Ability was immediately used.
    /// </summary>
    private bool SetOrQueueNextAbility(int index)
    {
        if (IsAttacking)
        {
            Debug.Log("Queued Abiliy: " + Abilities[index]);
            QueueAbility(Abilities[index]);
            return false;
        }
        else if (!Abilities[index].HasAnyCooldown(Entity, false))
        {
            CurrentAbility = Abilities[index];
            ResetQueue();
            return true;
        }
        return false;
    }
    public bool TryExecuteCurrentAbility()
    {
        if (CurrentAbility == null || IsAttacking)
            return false;
        if (CurrentAbility.TryUseAbility(Entity))
        {
            Debug.Log("Executing Ability: " + CurrentAbility);
            CurrentAbility.OnAbilitiyFinished += StopAttack;
            IsAttacking = true;
            return true;
        }
        return false;
    }
    private void ResetQueue()
    {
        NextAbility = null;
        CurrentQueueTimer = AbilityQueueTime;
    }
    private void QueueAbility(AbilityBase ability)
    {
        NextAbility = ability;
        CurrentQueueTimer = AbilityQueueTime;
    }

    private void StopAttack()
    {
        Debug.Log("Stop Attack.");
        CurrentAbility.OnAbilitiyFinished -= StopAttack;
        IsAttacking = false;
        CurrentAbility = null;

        //use queued ability next.
        if (NextAbility != null)
        {
            Debug.Log("Readying Ability from Queue.");
            CurrentAbility = NextAbility;
            NextAbility = null;
        }
    }

    internal bool CanUseQueuedAbility()
    {
        if (NextAbility != null)
            return false;

        if (!NextAbility.HasAnyCooldown(Entity, false) && !IsAttacking)
        {
            CurrentAbility = NextAbility;
            ResetQueue();
            return true;
        }
        return false;
    }
    public void ResetCurrentAbility()
    {
        if(CurrentAbility != null)
            CurrentAbility.OnAbilitiyFinished -= StopAttack;

        CurrentAbility = null;
    }
}
