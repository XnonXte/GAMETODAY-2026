using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Item Pool")]
    [SerializeField] private List<BaseItemSO> availableItems; 

    [Header("Shop Visual Slots")]
    [SerializeField] private ShopSlot[] shopSlots;

    private void Start()
    {
        PopulateShop();
    }

    public void PopulateShop()
    {
        if (availableItems == null || availableItems.Count == 0)
        {
            Debug.LogWarning("[ShopManager] No items assigned to the shop pool!");
            return;
        }

        List<BaseItemSO> itemsToPickFrom = new List<BaseItemSO>(availableItems);

        foreach (ShopSlot slot in shopSlots)
        {
            if (slot != null)
            {
                if (itemsToPickFrom.Count == 0)
                {
                    slot.SetupSlot(null);
                    continue;
                }

                int randomIndex = Random.Range(0, itemsToPickFrom.Count);
                BaseItemSO choosenItem = itemsToPickFrom[randomIndex];
                slot.SetupSlot(choosenItem);

                itemsToPickFrom.RemoveAt(randomIndex);
            }
        }
    }
}