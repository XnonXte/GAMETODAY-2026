using UnityEngine;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private List<SOBaseEnemy> enemiesData = new();
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform testPosition;

    public static SpawnerManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    [ContextMenu("Test Spawn Enemy")]
    public void SpawnEnemy()
    {
        for (int i = 0; i < enemiesData.Count; i++)
        {
            GameObject instantiatedEnemy = Instantiate(enemyPrefab, testPosition, true);
            EnemyController enemyController = instantiatedEnemy.GetComponent<EnemyController>();
            enemyController.InitEnemyData(enemiesData[0]);
            enemiesData.RemoveAt(0);
        }
    }
}
