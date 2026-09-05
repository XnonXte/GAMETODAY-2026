using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    #region Attributes
    public int FacingDirection { get; private set; } = 1; //1 = kanan, -1 = kiri
    [field:SerializeField] public EnemyConfig Config { get; private set; }
    #endregion

    #region Components
    public Rigidbody2D Rigidbody { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Transform Target { get; private set; }
    public Animator Anim { get; private set; }
    #endregion

    #region EnemyStateMachine
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyEvadeState EvadeState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyKnockbackState KnockbackState { get; private set; }
    #endregion

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponentInChildren<Animator>();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        Target = GameObject.FindGameObjectWithTag("Player").transform;

        StateMachine = new EnemyStateMachine();
        PatrolState = new EnemyPatrolState(this, StateMachine);
        EvadeState = new EnemyEvadeState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        KnockbackState = new EnemyKnockbackState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(PatrolState);
    }

    private void Update() 
    { 
        StateMachine.CurrentEnemyState?.Update();
        UpdateFacingDirection();
    }
    private void FixedUpdate() => StateMachine.CurrentEnemyState?.FixedUpdate();

    private void UpdateFacingDirection()
    {
        float directionX = 0f;

        // 1. If moving, base direction on NavMesh velocity
        if (Agent.velocity.sqrMagnitude > 0.1f)
        {
            directionX = Agent.velocity.x;
        }
        // 2. If stopped and attacking, face the target (Player)
        else if (StateMachine.CurrentEnemyState == AttackState)
        {
            directionX = Target.position.x - transform.position.x;
        }

        // 3. Flip if the intended direction does not match our current FacingDirection
        if (directionX > 0.05f && FacingDirection == -1)
        {
            FlipSprite();
        }
        else if (directionX < -0.05f && FacingDirection == 1)
        {
            FlipSprite();
        }
    }

    public void FlipSprite()
    {
        FacingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= Mathf.Abs(scale.x) * FacingDirection;
        transform.localScale = scale;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Ensure the game is running and the state machine is initialized
        if (!Application.isPlaying || StateMachine == null || StateMachine.CurrentEnemyState == null) return;

        // Set the color and style for the text
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.fontSize = 14;
        style.alignment = TextAnchor.MiddleCenter;

        // Draw the name of the current state slightly above the enemy's head
        Vector3 labelPosition = transform.position + Vector3.up * 5f;

        // GetType().Name cleanly strips away the namespace and just gives you "EnemyChaseState"
        string stateName = StateMachine.CurrentEnemyState.GetType().Name;

        UnityEditor.Handles.Label(labelPosition, stateName, style);

        //attack hitbox gizmos
        if (Config == null) return;

        // Determine which way the enemy is currently facing (default to 1 if not playing)
        int currentFacingDirection = Application.isPlaying ? FacingDirection : 1;

        // Calculate the same hitbox center as the attack state
        Vector2 hitboxCenter = (Vector2)transform.position + new Vector2(
            Config.attackHitboxOffset.x * currentFacingDirection,
            Config.attackHitboxOffset.y
        );

        // Draw the rectangle in the Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(hitboxCenter, Config.attackHitboxSize);
    }
#endif

}
