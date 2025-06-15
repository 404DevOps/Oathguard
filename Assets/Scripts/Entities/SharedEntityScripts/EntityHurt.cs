using System;
using System.Collections;
using UnityEngine;

public class EntityHurt : MonoBehaviour
{
    public bool IsHurt;

    public float HurtStateDuration = 0.2f;
    public float HurtFlashDuration;
    public Color HurtColor;

    private Animator _animator;
    private EntityHealth _health;

    private void OnEnable()
    {
        GameEvents.OnEntityDamageReceived.AddListener(OnDamageReceived);
    }

    private void OnDisable()
    {
        GameEvents.OnEntityDamageReceived.RemoveListener(OnDamageReceived);
    }

    public void Initialize(EntityBase entity)
    {
        _animator = entity.Animator;
        _health = entity.Health;
    }

    private void OnDamageReceived(DamageContext data)
    {
        if (data.Target != _health.Entity) return;

        if (_health.CurrentHealth > 0)
        {
            _animator.SetTrigger("isHurt");
            StartCoroutine(HandleHurt());
            StartCoroutine(HurtFlash());
            // Lock movement etc. here
        }
    }

    private IEnumerator HurtFlash()
    {
        Debug.Log("Start HurtFlash");

        yield return WaitManager.Wait(HurtFlashDuration);

        Debug.Log("Stop HurtFlash");
    }

    private IEnumerator HandleHurt()
    {
        IsHurt = true;
        Debug.Log("Start Hurt");

        yield return WaitManager.Wait(HurtStateDuration);
        
        IsHurt = false;
        Debug.Log("Stop Hurt");
    }
}