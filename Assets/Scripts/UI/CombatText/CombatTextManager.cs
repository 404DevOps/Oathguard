using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CombatTextManager : MonoBehaviour
{
    [SerializeField] GameObject _floatingTextPrefab;
    [SerializeField] private int _poolSize = 20;

    private Queue<CombatText> _combatTextPool = new Queue<CombatText>();
    private GameObject InstantiationObject;
    private Camera _mainCamera;

    private void OnEnable()
    {
        GameEvents.OnEntityDamageReceived.AddListener(OnEntityDamaged);
        GameEvents.OnEntityHealed.AddListener(OnEntityHealed);
        InstantiationObject = new GameObject();
        InstantiationObject.SetActive(false);

        _mainCamera = Utility.Camera;

        //warm up pool
        if (_combatTextPool.Count == 0)
        {
            for (int i = 0; i < _poolSize; i++)
            {
                _combatTextPool.Enqueue(CreateNewText());
            }
        }
    }
    private void OnDisable()
    {
        GameEvents.OnEntityDamageReceived.RemoveListener(OnEntityDamaged);
        GameEvents.OnEntityHealed.RemoveListener(OnEntityHealed);
    }

    #region Event Handlers
    private void OnEntityDamaged(DamageContext args)
    {
        if (args.IsImmune)
        {
            var immuneColor = DamageColorConfig.Instance().GetColor(DamageColorType.Immune);
            ShowText(args.Target, "Immune", immuneColor, false);
            return;
        }
        string baseTextColor;

        if (args.Target is PlayerEntity)
            baseTextColor = DamageColorConfig.Instance().GetColor(DamageColorType.PlayerDamage).ToHexString();
        else if (args.IsCritical)
            baseTextColor = DamageColorConfig.Instance().GetColor(DamageColorType.Critical).ToHexString();
        else
            baseTextColor = DamageColorConfig.Instance().GetColor(DamageColorType.PlayerDamage).ToHexString();

        var dmgTextString = $"<color=#{baseTextColor}>{Mathf.RoundToInt(args.FinalDamage)}</color>";

        if (args.IsCritical)
            dmgTextString = "<sprite name=crit_sprite>" + dmgTextString;

        ShowText(args.Target, dmgTextString, Color.white, args.IsCritical);
    }
    private void OnEntityHealed(HealingContext args)
    {
        var healColor = DamageColorConfig.Instance().GetColor(DamageColorType.Heal);
        ShowText(args.Target, Mathf.RoundToInt(args.FinalAmount).ToString(), healColor, false);
    }

    public void ShowText(EntityBase entity, string text, Color color, bool isCrit)
    {
        if (_mainCamera == null)
            _mainCamera = Utility.Camera;

        CombatText ft = GetFloatingText();
        
        ft.Color = color;
        ft.Text = text;
        ft.IsCrit = isCrit;
        ft.Entity = entity;

        ft.transform.SetParent(entity.CombatTextContainer);
        ft.gameObject.transform.localPosition = Vector3.zero;

        ft.gameObject.SetActive(true);
    }
    #endregion
    #region Object Pooling Cycle

    private CombatText GetFloatingText()
    {
        if (_combatTextPool.Count > 0)
        {
            var floatingText = _combatTextPool.Dequeue();
            return floatingText;
        }
        return CreateNewText();
    }

    private CombatText CreateNewText()
    {
        var floatingTextGo = Instantiate(_floatingTextPrefab, InstantiationObject.transform);
        CombatText ft = floatingTextGo.GetComponent<CombatText>();

        ft.OnTextFinished += () => ReturnToPool(ft);
        return ft;
    }
    private void ReturnToPool(CombatText ft)
    {
        ft.gameObject.SetActive(false);
        _combatTextPool.Enqueue(ft);
    }

    #endregion
}