using UnityEngine;
using System;

public static class EventHandler
{
    //battle
    public static event Action OnBattleStart;
    public static event Action OnBattleEnd;
    public static event Action OnEnemyDefeated;
    public static event Action OnGoldAmountChanged;
    public static event Action<Sprite, Sprite> OnInventoryUpdated;
    public static event Action<float, float> OnPlayerHealthChanged;
    public static event Action<float, float> OnPayloadHealthChanged;

    public static void WhenBattleStart() => OnBattleStart?.Invoke();
    public static void WhenBattleEnd() => OnBattleEnd?.Invoke();
    public static void WhenEnemyDefeated() => OnEnemyDefeated?.Invoke();
    public static void WhenGoldAmountChanged() => OnGoldAmountChanged?.Invoke();
    public static void WhenInventoryUpdated(Sprite consumeableSprite, Sprite weaponSprite) => OnInventoryUpdated?.Invoke(consumeableSprite, weaponSprite);
    public static void WhenPlayerHealthChanged(float currentHp, float maxHp) => OnPlayerHealthChanged?.Invoke(currentHp, maxHp);
    public static void WhenPayloadHealthChanged(float currentHp, float maxHp) => OnPayloadHealthChanged?.Invoke(currentHp, maxHp);

}