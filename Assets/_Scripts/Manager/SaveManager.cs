using UnityEngine;
using System;

public static class SaveManager
{
    public static void SaveGame(GameScene nextScene, float playerHp, float payloadHp, int gold, BaseItemSO consumable, BaseItemSO weapon)
    {
        // Save standard stats
        PlayerPrefs.SetString("SavedScene", nextScene.ToString());
        PlayerPrefs.SetFloat("PlayerHP", playerHp);
        PlayerPrefs.SetFloat("PayloadHP", payloadHp);
        PlayerPrefs.SetInt("Gold", gold);

        // Save items by their file name so we can load them from the Resources folder later
        PlayerPrefs.SetString("Consumable", consumable != null ? consumable.name : "");
        PlayerPrefs.SetString("Weapon", weapon != null ? weapon.name : "");

        PlayerPrefs.SetInt("HasSave", 1); // A flag we use to enable the Continue button
        PlayerPrefs.Save();

        Debug.Log($"[SaveManager] Game Saved successfully! Next Stage: {nextScene}");
    }

    public static bool HasSave()
    {
        return PlayerPrefs.HasKey("HasSave") && PlayerPrefs.GetInt("HasSave") == 1;
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[SaveManager] Save file wiped!");
    }

    public static BaseItemSO LoadItem(string prefsKey)
    {
        string itemName = PlayerPrefs.GetString(prefsKey, "");
        if (string.IsNullOrEmpty(itemName)) return null;

        // Searches the Resources/Items folder for the exact scriptable object file name
        return Resources.Load<BaseItemSO>("Items/" + itemName);
    }
}