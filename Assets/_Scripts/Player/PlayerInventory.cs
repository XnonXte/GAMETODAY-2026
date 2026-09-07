using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private BaseItemSO currentConsumable;
    private BaseItemSO currentWeapon;
    private Player player; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        player = GetComponent<Player>();
    }

    private void Update()
    {
        // Note: You should move these inputs to your InputManager later!
        // Pressing 'Q' for Consumable
        if (Input.GetKeyDown(KeyCode.Q) && currentConsumable != null)
        {
            if (currentConsumable.UseItem(player))
            {
                currentConsumable = null; // Remove it from inventory if successfully used
                UpdateUI();
            }
        }

        // Pressing 'E' for Weapon
        if (Input.GetKeyDown(KeyCode.E) && currentWeapon != null)
        {
            if (currentWeapon.UseItem(player))
            {
                currentWeapon = null; // Remove it from inventory if successfully used
                UpdateUI();
            }
        }
    }

    public void AddItem(BaseItemSO newItem)
    {
        if (newItem.itemType == ItemType.Consumable) currentConsumable = newItem;
        else if (newItem.itemType == ItemType.Weapon) currentWeapon = newItem;

        UpdateUI();
    }

    private void UpdateUI()
    {
        Sprite consumableSprite = currentConsumable != null ? currentConsumable.itemSprite : null;
        Sprite weaponSprite = currentWeapon != null ? currentWeapon.itemSprite : null;

        EventHandler.WhenInventoryUpdated(consumableSprite, weaponSprite);
    }
}