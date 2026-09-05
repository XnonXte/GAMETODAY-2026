using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private GameScene gameScene;
    private bool hasEntered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasEntered) return;

        hasEntered = true;
        if (collision.CompareTag("Player"))
        {
            GameSceneManager.Instance.ChangeScene(gameScene);
        }
    }
}
