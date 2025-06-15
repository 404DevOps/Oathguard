using System;
using System.Collections;
using UnityEngine;

public class EntityHurt : MonoBehaviour
{
    public bool IsHurt;
    public float HurtStateDuration = 0.2f;

    private Animator _animator;
    private EntityHealth _health;
    private EntityBase _entity;

    private HurtFlash _flash;

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
        _entity = entity;
        _animator = entity.Animator;
        _health = entity.Health;
        _flash = GetComponent<HurtFlash>();
    }

    private void OnDamageReceived(DamageContext data)
    {
        if (data.Target != _health.Entity) return;

        if (_health.CurrentHealth > 0)
        {
            _animator.SetTrigger("isHurt");
            _entity.AbilityExecutor.ForceStopAbility();
            _flash.FlashRed();
            StartCoroutine(HandleHurt());
        }
    }

    private IEnumerator HandleHurt()
    {
        IsHurt = true;
        yield return WaitManager.Wait(HurtStateDuration);
        IsHurt = false;
    }
}