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

        // 1. Dash Check
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

        // 3. Idle Check
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