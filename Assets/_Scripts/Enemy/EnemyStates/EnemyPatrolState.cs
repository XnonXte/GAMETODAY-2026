using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    protected override string AnimBoolName => "isWalking";
    public EnemyPatrolState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.speed = config.patrolSpeed;
        SetSmallWanderPoint();
    }

    public override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.Target.position);
        if (distanceToPlayer < config.minSafeDistance)
        {
            enemyStateMachine.ChangeState(enemy.EvadeState);
            return;
        }

        if (stateTimer <= 0)
        {
            if (Random.value <= config.chaseChance)
            {
                if (CombatManager.Instance.RequestSlot(enemy, out Vector3 slotPos))
                {
                    enemyStateMachine.ChangeState(enemy.ChaseState);
                    return;
                }
            }

            SetSmallWanderPoint();
        }
    }

    private void SetSmallWanderPoint()
    {
        stateTimer = config.decisionInterval;

        Vector2 randomOffset = Random.insideUnitCircle * config.smallWanderRadius;
        enemy.Agent.SetDestination(enemy.transform.position + (Vector3)randomOffset);
    }
}