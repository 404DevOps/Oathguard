using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class AuraManager : Singleton<AuraManager>
{
    private Dictionary<string, List<AuraInstance>> _activeAuras = new Dictionary<string, List<AuraInstance>>();

    private void OnEnable()
    {
        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
    }
    private void OnDisable()
    {
        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        if (_activeAuras == null)
            _activeAuras = new();
        if (!_activeAuras.ContainsKey(entity.Id)) // Prevent duplicate entries
            _activeAuras[entity.Id] = new List<AuraInstance>();
    }

    private void Update()
    {
        foreach (var kv in _activeAuras)
        {
            var list = kv.Value;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].IsExpired)
                {
                    list[i].Expire();
                    list.RemoveAt(i);
                }
            }
        }
    }

    public AuraInstance ApplyAura(EntityBase origin, EntityBase target, AuraBase newAura)
    {
        if (!_activeAuras.ContainsKey(target.Id))
            _activeAuras[target.Id] = new List<AuraInstance>();

        var auraList = _activeAuras[target.Id];

        // Handle uniqueness
        var existing = auraList.FirstOrDefault(a => a.Template.Id == newAura.Id);
        if (existing != null)
        {
            existing.Refresh();
            return existing;
        }
        else if (newAura is OathAura)
        {
            //clear oath if any.
            var currentOath = auraList.FirstOrDefault(a => a.Template is OathAura && a.Template.Id != newAura.Id);
            if (currentOath != null)
            {
                currentOath.Expire();
                auraList.Remove(currentOath);
            }
        }

        // Apply new
        var instance = new AuraInstance(newAura, origin, target);
        auraList.Add(instance);
        newAura.OnApply(instance);
        return instance;
    }
    public void CancelAuraById(string entityId, string auraId)
    {
        for (int i = 0; i < _activeAuras[entityId]?.Count; i++)
        {
            if (_activeAuras[entityId][i].Template.Id != auraId)
                continue;

            Debug.Log("Aura canceled");
            _activeAuras[entityId][i].Expire();
            _activeAuras[entityId].RemoveAt(i);

            GameEvents.OnAuraExpired.Invoke(new AuraExpiredEventArgs(entityId, auraId));
        }
    }
    public void ClearAllAuras()
    {
        foreach (var entityAuras in _activeAuras)
        {
            foreach (var auraInstance in entityAuras.Value)
            {
                auraInstance.Expire();
            }
        }
        _activeAuras.Clear();
    }

    public OathAura GetPlayerOathAura()
    {
        var player = EntityManager.Instance.Player;
        var oathAura = _activeAuras[player.Id].FirstOrDefault(a => a.Template.Type == AuraType.Oath);
        if (oathAura != default)
            return oathAura.Template as OathAura;

        return null;
    }
}