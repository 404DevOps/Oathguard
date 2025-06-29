using System.Collections.Generic;

[System.Serializable]
public class MinionWave
{
    public string waveName = "Wave";
    public float preWaveDelay = 2f;
    public List<MinionWaveEntry> entries;
    public float postWaveDelay = 2f;
}