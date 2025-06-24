using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityExperience : MonoBehaviour
{
    public int CurrentLevel;
    public float MaxXP;
    public float CurrentXP;

    private PlayerEntity _player;

    public int BaseXP = 50;
    public float Exponent = 1.35f;

    public void Initialize(PlayerEntity entity)
    { 
        _player = entity;
        CurrentXP = 0;
        CurrentLevel = 1;
        MaxXP = GetCurrentLevelXP();
    }

    private float GetCurrentLevelXP()
    {
        return BaseXP * (CurrentLevel * Exponent);
    }

    public void AddXP(float amount)
    {
        if (amount <= 0) return; //cant lose xp

        var finalAmount = amount * _player.Stats.ExperienceGainRate;
        CurrentXP += finalAmount;

        //level up
        if (CurrentXP >= MaxXP)
        {
            float levelLeftover = CurrentXP - MaxXP;
            CurrentXP = 0;
            CurrentLevel++;
            MaxXP = GetCurrentLevelXP();

            //loop until none left.
            if (levelLeftover > 0)
                AddXP(levelLeftover);

            //invoke later to allow for level up to show properly on xp bar 
            StartCoroutine(InvokeXPChangedDelayed(finalAmount));
            return;
        }
        //invoke directly if no level up
        GameEvents.OnEntityXPChanged.Invoke(new XPChangedEventArgs(_player, CurrentXP, MaxXP, finalAmount));
    }

    private IEnumerator InvokeXPChangedDelayed(float finalAmount)
    {
        yield return WaitManager.Wait(1);
        GameEvents.OnEntityXPChanged?.Invoke(new XPChangedEventArgs(_player, CurrentXP, MaxXP, finalAmount));
        yield return WaitManager.Wait(2);
        GameEvents.OnEntityLeveledUp?.Invoke(_player);
    }
}

