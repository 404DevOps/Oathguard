using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OathHUDHandler : MonoBehaviour
{
    private OathAura OathAura;
    private PlayerEntity _player;
    private EntityExperience _playerXP;

    [SerializeField] private ExperienceBarUIHandler _xpBar;
    [SerializeField] private Image OathImage;
    [SerializeField] private TextMeshProUGUI OathText;

    private void OnEnable()
    {
        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        GameEvents.OnEntityXPChanged.AddListener(OnXPChanged);
        GameEvents.OnEntityLeveledUp.AddListener(OnLeveledUp);
        GameEvents.OnOathSelected.AddListener(OnOathSelected);
    }
    private void OnDisable()
    {
        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
        GameEvents.OnEntityXPChanged.RemoveListener(OnXPChanged);
        GameEvents.OnEntityLeveledUp.RemoveListener(OnLeveledUp);
        GameEvents.OnOathSelected.RemoveListener(OnOathSelected);
    }

    private void OnOathSelected(OathAura aura)
    {
        SetOath(aura);
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        if (entity is not PlayerEntity) return;
        _player = (PlayerEntity)entity;

        _playerXP = _player.Experience;
        _xpBar.InitializeBar(_playerXP.CurrentXP, _playerXP.MaxXP, _playerXP.CurrentLevel);
    }

    private void OnLeveledUp(PlayerEntity entity)
    {
        var xp = entity.Experience;
        StartCoroutine(_xpBar.SetNewValue(xp.CurrentXP, xp.MaxXP, entity.Experience.CurrentLevel, true));
    }
    private void OnXPChanged(XPChangedEventArgs args)
    {
        if (_player == null || _player.Id != args.Entity.Id)
            return;
        StartCoroutine(_xpBar.SetNewValue(args.CurrentXP, args.MaxXP, args.Entity.Experience.CurrentLevel, false));
    }
    private void SetOath(OathAura aura)
    {
        OathAura = aura;
        OathImage.sprite = OathAura.Icon;
    }
}
