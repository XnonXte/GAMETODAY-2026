using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private EnemyConfig config;
    private Enemy enemy;
    private AnimationEventDetection eventDetection;
    private float lastAttackTime;

    private void Awake()
    {
        eventDetection = GetComponentInChildren<AnimationEventDetection>();
    }

    private void OnEnable()
    {
        eventDetection.OnAnimationAttackTriggered += PerformMeleeAtack;
    }

    private void OnDisable()
    {
        eventDetection.OnAnimationAttackTriggered -= PerformMeleeAtack;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        config = enemy.Config;
    }

    public bool CanMeleeAtack() => Time.time >= lastAttackTime + config.meleeCooldown;

    public void PerformMeleeAtack()
    {
        lastAttackTime = Time.time;

        Vector2 hitboxCenter = (Vector2)enemy.transform.position + new Vector2(config.attackHitboxOffset.x * enemy.FacingDirection, config.attackHitboxOffset.y);

        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(hitboxCenter, config.attackHitboxSize, 0f, config.targetLayerMask);

        foreach (Collider2D target in hitTargets)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(config.enemyMeleeDamage, Vector2.zero, AttackType.Light);
            }

            Debug.Log($"Enemy punched {target.name}!");
        }
    }
}
