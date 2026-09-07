using UnityEngine;

[CreateAssetMenu(menuName = "Consumeables/Power Potion")]
public class PowerPotion : BaseItemSO
{
    [Header("Boost Settings")]
    public float damageMultiplier = 2f;
    public float duration = 10f;

    public override bool UseItem(Player player)
    {
        player.ApplyPowerBoost(damageMultiplier, duration);
        return true;
    }
}
