using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform Container;
    public CanvasGroup ContainerCanvasGroup;
    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _descriptionText;
    public TextMeshProUGUI _durationText;
    public TextMeshProUGUI _damageTypeText;

    public float _fadeOutDuration = 2;
    private float _remainingFadeOutTime;

    private bool _keepTooltipOpen = false;
    private bool _enabled = false;

    void OnEnable()
    {
        UIEvents.OnTooltipShow.AddListener(ShowTooltip);
        UIEvents.OnTooltipHide.AddListener(HideTooltip);

    }

    void OnDisable()
    {
        UIEvents.OnTooltipShow.RemoveListener(ShowTooltip);
        UIEvents.OnTooltipHide.RemoveListener(HideTooltip);
    }

    private void Update()
    {
        if (_keepTooltipOpen) return;
        if (!_enabled) return;

        _remainingFadeOutTime -= Time.deltaTime;

        var perc = _remainingFadeOutTime / _fadeOutDuration;
        ContainerCanvasGroup.alpha = Mathf.InverseLerp(ContainerCanvasGroup.alpha, 0, perc);
        if (perc == 0)
            _enabled = false;
    }

    public void ShowTooltip(TooltipData data)
    {
        _titleText.text = data.Title;
        _descriptionText.text = data.Description;
        _durationText.text = data.DurationText;
        _damageTypeText.text = data.DamageTypeText;
        _remainingFadeOutTime = _fadeOutDuration;

        var rect = this.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        ContainerCanvasGroup.alpha = 1;
        _keepTooltipOpen = true;
        _enabled = true;
    }
    private void HideTooltip()
    {
        _keepTooltipOpen = false;
        _remainingFadeOutTime = _fadeOutDuration;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _keepTooltipOpen = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }
}

public struct TooltipData
{
    public TooltipData(AbilityBase ability)
    {
        Title = ability.AbilityData.Name;
        Description = ability.AbilityData.Description;
        DurationText = ability.AbilityData.Cooldown + " sec Cooldown";
        var effect = ability.Effects.FirstOrDefault(fx => fx is DamageEffect);
        if (effect != null)
        {
            var dmgeffect = effect as DamageEffect;
            if (dmgeffect.Type != DamageType.None)
                DamageTypeText = dmgeffect.Type.ToString();
            else
                DamageTypeText = "";
        }
        else
        {
            DamageTypeText = "Spell";
        }
    }
    public TooltipData(AuraBase aura)
    {
        Title = aura.Name;
        Description = aura.Description;
        DurationText = aura.Duration + " sec Duration";
        DamageTypeText = "";
    }

    public string Title;
    public string Description;
    public string DurationText;
    public string DamageTypeText;
}
