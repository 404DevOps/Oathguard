using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour
{
    public int Index;
    public AbilityBase Ability;
    public Image Icon;
    public Image Glow;
    public Image Swipe;
    public Image KeybindSymbol;
    public TextMeshProUGUI KeybindText;
    private EntityBase _entity;

    private bool _isInitialized = false;

    private EntityCooldowns _cooldowns;
    private EntityGCD _gcd;

    private float _cooldownDuration = 0f;
    private float _cooldownStartTime = 0f;

    private float _gcdStartTime = 0f;
    private float _gcdDuration = 0f;
    private float _lastSwipeFillAmount = -1;

    private void OnEnable()
    {
        GameEvents.OnCooldownStart.AddListener(OnCooldownStarted);
        GameEvents.OnGCDStart.AddListener(OnGCDStarted);
        //UserInput.Instance.PlayerInput.onControlsChanged += UpdateIcon;
        UpdateKeybindSymbol(null);
    }


    private void OnDisable()
    {
        GameEvents.OnCooldownStart.RemoveListener(OnCooldownStarted);
        GameEvents.OnGCDStart.RemoveListener(OnGCDStarted);
        if (UserInput.Instance != null) //null ref when ending game cause userinput is destroyed before.
            UserInput.Instance.PlayerInput.onControlsChanged -= UpdateIcon;
    }

    public void UpdateKeybindSymbol(string controlScheme)
    {
        //controlScheme ??= UserInput.Instance.CurrentControlScheme;

        //KeybindSymbol.sprite = UserInput.Instance.GetSymbolForAbilityIndex(Index, controlScheme);
        //bool isKeyboard = controlScheme == "Keyboard";
        //KeybindText.gameObject.SetActive(isKeyboard);

        //if (isKeyboard)
        //    KeybindText.text = UserInput.Instance.GetKeybindForAbilityIndex(Index);
    }

    internal void Initialize(int abilityIndex, AbilityBase ability, EntityBase entity)
    {
        Index = abilityIndex;
        Ability = ability;
        _entity = entity;
        _isInitialized = true;
        _cooldowns = _entity.Cooldowns;
        _gcd = _entity.GCD;

        SetAbilityIcons(ability.AbilityInfo);
        UpdateKeybindSymbol(null);

        SetGlowState(false);
        CheckInitialCooldownState();
    }

    private void CheckInitialCooldownState()
    {
        if (_cooldowns.GetCooldownData(Ability.Id, out CooldownData cdInfo))
        {
            _cooldownStartTime = cdInfo.StartTime;
            _cooldownDuration = cdInfo.Duration;
        }
    }

    private void UpdateIcon(PlayerInput input)
    {
        UpdateKeybindSymbol(input.currentControlScheme);
    }

    void Update()
    {
        if (!_isInitialized || Ability == null)
        {
            return;
        }
        float currentTime = Time.time;

        float remainingCooldown = Mathf.Max(0, _cooldownStartTime + _cooldownDuration - currentTime);
        float remainingGCD = Mathf.Max(0, _gcdStartTime + _gcdDuration - currentTime);
        bool canBeUsed = Ability.CanBeUsed(_entity, false, false);

        HandleCooldownSwipe(remainingCooldown, remainingGCD, canBeUsed);
    }


    private void HandleCooldownSwipe(float remainingCooldown, float remainingGCD, bool canBeUsed)
    {
        float newFillAmount;
        bool newSwipeState;


        if (remainingCooldown > 0)
        {
            newSwipeState = true;
            newFillAmount = remainingCooldown / GetTrueAbilityCooldown(Ability);
        }
        else if (!canBeUsed)
        {
            newSwipeState = true;
            newFillAmount = 1;
        }
        else if (remainingGCD > 0)
        {
            newSwipeState = true;
            newFillAmount = remainingGCD / _gcdDuration;
        }
        else
        {
            newSwipeState = false;
            newFillAmount = 0;
        }

        SetSwipeState(newSwipeState, newFillAmount);
    }

    private void SetSwipeState(bool setActive, float newFillAmount)
    {
        if (Swipe.isActiveAndEnabled != setActive)
        {
            Swipe.gameObject.SetActive(setActive);
        }
        if (_lastSwipeFillAmount != newFillAmount)
        {
            Swipe.fillAmount = newFillAmount;
            _lastSwipeFillAmount = newFillAmount;
        }
    }
    private void SetGlowState(bool setActive)
    {
        if (Glow.isActiveAndEnabled != setActive)
            Glow.gameObject.SetActive(setActive);
    }

    private float GetRemainingGCD() => Mathf.Max(0, _gcdStartTime + _gcdDuration - Time.time);
    private float GetRemainingCooldown() => Mathf.Max(0, _cooldownStartTime + _cooldownDuration - Time.time);
    private bool AbilityHasAnyCooldown() => GetRemainingCooldown() > 0 || GetRemainingGCD() > 0;

    private float GetTrueAbilityCooldown(AbilityBase ability) => ability.AbilityInfo.Cooldown;

    private void SetAbilityIcons(AbilityData abilityInfo)
    {
        Icon.sprite = abilityInfo.Icon;
        Swipe.sprite = abilityInfo.Icon;
    }

    private void OnCooldownStarted(CooldownStartedEventArgs args)
    {
        if (args.EntityId != _entity.Id || Ability.Id != args.AbilityId)
            return;

        _cooldownDuration = args.Duration;
        _cooldownStartTime = args.StartTime;
    }

    private void OnGCDStarted(GCDStartedEventArgs args)
    {
        if (_entity.Id != args.EntityId)
            return;

        _gcdStartTime = args.StartTime;
        _gcdDuration = args.Duration;
    }
}