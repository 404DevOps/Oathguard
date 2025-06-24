using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class CombatTextManager : MonoBehaviour
{
    [SerializeField] GameObject _combatTextPrefab;
    [SerializeField] private int _poolSize = 20;

    private Queue<CombatText> _textPool = new Queue<CombatText>();
    private Transform TextPool;

    private void Start()
    {
        TextPool = transform.Find("TextPool");

        //warm up pool
        if (_textPool.Count == 0)
        {
            for (int i = 0; i < _poolSize; i++)
            {
                _textPool.Enqueue(CreateText());
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.OnEntityDamageReceived.AddListener(OnEntityDamaged);
        GameEvents.OnEntityHealed.AddListener(OnEntityHealed);
        GameEvents.OnEntityShieldAbsorbed.AddListener(OnShieldAbsorbed);
        GameEvents.OnEntityXPChanged.AddListener(OnXPGained);
        GameEvents.OnEntityLeveledUp.AddListener(OnLevelUp);


    }

    private void OnLevelUp(EntityBase entity)
    {
        var baseTextColor = TextColorConfig.Instance().GetColor(TextColorType.LevelUp);
        ShowText(entity, "Level Up!", baseTextColor, false);
    }

    private void OnXPGained(XPChangedEventArgs args)
    {
        var baseTextColor = TextColorConfig.Instance().GetColor(TextColorType.ExperienceGained);
        ShowText(args.Entity, Mathf.RoundToInt(args.CurrentXP).ToString(), baseTextColor, false);
    }


    private void OnDisable()
    {
        GameEvents.OnEntityDamageReceived.RemoveListener(OnEntityDamaged);
        GameEvents.OnEntityHealed.RemoveListener(OnEntityHealed);
        GameEvents.OnEntityShieldAbsorbed.RemoveListener(OnShieldAbsorbed);
    }

    private void OnShieldAbsorbed(ShieldAbsorbedEventArgs args)
    {
        var baseTextColor = TextColorConfig.Instance().GetColor(TextColorType.ShieldAbsorbed);
        ShowText(args.Entity, Mathf.RoundToInt(args.AbsorbedAmount).ToString(), baseTextColor, false);
    }

    #region Event Handlers
    private void OnEntityDamaged(DamageContext args)
    {
        if (args.FinalDamage <= 0) return;

        if (args.IsImmune)
        {
            var immuneColor = TextColorConfig.Instance().GetColor(TextColorType.Immune);
            ShowText(args.Target, "Immune", immuneColor, false);
            return;
        }
        Color baseTextColor = Color.white;

        if (args.Target is PlayerEntity)
            baseTextColor = TextColorConfig.Instance().GetColor(args.IsCritical ? TextColorType.PlayerCriticalReceived : TextColorType.PlayerDamageReceived);
        else if (args.IsCritical)
            baseTextColor = TextColorConfig.Instance().GetColor(TextColorType.CriticalDamageDone);
        else
            baseTextColor = TextColorConfig.Instance().GetColor(TextColorType.NormalDamageDone);

        var dmgTextString = Mathf.RoundToInt(args.FinalDamage).ToString();

        //if (args.IsCritical)
            //add crit sprite?

        ShowText(args.Target, dmgTextString, baseTextColor, args.IsCritical);
    }
    private void OnEntityHealed(HealingContext args)
    {
        var healColor = TextColorConfig.Instance().GetColor(TextColorType.Heal);
        ShowText(args.Target, Mathf.RoundToInt(args.FinalAmount).ToString(), healColor, false);
    }

    public void ShowText(EntityBase entity, string text, Color color, bool isCrit)
    {
        CombatText ft = GetFloatingText();
        ft.transform.SetParent(entity.CombatTextContainer);
        ft.Setup(entity, text, color, isCrit);
        ft.gameObject.SetActive(true);
    }
    #endregion
    #region Object Pooling Cycle

    private CombatText GetFloatingText()
    {
        if (_textPool.Count > 0)
        {
            var floatingText = _textPool.Dequeue();
            return floatingText;
        }
        return CreateText();
    }

    private CombatText CreateText()
    {
        var floatingTextGo = Instantiate(_combatTextPrefab, TextPool);
        CombatText ft = floatingTextGo.GetComponent<CombatText>();
        ft.OnTextFinished += () => ReturnToPool(ft);
        return ft;
    }
    private void ReturnToPool(CombatText ft)
    {
        ft.gameObject.SetActive(false);
        _textPool.Enqueue(ft);
    }

    #endregion
}