using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class ShopSlot : MonoBehaviour, IInteractable
{
    private BaseItemSO slotItem;
    private int currentPrice; // Tracks the price specifically for this slot

    [SerializeField] private SpriteRenderer itemSpriteRenderer;
    [SerializeField] private TMP_Text priceText;

    // NEW: Added an optional overridePrice parameter (defaults to -1)
    public void SetupSlot(BaseItemSO itemToSell, int overridePrice = -1)
    {
        slotItem = itemToSell;

        if (slotItem != null)
        {
            // If we pass in 0 (for a swapped item), use it. Otherwise, use the item's standard price.
            currentPrice = overridePrice >= 0 ? overridePrice : slotItem.price;

            if (itemSpriteRenderer != null && slotItem.itemSprite != null)
            {
                itemSpriteRenderer.sprite = slotItem.itemSprite;
                itemSpriteRenderer.enabled = true;
            }

            if (priceText != null)
            {
                priceText.text = currentPrice == 0 ? "0" : currentPrice.ToString();
                priceText.gameObject.SetActive(true);
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void Interact(Player player)
    {
        if (slotItem == null) return;

        // Use currentPrice instead of slotItem.price
        if (GameResource.TrySpendGold(currentPrice))
        {
            Debug.Log($"Purchased {slotItem.itemName} for {currentPrice} gold!");

            AudioManager.Instance.PlayAudio(AudioManager.Instance.SFX_Purchase);

            BaseItemSO replacedItem = null;

            if (player != null)
            {
                PlayerInventory inventory = player.GetComponent<PlayerInventory>();
                if (inventory != null) replacedItem = inventory.AddItem(slotItem);
            }
            else if (PlayerInventory.Instance != null)
            {
                replacedItem = PlayerInventory.Instance.AddItem(slotItem);
            }

            // NEW: If the player dropped an old item, put it on display for 0 gold!
            if (replacedItem != null)
            {
                SetupSlot(replacedItem, 0);
            }
            else
            {
                ClearSlot();
            }
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    private void ClearSlot()
    {
        slotItem = null;
        currentPrice = 0;

        if (itemSpriteRenderer != null)
        {
            itemSpriteRenderer.sprite = null;
            itemSpriteRenderer.enabled = false;
        }

        if (priceText != null)
        {
            priceText.text = "";
            priceText.gameObject.SetActive(false);
        }
    }

    public string GetInteractionPrompt()
    {
        return null;
    }
}