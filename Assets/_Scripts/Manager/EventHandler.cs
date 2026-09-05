using UnityEngine;
using System;

public static class EventHandler
{
    //battle
    public static event Action OnBattleStart;
    public static event Action OnBattleEnd;
    public static event Action OnEnemyDefeated;

    public static void WhenBattleStart() => OnBattleStart?.Invoke();
    public static void WhenBattleEnd() => OnBattleEnd?.Invoke();
    public static void WhenEnemyDefeated() => OnEnemyDefeated?.Invoke();
}