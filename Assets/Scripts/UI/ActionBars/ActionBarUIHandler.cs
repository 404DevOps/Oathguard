using UnityEngine;

public class ActionBarUIHandler : MonoBehaviour
{
    PlayerEntity _player;
    [SerializeField] GameObject _abilityDisplayPrefab;
    public Transform AbilityContainer;

    void Awake()
    {
        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        AbilityContainer = transform.Find("AbilityContainer");
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
        if (AbilityContainer.childCount > 0)
            AbilityContainer.Clear();
    }

    void BuildActionBar()
    {
        if (_player != null)
        {
            var abilityController = _player.GetComponent<PlayerAbilityController>();
            int abilityIndex = 0;
            foreach (var ability in abilityController.Abilities)
            {
                var abilityDisplayGo = Instantiate(_abilityDisplayPrefab, AbilityContainer);
                var abilityDisplayScript = abilityDisplayGo.GetComponent<AbilityDisplay>();
                abilityDisplayScript.Initialize(abilityIndex, ability, _player);
                abilityIndex++;
            }
        }
    }
}