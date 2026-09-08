using UnityEngine;

[CreateAssetMenu(menuName = "Consumeables/Heal Potion")]
public class HealPotion : BaseItemSO
{
    [Header("Heal Settings")]
    public float healAmount = 40f;

    public override bool UseItem(Player player)
    {
        player.ApplyHeal(healAmount);
        return true;
    }
}