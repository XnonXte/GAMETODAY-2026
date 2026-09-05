using UnityEngine;

public abstract class EnemyState 
{
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemy;

    protected Rigidbody2D rigidbody;
    protected EnemyConfig config;
    protected Animator anim;
    protected virtual string AnimBoolName => null;
    protected float stateTimer;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
        
        rigidbody = enemy.Rigidbody;
        config = enemy.Config;
        anim = enemy.Anim;
    }

    public virtual void Enter() 
    {
        if (!string.IsNullOrEmpty(AnimBoolName)) anim.SetBool(AnimBoolName, true);
    }
    public virtual void Exit() 
    {
        if (!string.IsNullOrEmpty(AnimBoolName)) anim.SetBool(AnimBoolName, false);
    }
    public virtual void Update() 
    {
        //enemy.FlipSprite();
        if (stateTimer > 0) stateTimer -= Time.deltaTime;
    }
    public virtual void FixedUpdate() { }
}
