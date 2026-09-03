public abstract class EnemyState 
{
    protected EnemyStateMachine enemyStateMachine;

    //public EnemyState(Player player, PlayerStateMachine playerStateMachine)
    //{
    //    this.player = player;
    //    this.playerStateMachine = playerStateMachine;
    //}

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}
