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
        if (InputManager.Instance.GetPlayerDash() && player.CanDash())
        {
            playerStateMachine.ChangeState(player.DashState);
            return;
        }

        if (InputManager.Instance.GetPlayerAttack())
        {
            playerStateMachine.ChangeState(player.AttackState);
            return;
        }

        if (InputManager.Instance.GetPlayerMovement().sqrMagnitude <= .01f)
        {
            playerStateMachine.ChangeState(player.IdleState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        player.MovePlayer();
    }
}
