using UnityEngine;

public class PlayerIdleState : PlayerState
{
    protected override string AnimBoolName => "isIdling";

    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        base.Enter(); // Automatically sets "isIdling" to true
        player.StopMovement();

        player.Anim.Play("TestIdle", -1, 0f);
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

        if (InputManager.Instance.GetPlayerMovement().sqrMagnitude > 0.01f)
        {
            playerStateMachine.ChangeState(player.WalkState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit(); // Automatically sets "isIdling" to false
    }
}