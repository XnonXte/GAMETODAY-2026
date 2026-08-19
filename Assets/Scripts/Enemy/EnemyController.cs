using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    private SpriteRenderer spriteRenderer;
    private NavMeshAgent agent;
    private float currentHealth;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void InitEnemyData(SOBaseEnemy enemyData)
    {
        currentHealth = enemyData.maxHealth;
        float enemyDamage = enemyData.attackDamage;
        spriteRenderer.sprite = enemyData.enemySprite;
    }

    public void OnDamage(float amount)
    {
        ChangeHealth(amount);
    }

    public void ChangeHealth(float amount)
    {
        currentHealth -= amount;

        Debug.Log($"[BaseEnemy] Enemy Got Hit! {currentHealth}");

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject, .1f);
    }
}
