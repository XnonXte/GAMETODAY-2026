public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        player.playerAnimator.SetBool("isIdle", true);
        player.StopMovement();
    }

    public override void Exit()
    {
        player.playerAnimator.SetBool("isIdle", false);
    }

    public override void Update()
    {
        if (player.GetAttackInput())
        {
            playerStateMachine.ChangeState(player.attackState);
            return;
        }

        if (player.GetMoveInput().sqrMagnitude > .01f)
        {
            playerStateMachine.ChangeState(player.walkState);
            return;
        }
    }
}
