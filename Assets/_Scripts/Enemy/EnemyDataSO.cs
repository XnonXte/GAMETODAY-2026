using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Visuals")]
    public Sprite enemySprite; // The single PNG for the body
    public RuntimeAnimatorController weaponAnimatorController;

    [Header("Information")]
    public string enemyName = "Thug";
    public float maxHealth = 100f;
    public float enemyMeleeDamage = 10f;

    [Header("Loot Drop")]
    public GameObject coinPrefab;
    public int minCoinsDropped = 1;
    public int maxCoinsDropped = 3;

    [Header("AI Behavior")]
    public EnemyBehaviourSO enemyBehaviour;

    [Header("Combat Stats")]
    public float attackRange = 1f;
    public float attackDuration = 1f;
    public float attackDelay = 0.4f;
    public float meleeCooldown = 1f;
    public LayerMask targetLayerMask;

    [Header("Knockback Settings")]
    public float lightKnockbackStrength = 2f;
    public float lightStunDuration = 0.3f;
    public float heavyKnockbackStrength = 7f;
    public float heavyStunDuration = 0.7f;

    [Header("Attack Hitbox")]
    public Vector2 attackHitboxSize = new Vector2(1.5f, 0.4f);
    public Vector2 attackHitboxOffset = new Vector2(1f, 1.5f);
}