using UnityEngine;

public class EnemyChaseState : EnemyState
{
    protected override string AnimBoolName => "isRunning";
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.speed = enemyBehaviour.chaseSpeed;
    }

    public override void Update()
    {
        base.Update();

        // 1. Fallback if target is somehow missing
        if (enemy.Target == null) return;

        // 2. Pass enemy.Target so the CombatManager builds slots around the Payload (or Player if aggroed)
        if (CombatManager.Instance.RequestSlot(enemy, enemy.Target, out Vector3 slotPos))
        {
            enemy.Agent.SetDestination(slotPos);

            float distanceToSlot = Vector2.Distance(enemy.transform.position, slotPos);

            if (distanceToSlot <= enemyData.attackRange && enemy.Combat.CanMeleeAtack())
            {
                enemyStateMachine.ChangeState(enemy.AttackState);
            }
        }
        else
        {
            // Fallback if no slots are open: just walk directly toward the target
            enemy.Agent.SetDestination(enemy.Target.position);
        }
    }
}