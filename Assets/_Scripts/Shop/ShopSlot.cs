using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class ShopSlot : MonoBehaviour, IInteractable
{
    private BaseItemSO slotItem;
    [SerializeField] private SpriteRenderer itemSpriteRenderer;
    [SerializeField] private TMP_Text priceText;

    public void SetupSlot(BaseItemSO itemToSell)
    {
        slotItem = itemToSell;

        if (slotItem != null)
        {
            if (itemSpriteRenderer != null && slotItem.itemSprite != null)
            {
                itemSpriteRenderer.sprite = slotItem.itemSprite;
                itemSpriteRenderer.enabled = true;
            }

            if (priceText != null)
            {
                priceText.text = slotItem.price.ToString();
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

        if (GameResource.TrySpendGold(slotItem.price))
        {
            Debug.Log($"Purchased {slotItem.itemName} for {slotItem.price} gold!");

            if (player != null)
            {
                PlayerInventory inventory = player.GetComponent<PlayerInventory>();
                if (inventory != null) inventory.AddItem(slotItem);
            }
            else if (PlayerInventory.Instance != null)
            {
                PlayerInventory.Instance.AddItem(slotItem);
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
}