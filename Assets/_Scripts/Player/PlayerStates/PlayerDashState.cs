using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float dashTimer;

    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        dashTimer = player.GetDashDuration();
        player.StartDash();
    }

    public override void Update()
    {
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            player.StopDash();

            if (InputManager.Instance.GetPlayerMovement().sqrMagnitude > 0.01f)
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
        player.StopDash();
    }
}
