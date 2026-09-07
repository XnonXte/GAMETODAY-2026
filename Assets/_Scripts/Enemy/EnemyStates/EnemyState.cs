using UnityEngine;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

public abstract class EnemyState 
{
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemy;
    protected Rigidbody2D rigidbody;
    protected Animator anim;

    protected EnemyDataSO enemyData;
    protected EnemyBehaviourSO enemyBehaviour;

    protected virtual string AnimBoolName => null;
    protected float stateTimer;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
        
        rigidbody = enemy.Rigidbody;
        anim = enemy.Anim;

        enemyData = enemy.EnemyData;
        enemyBehaviour = enemy.EnemyData.enemyBehaviour;
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
    public virtual void AnimationFinishTrigger() { }
}
