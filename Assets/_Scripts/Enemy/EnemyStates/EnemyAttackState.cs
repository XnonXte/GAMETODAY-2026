using UnityEngine;

public class EnemyAttackState : EnemyState
{
    protected override string AnimBoolName => "";
    private string attackAnimationName = "TestEnemyAttack";

    private bool isWindingUp;
    private bool hasExecutedAttack;
    private float failsafeTimer;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.isStopped = true;
        isWindingUp = true;
        hasExecutedAttack = false;

        stateTimer = enemyData.attackDelay;
    }

    public override void Update()
    {
        base.Update();

        if (isWindingUp)
        {
            float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.Target.position);

            // THE FIX: We add + 0.5f as a buffer zone. 
            // This stops the enemy from immediately aborting the attack just because colliders kept them 0.01 units too far away!
            if (distanceToPlayer > (enemyData.attackRange + 2f))
            {
                CombatManager.Instance.ReleaseSlot(enemy);
                enemyStateMachine.ChangeState(enemy.ChaseState);
                return;
            }

            if (stateTimer <= 0)
            {
                if (enemyBehaviour.alwaysCommitToAttack || Random.value <= enemyBehaviour.attackCommitChance)
                {
                    isWindingUp = false;
                    hasExecutedAttack = true;

                    // MANUALLY turn the boolean on so the Animator doesn't cancel the attack!
                    anim.SetBool("isAttacking", true);
                    anim.Play(attackAnimationName, -1, 0f);

                    failsafeTimer = 1.5f;
                }
                else
                {
                    CombatManager.Instance.ReleaseSlot(enemy);
                    enemyStateMachine.ChangeState(enemy.EvadeState);
                }
            }
        }
        else if (hasExecutedAttack)
        {
            failsafeTimer -= Time.deltaTime;

            if (failsafeTimer <= 0)
            {
                Debug.LogWarning($"[Failsafe] {enemy.gameObject.name} got stuck in AttackState! Forcing exit.");
                AnimationFinishTrigger();
            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        if (hasExecutedAttack)
        {
            CombatManager.Instance.ReleaseSlot(enemy);
            enemyStateMachine.ChangeState(enemy.EvadeState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Agent.isStopped = false;
        anim.SetBool("isAttacking", false);
    }
}