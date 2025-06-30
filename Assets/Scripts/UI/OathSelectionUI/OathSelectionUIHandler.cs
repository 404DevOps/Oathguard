using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OathSelectionUIHandler : MonoBehaviour
{
    
    public GameObject SelectionItemPrefab;
    public Transform SelectionItemContainer;
    public Button ConfirmButton;

    private List<OathSelectionItem> _selectionItems;
    private OathAura selectedOath;

    void Start()
    {
        _selectionItems = new List<OathSelectionItem>();
        ConfirmButton.onClick.AddListener(OnConfirmClicked);
        var collection = OathCollection.Instance().GetRandomOths(3);

        BuildGrid(collection);
    }

    private void OnConfirmClicked()
    {
        GameManager.Instance.SetOath(selectedOath);
    }

    private void BuildGrid(List<OathAura> oathOptions)
    {
        SelectionItemContainer.Clear();

        var first = true;
        foreach (var oath in oathOptions)
        {
            var item = Instantiate(SelectionItemPrefab, SelectionItemContainer);
            var selectionItem = item.GetComponent<OathSelectionItem>();
            selectionItem.Initialize(oath);
            selectionItem.OnClick += SelectOath;
            if (first)
            {
                selectionItem.ToggleSelected(true);
                selectedOath = oath;
                first = false;
            }
            _selectionItems.Add(selectionItem);
        }
    }

    private void SelectOath(OathAura weaponSet)
    {
        selectedOath = weaponSet;
        if (selectedOath != null)
        {
            foreach (var item in _selectionItems)
            {
                item.ToggleSelected(item.Oath == selectedOath);
            }
        }

        ConfirmButton.enabled = true;
    }
}
