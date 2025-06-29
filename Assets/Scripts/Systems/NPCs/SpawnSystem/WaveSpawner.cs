using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WaveSpawner : Singleton<WaveSpawner>
{
    [SerializeField] private MinionSpawner minionSpawner;
    [SerializeField] private List<MinionWave> waves;
    [SerializeField] private List<Transform> spawnPoints;

    private List<NPCEntity> activeEntities;
    private List<NPCEntity> entitiesToRemove;

    private bool isSpawning = false;
    private bool waveOngoing = false;

    public Action OnWaveEnded;


    private void Start()
    {
        activeEntities = new List<NPCEntity>();
        entitiesToRemove = new List<NPCEntity>();
    }

    public void SpawnWave(int wave)
    {
        activeEntities.Clear();
        if (wave >= waves.Count) return;
        var minionwave = waves[wave];
        StartCoroutine(SpawnWave(minionwave));

    }

    private IEnumerator CheckForWaveEnd()
    {
        while(waveOngoing)
        {
            //find dead entities
            foreach (var minion in activeEntities)
            {
                if (minion.IsDead)
                {
                    minionSpawner.Despawn(minion.Type, minion.gameObject);
                    entitiesToRemove.Add((minion));
                }
            }
            //clean up dead entities
            foreach (var entity in entitiesToRemove)
            {
                activeEntities.Remove(entity);
            }
            entitiesToRemove.Clear();

            if (activeEntities.Count == 0)
            {
                waveOngoing = false;
                OnWaveEnded?.Invoke();
            }
            yield return null;
        }



    }

    private IEnumerator SpawnWave(MinionWave wave)
    {
        isSpawning = true;
        Debug.Log($"Starting Wave: {wave.waveName}");

        foreach (var entry in wave.entries)
        {
            for (int i = 0; i < entry.Count; i++)
            {
                Transform spawnPoint = GetRandomSpawnPoint();
                var entities = minionSpawner.Spawn(entry.MinionType, spawnPoint.position, Quaternion.identity);
                activeEntities.Add(entities);
                yield return new WaitForSeconds(entry.SpawnInterval);
            }
        }

        yield return new WaitForSeconds(wave.postWaveDelay);

        isSpawning = false;
        waveOngoing = true;
        StartCoroutine(CheckForWaveEnd());
        Debug.Log("Wave Spawn complete, ongoing.");
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
            return transform;

        return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
    }
}
