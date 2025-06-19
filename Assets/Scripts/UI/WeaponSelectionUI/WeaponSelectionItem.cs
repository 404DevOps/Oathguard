using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSelectionItem : MonoBehaviour, IPointerClickHandler
{
    public Image Image;
    public Image SelectedBorder;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Stats;
    public TextMeshProUGUI Description;

    [HideInInspector]
    public WeaponSet WeaponSet;

    public Action<WeaponSet> OnClick;

    public void Initialize(WeaponSet set)
    {
        WeaponSet = set;

        Image.sprite = WeaponSet.Image;
        Name.text = WeaponSet.Name;
        Stats.text = "Base Damage: " + WeaponSet.BaseDamage.ToString() + "\n"
                    + "Critical Hit Chance: " + WeaponSet.CritChance + "%";
        Description.text = WeaponSet.Description;

        ToggleSelected(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectedBorder.gameObject.SetActive(true);
        OnClick(WeaponSet);
    }

    public void ToggleSelected(bool selected)
    {
        if (SelectedBorder.enabled != selected)
            SelectedBorder.enabled = selected;
    }
}
