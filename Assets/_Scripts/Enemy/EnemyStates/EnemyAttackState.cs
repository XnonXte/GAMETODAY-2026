using UnityEngine;

public class EnemyAttackState : EnemyState
{
    protected override string AnimBoolName => "isAttacking";
    private string attackAnimationName = "TestEnemyAttack";
    private bool hasDealtDamage; 

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        stateTimer = config.attackDuration;
        hasDealtDamage = false; 
        enemy.Agent.isStopped = true;

        anim.Play(attackAnimationName, -1, 0f);
        // Example: enemy.Animator.SetTrigger("Attack");
    }

    public override void Update()
    {
        base.Update();

        if (!hasDealtDamage && stateTimer <= config.attackDuration / 2f)
        {
            PerformAttackCheck();
            hasDealtDamage = true;
        }

        if (stateTimer <= 0)
        {
            if (Random.value <= config.comboChance)
            {
                enemyStateMachine.ChangeState(enemy.AttackState);
            }
            else
            {
                CombatManager.Instance.ReleaseSlot(enemy);
                enemyStateMachine.ChangeState(enemy.PatrolState);
            }
        }
    }

    private void PerformAttackCheck()
    {
        Vector2 hitboxCenter = (Vector2)enemy.transform.position + new Vector2(config.attackHitboxOffset.x * enemy.FacingDirection, config.attackHitboxOffset.y);

        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(hitboxCenter, config.attackHitboxSize, 0f, config.targetLayerMask);

        foreach (Collider2D target in hitTargets)
        {
            // Example of how you would apply damage to your player:
            // if (target.TryGetComponent(out PlayerHealth playerHealth))
            // {
            //     playerHealth.TakeDamage(config.attackDamage);
            // }

            Debug.Log($"Enemy punched {target.name}!");
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Agent.isStopped = false;
    }
}