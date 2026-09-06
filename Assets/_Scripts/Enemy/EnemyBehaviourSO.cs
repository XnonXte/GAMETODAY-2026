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
    [Range(0f, 1f)] public float attackCommitChance = 0.8f;
    [Range(0f, 1f)] public float comboChance = 0.3f;

    [Header("Distances & Timers")]
    public float decisionInterval = 2f;
    public float smallWanderRadius = 1.5f;
    public float minSafeDistance = 4f;
}
