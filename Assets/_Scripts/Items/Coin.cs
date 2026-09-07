using UnityEngine;
using DG.Tweening; 

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [Header("Scatter Settings")]
    [SerializeField] private float scatterRadius = 1.5f; 
    [SerializeField] private float jumpHeight = 1f;    
    [SerializeField] private float dropDuration = 0.5f; 

    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();

        col.enabled = false;

        Vector2 randomOffset = Random.insideUnitCircle * scatterRadius;
        Vector3 targetPosition = transform.position + (Vector3)randomOffset;

        transform.DOJump(targetPosition, jumpHeight, 1, dropDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                col.enabled = true;
            });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameResource.AddGoldAmount(1);
            AudioManager.Instance.PlayAudio(AudioManager.Instance.SFX_Coin);

            col.enabled = false;
            transform.DOScale(0f, 0.15f).OnComplete(() => Destroy(gameObject));

            EventHandler.WhenGoldAmountChanged();
        }
    }
}