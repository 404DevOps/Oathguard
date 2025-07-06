using System;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
    public float baseAttractionSpeed = 3f;
    public float acceleration = 5f; // units per second squared
    private float magnetizedTime = -1f;

    public float magnetizeDelay = 1f;
    private float spawnTime;

    public Action<PickupBase> OnLootCollected;

    public virtual void OnCollected(PlayerEntity collector) 
    {
        OnLootCollected?.Invoke(this); 
    }

    private void Awake()
    {
        magnetizedTime = -1f;
        spawnTime = Time.time;
    }

    public bool CanBeMagnetized => Time.time - spawnTime >= magnetizeDelay;

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "LootMagnet") return;
        if (other.transform.parent.TryGetComponent(out PlayerEntity player))
        {
            Debug.Log("PickupCollected");
            OnCollected(player);
        }
    }

    public void StartMagnetizing()
    {
        if (magnetizedTime < 0f)
            magnetizedTime = Time.time;
    }

    public void MoveToward(Vector3 target)
    {
        if (!CanBeMagnetized)
            return;

        if (magnetizedTime < 0f)
            StartMagnetizing();

        float timeSinceMagnet = Time.time - magnetizedTime;
        float speed = baseAttractionSpeed + acceleration * timeSinceMagnet;

        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
}
