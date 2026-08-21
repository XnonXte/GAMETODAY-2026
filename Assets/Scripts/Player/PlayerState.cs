public abstract class PlayerState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;
    
    public PlayerState (Player player, PlayerStateMachine playerStateMachine) 
    { 
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

}
