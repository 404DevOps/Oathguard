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
        if (amount <= 0) return;

        float scaledAmount = amount * _player.Stats.ExperienceGainRate;
        float remainingXP = scaledAmount;
        float gainedXPThisCall = 0;

        while (remainingXP > 0)
        {
            float xpToNextLevel = MaxXP - CurrentXP;

            if (remainingXP >= xpToNextLevel)
            {
                remainingXP -= xpToNextLevel;
                CurrentXP = 0;
                CurrentLevel++;
                MaxXP = GetCurrentLevelXP();

                GameEvents.OnEntityLeveledUp?.Invoke(_player);
            }
            else
            {
                CurrentXP += remainingXP;
                gainedXPThisCall += remainingXP;
                remainingXP = 0;
            }
        }

        GameEvents.OnEntityXPChanged?.Invoke(new XPChangedEventArgs(_player, CurrentXP, MaxXP, scaledAmount));
    }

}

