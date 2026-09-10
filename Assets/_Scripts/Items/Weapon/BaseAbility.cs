using UnityEngine;
using System.Collections.Generic;

public abstract class BaseAbility : MonoBehaviour
{
    [Header("Base Ability Settings")]
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected float damagePerTick = 10f;
    [SerializeField] protected float timeBetweenTicks = 0.5f;

    protected Player player;
    private Dictionary<Collider2D, float> lastHitTimes = new Dictionary<Collider2D, float>();

    protected virtual void Start()
    {
        player = GetComponentInParent<Player>();

        if (player == null)
        {
            Debug.LogError("Ability must be spawned as a child of the Player!");
            return;
        }

        Destroy(gameObject, duration);

        OnAbilityStart();
    }

    private void OnDestroy()
    {
        if (player != null) OnAbilityEnd();
    }

    protected virtual void OnAbilityStart() { }
    protected virtual void OnAbilityEnd() { }

    protected void HandleAoEDamage(Collider2D enemyCollider, AttackType attackType)
    {
        IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
        if (damageable == null) return;

        if (!lastHitTimes.ContainsKey(enemyCollider) || Time.time >= lastHitTimes[enemyCollider] + timeBetweenTicks)
        {
            lastHitTimes[enemyCollider] = Time.time;

            float directionX = Mathf.Sign(enemyCollider.transform.position.x - player.transform.position.x);

            Vector2 hitDirection = new Vector2(directionX, 0f);
            float finalDamage = damagePerTick * player.Combat.DamageMultiplier;

            damageable.TakeDamage(finalDamage, hitDirection, attackType);
        }
    }
}