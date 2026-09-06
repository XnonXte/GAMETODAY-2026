using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private EnemyDataSO enemyData;
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
        enemyData = enemy.EnemyData;
    }

    public bool CanMeleeAtack() => Time.time >= lastAttackTime + enemyData.meleeCooldown;

    public void PerformMeleeAtack()
    {
        lastAttackTime = Time.time;

        Vector2 hitboxCenter = (Vector2)enemy.transform.position + new Vector2(enemyData.attackHitboxOffset.x * enemy.FacingDirection, enemyData.attackHitboxOffset.y);

        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(hitboxCenter, enemyData.attackHitboxSize, 0f, enemyData.targetLayerMask);

        foreach (Collider2D target in hitTargets)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(enemyData.enemyMeleeDamage, Vector2.zero, AttackType.Light);
            }

            Debug.Log($"Enemy punched {target.name}!");
        }
    }
}
