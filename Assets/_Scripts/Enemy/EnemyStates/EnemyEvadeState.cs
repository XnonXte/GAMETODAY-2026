using UnityEngine;

public class EnemyEvadeState : EnemyState
{
    protected override string AnimBoolName => "isWalking";

    private float evadeDistance = 4f;
    private float maxEvadeTime = 1.5f; 

    public EnemyEvadeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.speed = enemyBehaviour.evadeSpeed;
        stateTimer = maxEvadeTime;

        Vector3 directionAway = (enemy.transform.position - enemy.Target.position).normalized;

        Vector3 safeDestination = enemy.transform.position + (directionAway * evadeDistance);
        enemy.Agent.SetDestination(safeDestination);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            enemyStateMachine.ChangeState(enemy.PatrolState);
            return;
        }

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            enemyStateMachine.ChangeState(enemy.PatrolState);
        }
    }
}