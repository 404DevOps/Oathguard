using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform Canvas;
    public GameObject WeaponSelectionUI;

    private GameObject _selectionUIInstance;
    void Start()
    {
        GameEvents.OnWeaponSelected.AddListener(OnWeaponSelected);
        HUDToggle.Instance.Toggle(false);
        Time.timeScale = 0f;
        _selectionUIInstance = Instantiate(WeaponSelectionUI, Canvas);
    }
    public void SetWeapon(WeaponSet weapon)
    {
        Destroy(_selectionUIInstance);

        GameEvents.OnWeaponSelected?.Invoke(weapon);
    }

    private void OnWeaponSelected(WeaponSet set)
    {
        //select oath menu first? 
        Time.timeScale = 1f;
        HUDToggle.Instance.Toggle(true);
        GameEvents.OnGameStarted.Invoke();
    }
}
