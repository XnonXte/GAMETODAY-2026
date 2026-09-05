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
    [SerializeField] private float playerDamage = 10f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackOrigin.position, attackSize);
    }

    public void Attack()
    {
        AudioManager.Instance.PlayAudio(AudioManager.Instance.SFX_Melee);
        Collider2D[] hit = Physics2D.OverlapBoxAll(attackOrigin.position, attackSize, 0f, enemyMask);

        foreach (var enemy in hit)
        {
            if (enemy.isTrigger)
            {
                IDamageable enemyInterface = enemy.GetComponent<IDamageable>();

                if (enemyInterface != null)
                {
                    enemyInterface.TakeDamage(playerDamage, Vector2.zero, AttackType.Light);
                    Debug.Log("[PlayerCombat] Enemy Hit!");
                }
            }
        }
    }
}
