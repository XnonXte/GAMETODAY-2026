using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private BaseItemSO currentConsumable;
    private BaseItemSO currentWeapon;
    private Player player;
    public BaseItemSO CurrentConsumable => currentConsumable;
    public BaseItemSO CurrentWeapon => currentWeapon;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        player = GetComponent<Player>();
    }

    private void Update()
    {
        //v
        if (InputManager.Instance.GetPlayerUseConsume() && currentConsumable != null)
        {
            if (currentConsumable.UseItem(player))
            {
                currentConsumable = null;
                UpdateUI();
            }
        }

        if (InputManager.Instance.GetPlayerUseSkill() && currentWeapon != null)
        {
            if (currentWeapon.UseItem(player))
            {
                currentWeapon = null;
                UpdateUI();
            }
        }
    }

    private void Start()
    {
        if (GameSessionManager.Instance != null)
        {
            if (GameSessionManager.Instance.savedConsumable != null)
            {
                AddItem(GameSessionManager.Instance.savedConsumable);
            }

            if (GameSessionManager.Instance.savedWeapon != null)
            {
                AddItem(GameSessionManager.Instance.savedWeapon);
            }
        }
    }

    // NEW: Now returns a BaseItemSO (the replaced item, or null if the slot was empty)
    public BaseItemSO AddItem(BaseItemSO newItem)
    {
        BaseItemSO replacedItem = null;

        if (newItem.itemType == ItemType.Consumable)
        {
            replacedItem = currentConsumable;
            currentConsumable = newItem;
        }
        else if (newItem.itemType == ItemType.Weapon)
        {
            replacedItem = currentWeapon;
            currentWeapon = newItem;
        }

        UpdateUI();

        return replacedItem; // Send the old item back to the shop!
    }

    private void UpdateUI()
    {
        Sprite consumableSprite = currentConsumable != null ? currentConsumable.itemSprite : null;
        Sprite weaponSprite = currentWeapon != null ? currentWeapon.itemSprite : null;

        EventHandler.WhenInventoryUpdated(consumableSprite, weaponSprite);
    }
}