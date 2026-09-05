//using UnityEngine;
//using System.Collections.Generic;

//public class SpawnerController : MonoBehaviour
//{
//    [SerializeField] private List<SOBaseEnemy> enemiesData = new();
//    [SerializeField] private GameObject enemyPrefab;
//    [SerializeField] private Transform[] spawnPosition;

//    private void OnEnable()
//    {
//        EventHandler.OnBattleStart += SpawnEnemy;
//    }

//    private void OnDisable()
//    {
//        EventHandler.OnBattleStart -= SpawnEnemy;
//    }


//    [ContextMenu("Test Spawn Enemy")]
//    public void SpawnEnemy()
//    {
//        for (int i = 0; i < enemiesData.Count; i++)
//        {
//            int randomIndex = Random.Range(0, spawnPosition.Length);

//            GameObject instantiatedEnemy = Instantiate(enemyPrefab, spawnPosition[randomIndex]);
//            EnemyController enemyController = instantiatedEnemy.GetComponent<EnemyController>();
            
//            enemyController.InitEnemyData(enemiesData[i]);
//        }
//    }

//    public int GetEnemyCount()
//    {
//        return enemiesData.Count;
//    }
//}
