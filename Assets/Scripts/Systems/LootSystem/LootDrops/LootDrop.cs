using UnityEngine;

public abstract class LootDrop : ScriptableObject
{
    public abstract void Spawn(Vector3 position);
}