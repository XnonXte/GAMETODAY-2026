using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ShopSlot : MonoBehaviour, IInteractable
{
    private BaseItemSO slotItem;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupSlot(BaseItemSO itemToSell)
    {
        slotItem = itemToSell;

        if (slotItem != null && slotItem.itemSprite != null)
        {
            spriteRenderer.sprite = slotItem.itemSprite;
            spriteRenderer.enabled = true;
        }
        else
        {
            ClearSlot();
        }
    }

    public void Interact(Player player)
    {
        if (slotItem == null) return;

        if (GameResource.TrySpendGold(slotItem.price))
        {
            Debug.Log($"Purchased {slotItem.itemName} for {slotItem.price} gold!");

            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(slotItem);
            }

            ClearSlot(); 
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    private void ClearSlot()
    {
        slotItem = null;
        spriteRenderer.sprite = null;
        spriteRenderer.enabled = false;
    }
}