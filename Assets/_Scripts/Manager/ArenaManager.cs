using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine; 

[System.Serializable]
public struct EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
}

[System.Serializable]
public class Wave
{
    public string waveName = "Wave 1";
    public List<EnemySpawnInfo> enemiesToSpawn;
}

public class ArenaManager : MonoBehaviour
{
    [Header("Arena Boundaries")]
    [SerializeField] private GameObject[] invisibleWalls;
    [SerializeField] private Collider2D arenaCameraBounds;

    [Header("Wave Settings")]
    [SerializeField] private List<Wave> waves;

    private int currentWaveIndex = 0;
    private int activeEnemyCount = 0;
    private bool arenaStarted = false;

    private CinemachineConfiner2D mainCameraConfiner;
    private Collider2D defaultCameraBounds;

    private void Start()
    {
        SetWallsActive(false);
        mainCameraConfiner = FindAnyObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!arenaStarted && collision.CompareTag("Player"))
        {
            StartArena();
        }
    }

    private void StartArena()
    {
        arenaStarted = true;

        if (mainCameraConfiner != null && arenaCameraBounds != null)
        {
            defaultCameraBounds = mainCameraConfiner.BoundingShape2D;
            mainCameraConfiner.BoundingShape2D = arenaCameraBounds;
            mainCameraConfiner.InvalidateBoundingShapeCache();
        }

        SetWallsActive(true);
        SpawnNextWave();
    }

    private void SpawnNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            EndArena();
            return;
        }

        Wave currentWave = waves[currentWaveIndex];
        activeEnemyCount = currentWave.enemiesToSpawn.Count;

        foreach (var spawnInfo in currentWave.enemiesToSpawn)
        {
            GameObject spawnedEnemy = Instantiate(spawnInfo.enemyPrefab, spawnInfo.spawnPoint.position, Quaternion.identity);

            Health enemyHealth = spawnedEnemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += OnEnemyDefeated;
            }
        }

        currentWaveIndex++;
    }

    private void OnEnemyDefeated()
    {
        activeEnemyCount--;

        if (activeEnemyCount <= 0)
        {
            SpawnNextWave();
        }
    }

    private void EndArena()
    {
        Debug.Log("Arena Cleared! Unlocking area.");

        if (mainCameraConfiner != null && defaultCameraBounds != null)
        {
            mainCameraConfiner.BoundingShape2D = defaultCameraBounds;
            mainCameraConfiner.InvalidateBoundingShapeCache();
        }

        SetWallsActive(false);
        GetComponent<Collider2D>().enabled = false;
    }

    private void SetWallsActive(bool isActive)
    {
        foreach (var wall in invisibleWalls)
        {
            if (wall != null) wall.SetActive(isActive);
        }
    }
}