using UnityEngine;

public class EnemyChaseState : EnemyState
{
    protected override string AnimBoolName => "isRunning";
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.speed = config.chaseSpeed;
    }

    public override void Update()
    {
        base.Update();

        if (CombatManager.Instance.RequestSlot(enemy, out Vector3 slotPos))
        {
            enemy.Agent.SetDestination(slotPos);

            float distanceToSlot = Vector2.Distance(enemy.transform.position, slotPos);

            if (distanceToSlot <= config.attackRange && enemy.Combat.CanMeleeAtack())
            {
                enemyStateMachine.ChangeState(enemy.AttackState);
            }
        }
        else
        {
            enemyStateMachine.ChangeState(enemy.EvadeState);
        }
    }
}