public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        player.StopMovement();
    }

    public override void Exit()
    {
        
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
