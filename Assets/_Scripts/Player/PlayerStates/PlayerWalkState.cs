public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        player.playerAnimator.SetBool("isWalking", true);
    }

    public override void Exit()
    {
        player.playerAnimator.SetBool("isWalking", false);
        player.StopMovement();
    }

    public override void Update()
    {
        if (InputManager.instance.GetPlayerDash() && player.CanDash())
        {
            playerStateMachine.ChangeState(player.dashState);
            return;
        }

        if (InputManager.instance.GetPlayerAttack())
        {
            playerStateMachine.ChangeState(player.attackState);
            return;
        }

        if (InputManager.instance.GetPlayerMovement().sqrMagnitude <= .01f)
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
