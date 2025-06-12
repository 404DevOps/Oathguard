using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStatusEffectHandler : MonoBehaviour
{
    private readonly Dictionary<StatusEffectType, int> activeEffectCounts = new();
    private readonly Dictionary<StatusEffectType, Coroutine> visualCoroutines = new();
    public List<StatusEffectOverlay> _statusEffectOverlays = new();

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private EntityBase _entity;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _entity = GetComponent<EntityBase>();
    }
    #region Start/Stop
    public void StartStatusEffect(StatusEffectType type)
    {
        if (!activeEffectCounts.ContainsKey(type))
            activeEffectCounts[type] = 0;

        activeEffectCounts[type]++;

        if (activeEffectCounts[type] == 1)
        {
            if (type == StatusEffectType.Poisoned)
            {
                Coroutine routine = StartCoroutine(PoisonBlinkRoutine());
                visualCoroutines[type] = routine;
            }
            else if (StatusEffectUtility.IsRootEffect(type))
            {
                SetEffectOverlay(type, true);
                SetRootState(true);
            }
            else if (StatusEffectUtility.IsStunEffect(type))
            {
                SetEffectOverlay(type, true);
                SetStunState(true);
            }
            else
            {
                SetEffectOverlay(type, true);
            }
        }
    }

    public void StopStatusEffect(StatusEffectType type)
    {
        if (!activeEffectCounts.ContainsKey(type))
            return;

        activeEffectCounts[type] = Mathf.Max(0, activeEffectCounts[type] - 1);

        if (activeEffectCounts[type] == 0)
        {
            if (visualCoroutines.TryGetValue(type, out var routine))
            {
                StopCoroutine(routine);
                visualCoroutines.Remove(type);
            }
            if (StatusEffectUtility.IsRootEffect(type))
            {
                SetEffectOverlay(type, false);
                SetRootState(false);
            }
            else if (StatusEffectUtility.IsStunEffect(type))
            {
                SetEffectOverlay(type, false);
                SetStunState(false);
            }
            else
            {
                SetEffectOverlay(type, false);
            }

            ResetVisuals(type);
        }
    }

    #endregion

    #region Effects
    private IEnumerator PoisonBlinkRoutine()
    {
        while (true)
        {
            _spriteRenderer.color = Color.green;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(1.8f); // 2s total
        }
    }
    private void SetEffectOverlay(StatusEffectType type, bool active)
    {
        var overlay = _statusEffectOverlays.FirstOrDefault(seo => seo.Type == type);
        if (overlay == null)
            return;

        if (active)
        {
            if (overlay.EffectInstance == null)
            {
                overlay.EffectInstance = Instantiate(
                    overlay.EffectPrefab,
                    transform.position + overlay.EntityOffset,
                    Quaternion.identity,
                    transform
                );
            }
            else
            {
                overlay.EffectInstance.transform.position = transform.position + overlay.EntityOffset;
            }

            overlay.EffectInstance.SetActive(true);
        }
        else
        {
            if (overlay.EffectInstance != null)
                overlay.EffectInstance.SetActive(false);
        }
    }
    private void SetRootState(bool isRooted)
    {
        _animator?.SetBool("isRooted", isRooted);
        if (_entity is PlayerEntity)
        {
            PlayerEntity player = (PlayerEntity)_entity;
            player.CanMove = !isRooted;
            player.CanRotate = !isRooted;
        }
    }
    private void SetStunState(bool isStunned)
    {
        _animator?.SetBool("isStunned", isStunned);
        if (_entity is PlayerEntity)
        {
            PlayerEntity player = (PlayerEntity)_entity;
            player.CanMove = !isStunned;
            player.CanRotate = !isStunned;
            player.CanUseAbilities = !isStunned;
        }
    }
    #endregion

    private void ResetVisuals(StatusEffectType type)
    {
        // You could reset only specific visuals if needed
        _spriteRenderer.color = Color.white;
    }
}

[Serializable]
public class StatusEffectOverlay
{
    public StatusEffectType Type;
    public GameObject EffectPrefab;
    public GameObject EffectInstance;
    public Vector3 EntityOffset;
}