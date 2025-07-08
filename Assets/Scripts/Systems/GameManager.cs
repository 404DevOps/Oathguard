using System;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool SkipInitialization;
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
        if (SkipInitialization)
        {
            SkipInit();
            return;
        }

        GameEvents.OnPerkSelected.AddListener(OnPerkSelected);

        UIEvents.OnInGameMenuOpen?.Invoke();
        HUDToggle.Instance.Toggle(false);
        Time.timeScale = 0f;
        _selectionUIInstance = Instantiate(WeaponSelectionUI, Canvas);
    }

    private void SkipInit()
    {
        HUDToggle.Instance.Toggle(true);
        Time.timeScale = 1f;

        // Select a default weapon and apply it
        var weapon = WeaponCollection.Instance().GetAllWeapons().FirstOrDefault();
        if (weapon != null)
        {
            SelectedWeaponSet = weapon;
            GameEvents.OnWeaponSelected?.Invoke(weapon);
        }

        // Select a default oath and apply it
        var oath = OathCollection.Instance().GetAllOaths().FirstOrDefault(); // Replace with actual source
        if (oath != null)
        {
            SelectedOath = oath;
            var player = EntityManager.Instance.Player;
            AuraManager.Instance.ApplyAura(player, player, oath);
            GameEvents.OnOathSelected?.Invoke(oath);
        }

        // Start the round
        StartRound();
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
        UIEvents.OnInGameMenuClose?.Invoke();
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
