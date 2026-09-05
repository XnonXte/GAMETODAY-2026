using UnityEngine;

public class PlayerIdleState : PlayerState
{
    protected override string AnimBoolName => "isIdling";

    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        base.Enter(); // Automatically sets "isIdling" to true
        player.StopMovement();
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.Instance == null) return;

        // 1. Dash Check (Allows dashing directly from standing still)
        if (InputManager.Instance.GetPlayerDash() && player.CanDash())
        {
            playerStateMachine.ChangeState(player.DashState);
            return;
        }

        // 2. Attack Check
        if (InputManager.Instance.GetPlayerAttack())
        {
            playerStateMachine.ChangeState(player.AttackState);
            return;
        }

        // 3. Movement Check
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