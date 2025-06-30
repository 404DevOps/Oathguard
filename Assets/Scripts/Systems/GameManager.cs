using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int NextWave { get; private set; }
    public Transform Canvas;
    public GameObject WeaponSelectionUI;
    public GameObject OathSelectionUI;
    public GameObject UpgradeSelectionUI;

    public OathAura SelectedOath;
    public WeaponSet SelectedWeaponSet;

    private GameObject _selectionUIInstance;
    void Start()
    {
        GameEvents.OnPerkSelected.AddListener(OnPerkSelected);

        HUDToggle.Instance.Toggle(false);
        Time.timeScale = 0f;
        _selectionUIInstance = Instantiate(WeaponSelectionUI, Canvas);
    }

    private void OnPerkSelected()
    {
        throw new NotImplementedException();
    }

    public void SetOath(OathAura selectedOath)
    {
        SelectedOath = selectedOath; 
        var player = EntityManager.Instance.Player;
        AuraManager.Instance.ApplyAura(player, player, selectedOath);
        Destroy(_selectionUIInstance);
        GameEvents.OnOathSelected?.Invoke(selectedOath);
        StartRound();
    }

    public void SetWeapon(WeaponSet weapon)
    {
        SelectedWeaponSet = weapon;
        Destroy(_selectionUIInstance);
        GameEvents.OnWeaponSelected?.Invoke(weapon);

        //show oath selection
        _selectionUIInstance = Instantiate(OathSelectionUI, Canvas);
    }

    private void StartRound()
    {
        Time.timeScale = 1f;
        HUDToggle.Instance.Toggle(true);
        GameEvents.OnRoundStarted.Invoke();

        SpawnNextWave();
        WaveSpawner.Instance.OnWaveEnded += SpawnNextWave;
    }

    private void SpawnNextWave()
    {
        WaveSpawner.Instance.SpawnWave(NextWave);
        WaveDisplayUI.Instance.Show(NextWave);
        NextWave++;
    }
}
