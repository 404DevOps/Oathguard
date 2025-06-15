using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityResource : MonoBehaviour
{
    public float MaxResource => GetComponent<EntityStats>().MaxResource;
    public ResourceType ResourceType => GetComponent<EntityStats>().ResourceType;
    public float CurrentResource;

    public void Initialize(ResourceData data)
    {
        CurrentResource = (MaxResource / 100) * data.StartAmountPercentage;
    }

    public void ChangeResource(int changeAmount)
    {
        if (ResourceType == ResourceType.None) //bosses have free abilities for now
            return;

        CurrentResource += changeAmount;

        if (CurrentResource > MaxResource)
            CurrentResource = MaxResource;
        if (CurrentResource < 0)
            CurrentResource = 0;

        GameEvents.OnEntityResourceChanged.Invoke(new ResourceChangedEventArgs(GetComponent<EntityBase>(), CurrentResource, MaxResource));
    }
}