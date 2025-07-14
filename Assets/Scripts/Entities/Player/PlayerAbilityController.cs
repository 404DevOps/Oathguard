using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] private float comboWindow = 1f; // unified window for input buffer & chaining
    public List<AbilityBase> Abilities;

    private AbilityExecutor _executor;
    private PlayerEntity _playerEntity;
    private bool _isInitialized = false;

    private WeaponComboData currentComboData;
    private Dictionary<int, List<AbilityBase>> activeCombos = new(); // inputIndex -> comboList

    private AbilityBase queuedAbility;
    private int queuedIndex;
    private float comboTimer = 0f;
    private bool isComboActive = false;
    private int comboIndex = 0;

    private void Update()
    {
        if (!_isInitialized) return;

        HandleInput();

        if (queuedAbility != null && !_executor.IsAttacking)
        {
            _executor.TryExecuteAbility(queuedAbility);
            AdvanceCombo(activeCombos[queuedIndex]); // continue the combo
            queuedAbility = null;
        }

        if (isComboActive)
        {
            if (_executor.IsAttacking || queuedAbility != null) return; //only start counting after current attack has been fully executed
            comboTimer += Time.deltaTime;
            if (comboTimer > comboWindow)
                ResetCombo();
        }
    }

    public void Initialize(PlayerEntity entity, WeaponSet weapon)
    {
        _playerEntity = entity;
        _executor = entity.AbilityExecutor;

        Abilities = weapon.WeaponAbilities;
        foreach (var ability in Abilities)
            ability.Initialize();

        SetWeaponComboData(weapon.ComboData);
        _isInitialized = true;
    }

    private void HandleInput()
    {
        if (!_playerEntity.CanUseAbilities) return;

        if (UserInput.Instance.PrimaryAttackPressed) TryUseComboOrAbility(0);
        if (UserInput.Instance.SecondaryAttackPressed) TryUseComboOrAbility(1);
        if (UserInput.Instance.Oath1Pressed) TryUseComboOrAbility(2);
        if (UserInput.Instance.Oath2Pressed) TryUseComboOrAbility(3);
        if (UserInput.Instance.Oath3Pressed) TryUseComboOrAbility(4);
        if (UserInput.Instance.Oath4Pressed) TryUseComboOrAbility(5);
    }

    private void TryUseComboOrAbility(int index)
    {
        if (activeCombos.ContainsKey(index))
        {
            var comboList = activeCombos[index];
            var abilityToUse = comboList[comboIndex];

            if (_executor.IsAttacking)
            {
                // queue combo step within comboWindow
                if (comboTimer <= comboWindow)
                {
                    queuedAbility = abilityToUse;
                    queuedIndex = index;
                }
            }
            else
            {
                if (_playerEntity.CanUseAbilities && _executor.TryExecuteAbility(abilityToUse))
                {
                    AdvanceCombo(comboList);
                }
            }
        }
        else if (index < Abilities.Count) //non combo 
        {
            var ability = Abilities[index];

            if (_executor.IsAttacking)
            {
                queuedAbility = ability;
                queuedIndex = index;

            }
            else
            {
                if (_playerEntity.CanUseAbilities)
                    _executor.TryExecuteAbility(ability);
            }
        }
    }

    private void AdvanceCombo(List<AbilityBase> comboList)
    {
        comboTimer = 0f;
        isComboActive = true;
        _playerEntity.Animator.SetFloat("ComboIndex", comboIndex);

        comboIndex++;
        if (comboIndex >= comboList.Count)
            comboIndex = 0;
    }

    private void ResetCombo()
    {
        comboIndex = 0;
        comboTimer = 0f;
        isComboActive = false;
        queuedAbility = null;
        _playerEntity.Animator.SetFloat("ComboIndex", comboIndex);
        Debug.Log("Reset Combo: " + comboIndex);
    }

    public void SetWeaponComboData(WeaponComboData comboData)
    {
        currentComboData = comboData;
        activeCombos.Clear();

        if (comboData == null) return;

        if (comboData.PrimaryCombo != null && comboData.PrimaryCombo.Count > 0)
            activeCombos[0] = comboData.PrimaryCombo;
        if (comboData.SecondaryCombo != null && comboData.SecondaryCombo.Count > 0)
            activeCombos[1] = comboData.SecondaryCombo;
        // Extend for more inputs if needed
    }
}
