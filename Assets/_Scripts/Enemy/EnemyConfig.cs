using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float evadeSpeed = 4f;

    [Header("Transition Probabilities")]
    [Range(0f, 1f)] public float chaseChance = 0.5f; // Patrol -> Chase (Timer based)
    [Range(0f, 1f)] public float evadeChance = 0.6f; // Patrol -> Evade (Distance based)
    [Range(0f, 1f)] public float attackCommitChance = 0.8f; // Chase -> Attack (Will they hesitate?)
    [Range(0f, 1f)] public float comboChance = 0.3f; // Attack -> Attack (Will they do a follow-up attack?)

    [Header("Distances & Timers")]
    public float decisionInterval = 2f;
    public float smallWanderRadius = 1.5f;
    public float minSafeDistance = 4f;

    [Header("Combat")]
    public float attackRange = 1f;
    public float attackDuration = 1f;
    public float knockbackDuration = 0.5f;
    public float attackDelay = 0.4f;
    public float meleeCooldown = 1f;
    public float enemyMeleeDamage = 10;
    public LayerMask targetLayerMask;

    [Header("Knockback")]
    public float lightKnockbackStrength = 2f;
    public float lightStunDuration = 0.3f;
    public float heavyKnockbackStrength = 7f;
    public float heavyStunDuration = 0.7f;

    [Header("Attack Hitbox")]
    // Wide X (reach), narrow Y (lane strictness)
    public Vector2 attackHitboxSize = new Vector2(1.5f, 0.4f);
    // Places the box in front of the enemy's face
    public Vector2 attackHitboxOffset = new Vector2(1f, 1.5f);
}