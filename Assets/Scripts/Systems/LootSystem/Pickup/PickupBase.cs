using System;
using UnityEngine;
using UnityEngine.VFX;

public abstract class PickupBase : MonoBehaviour
{
    public float baseAttractionSpeed = 3f;
    public float acceleration = 5f; // units per second squared
    private float magnetizedTime = -1f;

    public float magnetizeDelay = 1f;
    private float spawnTime;

    public GameObject _collectedVFX;
    public Action OnMagnetized;
    public Action<PickupBase> OnLootCollected;

    public virtual void OnCollected(PlayerEntity collector) 
    {
        var instance = Instantiate(_collectedVFX, collector.AuraVisualsContainer);
        var vfx = instance.GetComponent<VisualEffect>();
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
        if (other.transform.TryGetComponent(out PlayerEntity player))
        {
            Debug.Log("PickupCollected");
            OnCollected(player);
        }
    }

    public void StartMagnetizing()
    {
        if (magnetizedTime < 0f)
        {
            magnetizedTime = Time.time;
            OnMagnetized?.Invoke();
        }
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
