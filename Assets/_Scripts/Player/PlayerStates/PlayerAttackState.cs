using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private float attackTimer;
    private float attackDuration = 0.4f;

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
        base.Update();

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            if (InputManager.Instance != null && InputManager.Instance.GetPlayerMovement().sqrMagnitude > 0.01f)
            {
                playerStateMachine.ChangeState(player.WalkState);
            }
            else
            {
                playerStateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
