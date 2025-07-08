using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityExecutor : MonoBehaviour
{
    public AbilityBase CurrentAbility { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsSwingWindow { get; internal set; }

    private EntityBase _entity;

    public void Initialize(EntityBase entity)
    {
        _entity = entity;
    }

    public bool TryExecuteAbility(AbilityBase ability)
    {
        if (ability == null || IsAttacking || ability.HasAnyCooldown(_entity, false))
            return false;

        CurrentAbility = ability;
        if (CurrentAbility.TryUseAbility(_entity))
        {
            Debug.Log("AbilityExecutor.TryExecuteAbility() - " + CurrentAbility.AbilityData.Name);
            CurrentAbility.OnAbilitiyFinished += OnAbilityFinished;
            CurrentAbility.OnSwingStart += OnSwingStart;
            CurrentAbility.OnSwingEnd += OnSwingEnd;
            IsAttacking = true;
            return true;
        }

        CurrentAbility = null;
        return false;
    }

    private void OnSwingEnd()
    {
       // IsSwingWindow = false;
    }

    private void OnSwingStart()
    {
        IsSwingWindow = true;
    }

    private void OnAbilityFinished()
    {
        if (CurrentAbility != null)
            CurrentAbility.OnAbilitiyFinished -= OnAbilityFinished;

        IsSwingWindow = false;
        IsAttacking = false;
       
        Debug.Log("AbiliyExecutor.OnAbilityFinished() - " + CurrentAbility.AbilityData.Name);
        CurrentAbility = null;
    }

    public void ForceStopAbility()
    {
        if (CurrentAbility != null)
        {
            CurrentAbility.AbortAbility();
            if (CurrentAbility == null) 
                Debug.Log("Ability was null after aborting");
            else
                CurrentAbility.OnAbilitiyFinished -= OnAbilityFinished;
        }
        CurrentAbility = null;
        IsAttacking = false;
    }
}
