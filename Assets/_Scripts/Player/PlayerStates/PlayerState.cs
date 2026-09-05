using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;

    protected Rigidbody2D rigidbody;
    protected Animator anim;

    protected virtual string AnimBoolName => null;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;

        rigidbody = player.Rigidbody;
        anim = player.Anim;
    }

    public virtual void Enter()
    {
        if (!string.IsNullOrEmpty(AnimBoolName) && anim != null) anim.SetBool(AnimBoolName, true);
    }

    public virtual void Exit()
    {
        if (!string.IsNullOrEmpty(AnimBoolName) && anim != null) anim.SetBool(AnimBoolName, false);
    }

    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}