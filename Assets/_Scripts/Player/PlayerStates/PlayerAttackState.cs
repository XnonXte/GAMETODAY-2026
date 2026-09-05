using UnityEngine;

public class PlayerAttackState : PlayerState
{
    protected override string AnimBoolName => "isAttacking";

    private float attackTimer;
    private float attackDuration = 0.4f;

    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        base.Enter(); // Automatically sets "isAttacking" to true

        player.StopMovement();
        attackTimer = attackDuration;

        if (player.Combat != null)
        {
            player.Combat.Attack();
        }
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

    public override void Exit()
    {
        base.Exit(); // Automatically sets "isAttacking" to false
        player.StopMovement();
    }
}