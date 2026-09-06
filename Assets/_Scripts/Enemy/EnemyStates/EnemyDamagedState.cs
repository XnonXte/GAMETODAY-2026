using UnityEngine;

public class EnemyDamagedState : EnemyState
{
    protected override string AnimBoolName => "isDamaged";
    private string damageClipName = "TestEnemyDamaged";
    private Vector2 knockbackForce;
    private float customStunDuration;

    public EnemyDamagedState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public void SetKnockbackForce(Vector2 force, float stunDuration = -1f)
    {
        knockbackForce = force;
        customStunDuration = (stunDuration > 0f) ? stunDuration : enemyData.knockbackDuration;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = customStunDuration;

        CombatManager.Instance.ReleaseSlot(enemy);
        enemy.Agent.enabled = false;

        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);

        anim.SetBool("isIdling", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);

        anim.Play(damageClipName, -1, 0f);
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