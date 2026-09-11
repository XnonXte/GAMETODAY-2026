using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance { get; private set; }

    [Header("Persistent Data")]
    public float savedPlayerHealth = -1f; // -1 means no data has been saved yet (new game)
    public float savedPayloadHealth = -1f;
    public BaseItemSO savedConsumable;
    public BaseItemSO savedWeapon;

    private void Awake()
    {
        // Standard Singleton that SURVIVES scene loads
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EventHandler.OnGameLose += HandleGameLose;
    }

    private void OnDisable()
    {
        EventHandler.OnGameLose -= HandleGameLose;
    }

    // Call this right before you load the next scene!
    public void SaveLevelData(float playerHp, float payloadHp, BaseItemSO consumable, BaseItemSO weapon)
    {
        savedPlayerHealth = playerHp;
        savedPayloadHealth = payloadHp;
        savedConsumable = consumable;
        savedWeapon = weapon;

        Debug.Log("[GameSessionManager] Level Data Saved successfully!");
    }

    // Optional: Call this if the player dies or restarts the whole run
    public void ResetSession()
    {
        savedPlayerHealth = -1f;
        savedPayloadHealth = -1f;
        savedConsumable = null;
        savedWeapon = null;
    }

    private void HandleGameLose()
    {
        ResetSession();
        SaveManager.ClearSave();
        GameSceneManager.Instance.ChangeScene(GameScene.MainMenu);
    }
}