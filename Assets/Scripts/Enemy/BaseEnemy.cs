using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    private NavMeshAgent agent;
    private float currentHealth;

    //refactor diakhir
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = .5f;
    private Material currentMaterial;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentHealth = maxHealth;

        spriteRenderer.material = defaultMaterial;
    }

    public void OnDamage(float amount)
    {
        ChangeHealth(amount);
        StartCoroutine(FlashHit(flashDuration));
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

    //refactor diakhir aja
    private IEnumerator FlashHit(float duration)
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = defaultMaterial;
    }
}
