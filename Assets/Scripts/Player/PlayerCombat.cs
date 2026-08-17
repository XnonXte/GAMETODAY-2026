using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private Vector2 attackSize = new Vector2(5f, 5f);
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float playerDamage = 10f;
    [SerializeField] private float attackCooldown = .5f; //.5 detik
    private bool hasAttack;


    private void Update()
    {
        ReadInput();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackOrigin.position, attackSize);
    }

    private void ReadInput()
    {
        if (InputManager.instance.GetPlayerAttack())
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!hasAttack)
        {
            Collider2D[] hit = Physics2D.OverlapBoxAll(attackOrigin.position, attackSize, 0f, enemyMask);

            foreach (var enemy in hit)
            {
                if (enemy.isTrigger)
                {
                    IDamageable enemyInterface = enemy.GetComponent<IDamageable>();

                    if (enemyInterface != null)
                    {
                        enemyInterface.OnDamage(playerDamage);
                        Debug.Log("[PlayerCombat] Enemy Hit!");
                        StartCoroutine(AttackDebounce());
                    }
                }
            }
        }
    }

    private IEnumerator AttackDebounce()
    {
        hasAttack = true;
        yield return new WaitForSeconds(attackCooldown);
        hasAttack = false;
    }
}
