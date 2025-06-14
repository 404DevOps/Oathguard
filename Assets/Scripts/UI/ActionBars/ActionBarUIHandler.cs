using System;
using UnityEngine;

public class ActionBarUIHandler : MonoBehaviour
{
    PlayerEntity _player;
    [SerializeField] GameObject _abilityDisplayPrefab;
    Transform _upperBar;
    Transform _lowerBar;

    void Awake()
    {
        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        _upperBar = transform.Find("UpperBar");
        _lowerBar = transform.Find("LowerBar");
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        if (entity.GetType() != typeof(PlayerEntity)) return;

        _player = entity as PlayerEntity;
        ClearAbilities();
        BuildActionBar();

        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
        GameEvents.OnAbilitiesChanged.AddListener(OnAbilitiesChanged);
    }

    void OnDisable()
    {
        GameEvents.OnAbilitiesChanged.RemoveListener(OnAbilitiesChanged);
    }

    private void OnAbilitiesChanged(EntityBase entity)
    {
        if (_player.Id != entity.Id)
            return;

        ClearAbilities();
        BuildActionBar();
    }

    void ClearAbilities()
    {
        if (_upperBar.childCount > 0)
            _upperBar.DeleteChildren();
        if (_lowerBar.childCount > 0)
            _lowerBar.DeleteChildren();
    }

    void BuildActionBar()
    {
        if (_player != null)
        {
            var executor = _player.GetComponent<PlayerAbilityExecutor>();
            int abilityIndex = 0;
            var spawnTransform = _upperBar;
            foreach (var ability in executor.Abilities)
            {
                var abilityDisplayGo = Instantiate(_abilityDisplayPrefab, spawnTransform);
                var abilityDisplayScript = abilityDisplayGo.GetComponent<AbilityDisplay>();
                abilityDisplayScript.Initialize(abilityIndex, ability, _player);
                abilityIndex++;

                if (abilityIndex == 2)
                    spawnTransform = _lowerBar;
            }
        }
    }
}