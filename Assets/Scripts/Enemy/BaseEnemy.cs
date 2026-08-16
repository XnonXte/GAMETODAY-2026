using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;

    private float currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
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
