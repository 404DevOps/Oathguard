using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class PickupBase : MonoBehaviour
{
    public abstract void OnCollected(PlayerEntity collector);

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out PlayerEntity player))
        {
            Debug.Log("PickupCollected");
            OnCollected(player);
        }
    }
}