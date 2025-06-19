using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionUIHandler : MonoBehaviour
{
    
    public GameObject SelectionItemPrefab;
    public Transform SelectionItemContainer;
    public Button ConfirmButton;

    private List<WeaponSelectionItem> _selectionItems;
    private WeaponSet selectedWeaponSet;

    void Start()
    {
        _selectionItems = new List<WeaponSelectionItem>();
        ConfirmButton.onClick.AddListener(OnConfirmClicked);
        var collection = WeaponCollection.Instance().GetAllWeapons();

        BuildGrid(collection);
    }

    private void OnConfirmClicked()
    {
        GameManager.Instance.SetWeapon(selectedWeaponSet);
    }

    private void BuildGrid(List<WeaponSet> collection)
    {
        SelectionItemContainer.DeleteChildren();

        var first = true;
        foreach (var set in collection)
        {
            var item = Instantiate(SelectionItemPrefab, SelectionItemContainer);
            var selectionItem = item.GetComponent<WeaponSelectionItem>();
            selectionItem.Initialize(set);
            selectionItem.OnClick += SelectWeaponset;
            if (first)
            {
                selectionItem.ToggleSelected(true);
                selectedWeaponSet = set;
                first = false;
            }
            _selectionItems.Add(selectionItem);
        }
    }

    private void SelectWeaponset(WeaponSet weaponSet)
    {
        selectedWeaponSet = weaponSet;
        if (selectedWeaponSet != null)
        {
            foreach (var item in _selectionItems)
            {
                item.ToggleSelected(item.WeaponSet == selectedWeaponSet);
            }
        }

        ConfirmButton.enabled = true;
    }
}
