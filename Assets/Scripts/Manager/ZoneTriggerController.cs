using UnityEngine;

public class ZoneTriggerController : MonoBehaviour
{
    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered) return;

        if (collision.CompareTag("Player"))
        {
            EventHandler.WhenBattleStart();
            isTriggered = true;
        }
    }
}
