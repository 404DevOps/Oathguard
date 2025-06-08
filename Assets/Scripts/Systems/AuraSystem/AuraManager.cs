using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class AuraManager : Singleton<AuraManager>
{
    private Dictionary<string, List<AuraInfo>> _auras = new Dictionary<string, List<AuraInfo>>();

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
        if (!_auras.ContainsKey(entity.Id)) // Prevent duplicate entries
            _auras[entity.Id] = new List<AuraInfo>();
    }

    private void Update()
    {
        foreach (var entry in _auras)
        {
            if (entry.Value != null)
            {
                for (int i = entry.Value.Count - 1; i >= 0; i--) // Iterate backward
                {
                    if (entry.Value[i].StartTime + entry.Value[i].Aura.Duration - Time.time <= 0)
                    {
                        Debug.Log("Aura expired.");
                        entry.Value[i].Aura.Expire();
                        GameEvents.OnAuraExpired.Invoke(new AuraExpiredEventArgs(entry.Key, entry.Value[i].Aura.Id));
                        entry.Value.RemoveAt(i);
                    }
                }
            }
        }
    }
    public bool AddOrRefreshAura(string entityId, AuraBase aura)
    {
        if (!_auras.ContainsKey(entityId))
            _auras[entityId] = new List<AuraInfo>(); // Ensure list exists

        var entityAuras = _auras[entityId];

        if (aura.Unique)
        {
            var existingAura = entityAuras.FirstOrDefault(a => a.Aura.Id == aura.Id);
            if (existingAura != null)
            {
                Debug.Log("Refreshed Aura.");
                existingAura.StartTime = Time.time; // Refresh aura duration instead of adding new when aura is unique
                GameEvents.OnAuraRefreshed.Invoke(new AuraRefreshedEventArgs(entityId, existingAura));
                return false;
            }
        }

        Debug.Log("Added Aura.");
        var info = new AuraInfo(aura, Time.time);
        entityAuras.Add(info);
        GameEvents.OnAuraApplied.Invoke(new AuraAppliedEventArgs(entityId, info));
        return true;
    }
    public void CancelAuraById(string entityId, string auraId)
    {
        for (int i = 0; i < _auras[entityId]?.Count; i++)
        {
            if (_auras[entityId][i].Aura.Id != auraId)
                continue;

            Debug.Log("Aura canceled");
            _auras[entityId][i].Aura.Expire();
            _auras[entityId].RemoveAt(i);

            GameEvents.OnAuraExpired.Invoke(new AuraExpiredEventArgs(entityId, auraId));
        }
    }
    public void ClearAllAuras()
    {
        foreach (var entityAuras in _auras)
        {
            foreach (var auraInfo in entityAuras.Value)
            {
                auraInfo.Aura.Expire();
            }
        }
        _auras.Clear();
    }

    public SealType GetPlayerSeal()
    {
        var player = EntityManager.Instance.Player;
        var sealAura = _auras[player.Id].FirstOrDefault(a => a.Aura.Type == AuraType.Seal);
        if(sealAura != default)
        {
            var sA = sealAura.Aura as SealAura;
            return sA.SealType;
        }
        return SealType.None;
    }
}