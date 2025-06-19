using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D AimCursor;

    void Start()
    {
        GameEvents.OnWeaponSelected.AddListener(OnWeaponSelected);
    }

    private void OnWeaponSelected(WeaponSet set)
    {
        Vector2 hotspot = new Vector2(AimCursor.width / 2f, AimCursor.height / 2f);
        Cursor.SetCursor(AimCursor, hotspot, CursorMode.Auto);
    }
}
