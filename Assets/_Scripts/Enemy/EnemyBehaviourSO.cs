using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyBehaviour")]
public class EnemyBehaviourSO : ScriptableObject
{
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float evadeSpeed = 4f;

    [Header("Transition Probabilities")]
    [Range(0f, 1f)] public float chaseChance = 0.5f;
    [Range(0f, 1f)] public float evadeChance = 0.6f;

    [Tooltip("If 1 (or 100%), the enemy will roll the dice. Set the bool below to completely bypass the dice roll.")]
    [Range(0f, 1f)] public float attackCommitChance = 0.8f;

    [Header("Forced Overrides")]
    [Tooltip("If true, the enemy will NEVER chicken out, completely ignoring the Attack Commit Chance.")]
    public bool alwaysCommitToAttack = false;

    [Header("Distances & Timers")]
    public float decisionInterval = 2f;
    public float smallWanderRadius = 1.5f;
    public float minSafeDistance = 4f;
}