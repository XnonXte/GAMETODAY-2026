using UnityEngine;

public class EnemyKnockbackState : EnemyState
{
    protected override string AnimBoolName => "isWalking";
    private Vector2 knockbackForce;

    public EnemyKnockbackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public void SetKnockbackForce(Vector2 force) => knockbackForce = force;

    public override void Enter()
    {
        base.Enter();
        stateTimer = config.knockbackDuration;

        CombatManager.Instance.ReleaseSlot(enemy);
        enemy.Agent.enabled = false;

        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            enemyStateMachine.ChangeState(enemy.PatrolState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        rigidbody.linearVelocity = Vector2.zero;
        enemy.Agent.enabled = true;
    }
}
