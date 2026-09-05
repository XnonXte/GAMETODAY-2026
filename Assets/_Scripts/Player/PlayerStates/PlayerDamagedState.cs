using UnityEngine;

public class PlayerDamagedState : PlayerState
{
    protected override string AnimBoolName => "isDamaged";

    private Vector2 knockbackForce;
    private float customStunDuration;
    private float stunTimer; // Local timer just for this state

    public PlayerDamagedState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public void SetKnockbackForce(Vector2 force, float stunDuration = 0.4f)
    {
        knockbackForce = force;
        customStunDuration = stunDuration;
    }

    public override void Enter()
    {
        base.Enter();
        stunTimer = customStunDuration; // Start the stun countdown

        player.StopMovement();
        player.Rigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();

        // Manually tick down the stun timer
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        }
        else
        {
            // Stun is over, give control back to the player!
            playerStateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StopMovement(); // Stop sliding when recovering
    }
}