using JetBrains.Annotations;
using UnityEngine;

public class HUDToggle : Singleton<HUDToggle>
{
    public GameObject UIContainer;

    public void Toggle(bool isOn)
    {
        if (UIContainer.activeSelf != isOn)
            UIContainer.SetActive(isOn);
    }
}
