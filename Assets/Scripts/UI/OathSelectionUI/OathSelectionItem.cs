using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OathSelectionItem : MonoBehaviour, IPointerClickHandler
{
    public Image Icon;
    public Image SelectedBorder;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI FlavorText;

    [HideInInspector]
    public OathAura Oath;

    public Action<OathAura> OnClick;

    public void Initialize(OathAura oath)
    {
        Oath = oath;

        Icon.sprite = Oath.Icon;
        Name.text = Oath.Name;
        Description.text = Oath.Description;
        FlavorText.text = Oath.FlavorText;

        ToggleSelected(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectedBorder.gameObject.SetActive(true);
        OnClick(Oath);
    }

    public void ToggleSelected(bool selected)
    {
        if (SelectedBorder.enabled != selected)
            SelectedBorder.enabled = selected;
    }
}
