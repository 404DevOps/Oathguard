using System.Collections.Generic;
using UnityEngine;

public class AbilityExecutor : MonoBehaviour
{
    public AbilityBase CurrentAbility { get; private set; }
    public bool IsAttacking { get; private set; }
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
            Debug.Log("Executing Ability: " + CurrentAbility);
            CurrentAbility.OnAbilitiyFinished += OnAbilityFinished;
            IsAttacking = true;
            return true;
        }

        CurrentAbility = null;
        return false;
    }

    private void OnAbilityFinished()
    {
        if (CurrentAbility != null)
            CurrentAbility.OnAbilitiyFinished -= OnAbilityFinished;

        IsAttacking = false;
        CurrentAbility = null;
    }

    public void ForceStopAbility()
    {
        if (CurrentAbility != null)
            CurrentAbility.OnAbilitiyFinished -= OnAbilityFinished;

        CurrentAbility = null;
        IsAttacking = false;
    }
}
