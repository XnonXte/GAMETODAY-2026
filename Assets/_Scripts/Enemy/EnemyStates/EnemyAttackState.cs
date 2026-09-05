using UnityEngine;

public class EnemyAttackState : EnemyState
{
    protected override string AnimBoolName => "isAttacking";
    private string attackAnimationName = "TestEnemyAttack";
    private bool isWindingUp;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.isStopped = true;

        isWindingUp = true;
        stateTimer = config.attackDelay;
    }

    public override void Update()
    {
        base.Update();

        if (isWindingUp && stateTimer <= 0)
        {
            if (Random.value <= config.attackCommitChance)
            {
                isWindingUp = false;
                anim.Play(attackAnimationName, -1, 0f);
            }
            else
            {
                CombatManager.Instance.ReleaseSlot(enemy);
                enemyStateMachine.ChangeState(enemy.EvadeState);
            }
        }
    }

    public override void AnimationFinishTrigger()
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

    public override void Exit()
    {
        base.Exit();
        enemy.Agent.isStopped = false;
    }
}