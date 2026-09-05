using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region [Attributes]
    public int FacingDirection { get; private set; } = 1; //1 = kanan, -1 = kiri
    [field:SerializeField] public EnemyConfig Config { get; private set; }
    #endregion

    #region [Components]
    public Rigidbody2D Rigidbody { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Transform Target { get; private set; }
    public Animator Anim { get; private set; }
    public EnemyCombat Combat { get; private set; }
    public Health HealthComponent { get; private set; }
    private AnimationEventDetection eventDetection;
    #endregion

    #region [EnemyStateMachine]
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyEvadeState EvadeState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyDamagedState DamagedState { get; private set; }
    #endregion

    #region [Unity Lifecycle]
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Agent = GetComponent<NavMeshAgent>();
        Combat = GetComponent<EnemyCombat>();
        HealthComponent = GetComponent<Health>();

        Anim = GetComponentInChildren<Animator>();
        eventDetection = GetComponentInChildren<AnimationEventDetection>();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        Target = GameObject.FindGameObjectWithTag("Player").transform;

        StateMachine = new EnemyStateMachine();
        PatrolState = new EnemyPatrolState(this, StateMachine);
        EvadeState = new EnemyEvadeState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        DamagedState = new EnemyDamagedState(this, StateMachine);
    }

    private void OnEnable()
    {
        eventDetection.OnAnimationFinishedTriggered += OnAnimationFinished;
        HealthComponent.OnDamaged += HandleDamageTaken;
        HealthComponent.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        eventDetection.OnAnimationFinishedTriggered -= OnAnimationFinished;
        HealthComponent.OnDamaged -= HandleDamageTaken;
        HealthComponent.OnDeath -= HandleDeath;
    }

    private void Start()
    {
        StateMachine.Initialize(PatrolState);
    }

    private void Update() 
    { 
        StateMachine.CurrentEnemyState?.Update();
        UpdateFacingDirection();
        UpdateAnimations();
    }
    
    private void FixedUpdate() => StateMachine.CurrentEnemyState?.FixedUpdate();
    #endregion

    #region [Animation/Visual]
    private void UpdateFacingDirection()
    {
        float directionX = 0f;
        directionX = Target.position.x - transform.position.x;

        if (directionX > 0.05f && FacingDirection == -1)
        {
            FlipSprite();
        }
        else if (directionX < -0.05f && FacingDirection == 1)
        {
            FlipSprite();
        }
    }
    
    private void OnAnimationFinished()
    {
        StateMachine.CurrentEnemyState?.AnimationFinishTrigger();
    }

    private void UpdateAnimations()
    {
        bool isStandingStill = Agent.velocity.sqrMagnitude < 0.1f;

        if (StateMachine.CurrentEnemyState == DamagedState) return;

        Anim.SetBool("isIdling", isStandingStill);

        if (StateMachine.CurrentEnemyState != ChaseState) Anim.SetBool("isWalking", !isStandingStill);
        else Anim.SetBool("isRunning", !isStandingStill);
    }

    public void FlipSprite()
    {
        FacingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * FacingDirection;
        transform.localScale = scale;
    }
    #endregion

    #region Damaging
    private void HandleDamageTaken(Vector2 hitDirection, AttackType attackType)
    {
        float strength;
        float stunTime;

        if (attackType == AttackType.Heavy)
        {
            strength = Config.heavyKnockbackStrength;
            stunTime = Config.heavyStunDuration;
        }
        else
        {
            strength = Config.lightKnockbackStrength;
            stunTime = Config.lightStunDuration;
        }

        float directionX = hitDirection.x != 0 ? Mathf.Sign(hitDirection.x) : Mathf.Sign(transform.position.x - Target.position.x);

        Vector2 finalForce = new Vector2(directionX * strength, 0f);

        DamagedState.SetKnockbackForce(finalForce, stunTime);
        StateMachine.ChangeState(DamagedState);
    }

    private void HandleDeath()
    {
        CombatManager.Instance.ReleaseSlot(this);
        StateMachine.ChangeState(null);
        enabled = false;

        if (Agent != null && Agent.enabled)
        {
            Agent.isStopped = true;
            Agent.enabled = false;
        }

        if (Rigidbody != null)
        {
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.simulated = false;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        if (Anim != null) Anim.Play("TestEnemyDeath", -1, 0f);
    }
    #endregion

    #region [Helper]
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
    #endregion

}
