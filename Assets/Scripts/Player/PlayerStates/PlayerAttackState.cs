using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private float attackDuration = .4f;
    private float attackTimer;

    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        player.StopMovement();
        attackTimer = attackDuration;

        player.playerCombat.Attack();

        player.playerAnimator.SetBool("isAttacking", true);
    }

    public override void Exit()
    {
        player.StopMovement();

        player.playerAnimator.SetBool("isAttacking", false);
    }

    public override void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
    }
}
