using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D AimCursor;

    void Start()
    {
        GameEvents.OnOathSelected.AddListener(OnOathSelected);
    }

    private void OnOathSelected(OathAura aura)
    {
        Vector2 hotspot = new Vector2(AimCursor.width / 2f, AimCursor.height / 2f);
        Cursor.SetCursor(AimCursor, hotspot, CursorMode.Auto);
    }
}
