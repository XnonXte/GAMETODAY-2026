using UnityEngine;

public class FlyingSlashProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float baseDamage = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private bool pierceEnemies = true;

    private float directionX;
    private float damageMultiplier = 1f;

    public void Initialize(float facingDirectionX, float playerDamageMultiplier)
    {
        directionX = Mathf.Sign(facingDirectionX); 
        damageMultiplier = playerDamageMultiplier;

        if (directionX < 0)
        {
            transform.localScale = new Vector3(-7, 7, 7);
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * directionX * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Vector2 hitDirection = new Vector2(directionX, 0f);
                damageable.TakeDamage(baseDamage * damageMultiplier, hitDirection, AttackType.Heavy);

                Debug.Log("Flying Slash hit an enemy!");
            }

            if (!pierceEnemies)
            {
                Destroy(gameObject);
            }
        }
    }
}