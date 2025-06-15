using System;
using System.Collections.Generic;

public class EntityManager : Singleton<EntityManager>
{
    private Dictionary<string, EntityBase> _entities = new();
    public IReadOnlyCollection<EntityBase> Entities => _entities.Values;

    private PlayerEntity _player;
    public PlayerEntity Player
    {
        get
        {
            if (_player != null)
                return _player;

            _player = FindAnyObjectByType<PlayerEntity>();
            return _player;
        }
        set
        {
            _player = value;
        }
    }

    public EntityBase Get(string entityId)
    {
        if (!_entities.ContainsKey(entityId))
            throw new ArgumentOutOfRangeException("EntityManager did not find an Entity with Id " + entityId);

        return _entities[entityId];
    }

    private void OnEnable()
    {
        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        GameEvents.OnEntityDestroyed.AddListener(OnEntityDied);
    }

    private void OnDisable()
    {
        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
        GameEvents.OnEntityDestroyed.RemoveListener(OnEntityDied);
    }

    private void OnEntityDied(string entityId)
    {
        _entities.Remove(entityId);
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        _entities[entity.Id] = entity;
        if (entity is PlayerEntity)
            Player = entity as PlayerEntity;
    }

    public void ClearAll()
    {
        _player = null;
        _entities.Clear();
    }
}