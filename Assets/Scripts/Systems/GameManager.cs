using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform Canvas;
    public GameObject WeaponSelectionUI;

    private GameObject _selectionUIInstance;
    void Start()
    {
        Time.timeScale = 0f;
        _selectionUIInstance = Instantiate(WeaponSelectionUI, Canvas);
    }

    public void SetWeapon(WeaponSet weapon)
    {
        Destroy(_selectionUIInstance);
        Time.timeScale = 1f;
        GameEvents.OnWeaponSelected?.Invoke(weapon);
    }
}
