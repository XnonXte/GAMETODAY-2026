public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {
        player.StopMovement();
    }

    public override void Update()
    {
        if (player.GetAttackInput())
        {
            playerStateMachine.ChangeState(player.attackState);
            return;
        }

        if (player.GetMoveInput().sqrMagnitude <= .01f)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        player.MovePlayer();
    }
}
