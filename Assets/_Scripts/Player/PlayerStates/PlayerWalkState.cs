using UnityEngine;

public class PlayerWalkState : PlayerState
{
    protected override string AnimBoolName => "isWalking";

    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        base.Enter(); // Automatically sets "isWalking" to true

        player.Anim.Play("TestWalkAnimation", -1, 0f);
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.Instance == null) return;

        if (InputManager.Instance.GetPlayerDash() && player.CanDash())
        {
            playerStateMachine.ChangeState(player.DashState);
            return;
        }

        if (InputManager.Instance.GetPlayerAttack())
        {
            if (!player.IsAttackLocked) playerStateMachine.ChangeState(player.AttackState);
            return;
        }

        if (InputManager.Instance.GetPlayerMovement().sqrMagnitude <= 0.01f)
        {
            playerStateMachine.ChangeState(player.IdleState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.MovePlayer();
    }

    public override void Exit()
    {
        base.Exit(); // Automatically sets "isWalking" to false
        player.StopMovement();
    }
}