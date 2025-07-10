using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticleSystem : MonoBehaviour
{
    Action<GameObject> _onReturn;
    public void Init(Action<GameObject> action)
    {
        _onReturn = action;
    }

    void OnDisable()
    { 
        _onReturn?.Invoke(gameObject);
    }
}