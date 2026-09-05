//using UnityEngine;

//public class BattleZoneController : MonoBehaviour
//{
//    [SerializeField] private SpawnerController spawner;
//    [SerializeField] private Collider2D leftBorder;
//    [SerializeField] private Collider2D rightBorder;
//    private int remainingEnemies;
//    private bool isBattleStarted = false;

//    private void OnEnable()
//    {
//        EventHandler.OnBattleStart += StartBattle;
//        EventHandler.OnEnemyDefeated += HandleEnemyDeath;
//    }

//    private void OnDisable()
//    {
//        EventHandler.OnBattleStart -= StartBattle;
//        EventHandler.OnEnemyDefeated -= HandleEnemyDeath;
//    }

//    private void Start()
//    {
//        DisableCollider();
//    }

//    private void StartBattle()
//    {
//        if (isBattleStarted) return;

//        EnableCollider();
//        remainingEnemies = spawner.GetEnemyCount();
//    }    

//    private void EndBattle()
//    {
//        DisableCollider();
//        EventHandler.WhenBattleEnd();
//    }

//    private void HandleEnemyDeath()
//    {
//        remainingEnemies--;

//        if (remainingEnemies <= 0) EndBattle();
//    }

//    private void EnableCollider()
//    {
//        leftBorder.enabled = true;
//        rightBorder.enabled = true;
//    }

//    private void DisableCollider()
//    {
//        leftBorder.enabled = false;
//        rightBorder.enabled = false;
//    }
//}
