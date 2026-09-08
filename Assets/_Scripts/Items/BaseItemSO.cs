using UnityEngine;

public enum ItemType
{
    Consumable,
    Weapon
}

public abstract class BaseItemSO : ScriptableObject
{
    public Sprite itemSprite;
    public string itemName;
    public ItemType itemType;
    public int price;

    // Every item must write its own logic for this method!
    public abstract bool UseItem(Player player);
}
