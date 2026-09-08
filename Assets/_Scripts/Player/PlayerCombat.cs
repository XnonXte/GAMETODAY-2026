using UnityEngine;

public enum AttackType
{
    Light,
    Heavy
}

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private Vector2 attackSize = new Vector2(5f, 5f);
    [SerializeField] private LayerMask enemyMask;

    [Header("Damage Settings")]
    [SerializeField] private float lightAttackDamage = 10f;
    [SerializeField] private float heavyAttackDamage = 25f;
    public float DamageMultiplier = 1.0f;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackOrigin.position, attackSize);
    }

    public void Attack(AttackType attackType, int facingDirection)
    {
        // Optional: Play different sounds based on heavy vs light
        AudioManager.Instance.PlayAudio(AudioManager.Instance.SFX_Melee);

        float damage = (attackType == AttackType.Heavy) ? heavyAttackDamage : lightAttackDamage;

        Collider2D[] hit = Physics2D.OverlapBoxAll(attackOrigin.position, attackSize, 0f, enemyMask);

        foreach (var enemy in hit)
        {
            if (enemy.isTrigger)
            {
                IDamageable enemyInterface = enemy.GetComponent<IDamageable>();

                if (enemyInterface != null)
                {
                    Vector2 hitDirection = new Vector2(facingDirection, 0f);

                    enemyInterface.TakeDamage(damage * DamageMultiplier, hitDirection, attackType);
                    Debug.Log($"[PlayerCombat] Enemy Hit with {attackType} attack!");

                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.AgroOnPlayer();
                    }
                }
            }
        }
    }
}