using UnityEngine;

public class EnemyEvadeState : EnemyState
{
    protected override string AnimBoolName => "isWalking";
    public EnemyEvadeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.speed = config.evadeSpeed;
    }

    public override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.Target.position);

        // 1. Check if we have backed up enough to be safe again
        if (distanceToPlayer >= config.minSafeDistance + 1f) // Added 1f buffer so they don't stutter between states
        {
            enemyStateMachine.ChangeState(enemy.PatrolState);
            return;
        }

        // 2. Move away from the player
        // Calculate the direction FROM the player TO the enemy
        Vector3 directionAway = (enemy.transform.position - enemy.Target.position).normalized;

        // Pick a destination safely out of reach
        Vector3 safeDestination = enemy.Target.position + (directionAway * (config.minSafeDistance + 2f));

        enemy.Agent.SetDestination(safeDestination);
    }
}