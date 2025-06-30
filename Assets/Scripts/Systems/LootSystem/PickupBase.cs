using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
    public abstract void OnCollected(GameObject collector);
}