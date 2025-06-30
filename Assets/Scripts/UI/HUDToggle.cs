using JetBrains.Annotations;
using UnityEngine;

public class HUDToggle : Singleton<HUDToggle>
{
    public GameObject UIContainer;
    private CanvasGroup _canvasGroup;

    protected override void Awake()
    {
        base.Awake();

        UIContainer.SetActive(true); // Ensure CanvasGroup gets initialized
        _canvasGroup = UIContainer.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void Toggle(bool isOn)
    {
        _canvasGroup.alpha = isOn ? 1f : 0f;
        _canvasGroup.interactable = isOn;
        _canvasGroup.blocksRaycasts = isOn;
    }
}