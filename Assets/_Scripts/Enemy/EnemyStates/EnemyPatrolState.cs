using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    protected override string AnimBoolName => "isWalking";
    public EnemyPatrolState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.speed = enemyBehaviour.patrolSpeed;
        SetSmallWanderPoint();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            if (Random.value <= enemyBehaviour.chaseChance)
            {
                if (CombatManager.Instance.RequestSlot(enemy, out Vector3 slotPos))
                {
                    enemyStateMachine.ChangeState(enemy.ChaseState);
                    return;
                }
            }

            if (Random.value <= enemyBehaviour.evadeChance)
            {
                enemyStateMachine.ChangeState(enemy.EvadeState);
                return;
            }

            SetSmallWanderPoint();
        }
    }

    private void SetSmallWanderPoint()
    {
        stateTimer = enemyBehaviour.decisionInterval;
        Vector2 randomOffset = Random.insideUnitCircle * enemyBehaviour.smallWanderRadius;
        enemy.Agent.SetDestination(enemy.transform.position + (Vector3)randomOffset);
    }
}