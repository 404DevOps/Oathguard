[System.Serializable]
public class MinionWaveEntry
{
    public EntityType MinionType;             // Matches ID in MinionSpawner
    public int Count = 5;               // Number of minions
    public float SpawnInterval = 0.5f;  // Delay between each spawn of the wave
}