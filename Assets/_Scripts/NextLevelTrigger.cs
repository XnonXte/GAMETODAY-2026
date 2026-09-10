using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private GameScene gameScene;
    private bool hasEntered = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hasEntered) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Changing Scene to Next!");
            hasEntered = true;

            // --- NEW SAVE LOGIC START ---
            Player player = collision.GetComponent<Player>();

            // Check if we have our player and the GameSessionManager exists in the scene
            if (player != null && GameSessionManager.Instance != null)
            {
                // 1. Get the Payload's Health (checking if Payload is alive/exists in this scene)
                float payloadHp = -1f;
                if (Payload.Instance != null)
                {
                    payloadHp = Payload.Instance.GetComponent<Health>().CurrentHp;
                }

                // 2. Get the Player's Inventory
                PlayerInventory inventory = player.GetComponent<PlayerInventory>();

                // 3. Save it all to the persistent Session Manager!
                GameSessionManager.Instance.SaveLevelData(
                    player.HealthComponent.CurrentHp,
                    payloadHp,
                    inventory.CurrentConsumable,
                    inventory.CurrentWeapon
                );
            }
            // --- NEW SAVE LOGIC END ---

            // Proceed to load the next level
            GameSceneManager.Instance.ChangeScene(gameScene);
        }
    }
}