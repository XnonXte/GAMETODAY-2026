using UnityEngine;

[CreateAssetMenu(menuName = "Consumeables/Speed Potion")]
public class SpeedPotion : BaseItemSO
{
    [Header("Boost Settings")]
    public float speedBoost = 3f;
    public float duration = 5f; 

    public override bool UseItem(Player player)
    {
        player.ApplySpeedBoost(speedBoost, duration);
        return true;
    }
}