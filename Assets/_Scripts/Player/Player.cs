using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region [Attributes]
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    private Vector2 moveInput;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 18f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.8f;

    private float currentDashCooldown;
    private Vector2 lastMoveDirection = Vector2.right;
    [SerializeField] private ParticleSystem dustParticle;

    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.15f;
    private Material defaultMaterial;
    private Coroutine flashCoroutine;
    #endregion

    #region [Components]
    [field: SerializeField] public Animator SwordAnim { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public Health HealthComponent { get; private set; }

    [Header("Animation Event Detectors")]
    [SerializeField] private AnimationEventDetection bodyEventDetection;
    [SerializeField] private AnimationEventDetection swordEventDetection;
    [SerializeField] private SpriteRenderer spriteRenderer;
    #endregion

    #region [PlayerStateMachine]
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerDamagedState DamagedState { get; private set; }
    public bool IsMovementLocked { get; set; } = false;
    public bool IsAttackLocked { get; set; } = false;
    public bool IsAbilityLocked { get; set; } = false;
    #endregion

    #region [Unity Lifecycle]
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Combat = GetComponent<PlayerCombat>();
        HealthComponent = GetComponent<Health>();

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        WalkState = new PlayerWalkState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
        DashState = new PlayerDashState(this, StateMachine);
        DamagedState = new PlayerDamagedState(this, StateMachine);
    }

    private void OnEnable()
    {
        HealthComponent.OnDamaged += HandleDamageTaken;
        HealthComponent.OnDeath += HandleDeath;

        swordEventDetection.OnAnimationAttackTriggered += HandleAttackEvent;
        swordEventDetection.OnAnimationFinishedTriggered += HandleAnimationFinished;
    }

    private void OnDisable()
    {
        HealthComponent.OnDamaged -= HandleDamageTaken;
        HealthComponent.OnDeath -= HandleDeath;

        swordEventDetection.OnAnimationAttackTriggered -= HandleAttackEvent;
        swordEventDetection.OnAnimationFinishedTriggered -= HandleAnimationFinished;
    }

    private void Start()
    {
        if (spriteRenderer != null) defaultMaterial = spriteRenderer.material;
        StateMachine.Initialize(IdleState);

        if (GameSessionManager.Instance != null && GameSessionManager.Instance.savedPlayerHealth > 0)
        {
            HealthComponent.LoadSavedHealth(GameSessionManager.Instance.savedPlayerHealth);
        }
    }

    private void Update()
    {
        if (IsAbilityLocked) return;

        if (!enabled) return;

        ReadInput();
        StateMachine.CurrentPlayerState?.Update();

        if (!IsAbilityLocked)
        {
            StateMachine.CurrentPlayerState?.Update();
        }

        if (currentDashCooldown > 0f)
        {
            currentDashCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!IsAbilityLocked) StateMachine.CurrentPlayerState?.FixedUpdate();
    }
    #endregion

    #region [Movement & Logic]
    private void ReadInput()
    {
        if (IsMovementLocked)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = InputManager.Instance.GetPlayerMovement();
        if (moveInput.magnitude > 1f) moveInput.Normalize();
    }

    public void MovePlayer()
    {
        Rigidbody.linearVelocity = moveInput * moveSpeed;

        if (moveInput.sqrMagnitude > .01f) lastMoveDirection = moveInput.normalized;
        dustParticle.Play();

        if (moveInput.x > .1f) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < -.1f) transform.localScale = new Vector3(-1, 1, 1);
    }

    public void StopMovement()
    {
        Rigidbody.linearVelocity = Vector2.zero;
    }

    public void SetMoveSpeed(float speed) => moveSpeed = Mathf.Max(0f, speed);
    public Vector2 GetCurrentVelocity() => Rigidbody.linearVelocity;
    public float GetDashDuration() => dashDuration;
    public bool CanDash() => currentDashCooldown <= 0f && !IsMovementLocked;

    public void StartDash()
    {
        currentDashCooldown = dashCooldown;
        AudioManager.Instance.PlayAudio(AudioManager.Instance.SFX_Dash);
        Rigidbody.linearVelocity = lastMoveDirection * dashForce;
    }

    public void StopDash()
    {
        Rigidbody.linearVelocity = Vector2.zero;
    }
    #endregion

    #region [Damaging]
    private void HandleAttackEvent()
    {
        if (StateMachine.CurrentPlayerState == AttackState)
        {
            AttackState.TriggerAttackHitbox();
        }
    }

    private void HandleAnimationFinished()
    {
        if (StateMachine.CurrentPlayerState == AttackState)
        {
            AttackState.AnimationFinishTrigger();
        }
    }

    private void HandleDamageTaken(Vector2 hitDirection, AttackType attackType)
    {
        // Define strengths based on the incoming attack type
        float strength = (attackType == AttackType.Heavy) ? 10f : 5f;
        float stunTime = (attackType == AttackType.Heavy) ? 0.6f : 0.4f;

        float directionX = hitDirection.x != 0 ? Mathf.Sign(hitDirection.x) : (transform.localScale.x * -1f);
        Vector2 finalForce = new Vector2(directionX * strength, 0f);

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());

        DamagedState.SetKnockbackForce(finalForce, stunTime);
        StateMachine.ChangeState(DamagedState);
    }

    private void HandleDeath()
    {
        StateMachine.ChangeState(null);
        enabled = false;

        StopMovement();

        if (Rigidbody != null) Rigidbody.simulated = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (Anim != null) Anim.Play("TestDeathAnimation", -1, 0f);

        AudioManager.Instance.PlayAudio(AudioManager.Instance.UI_Gameover);
        EventHandler.WhenGameLose();
    }

    private IEnumerator FlashRoutine()
    {
        if (spriteRenderer != null && flashMaterial != null)
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.material = defaultMaterial;
        }
    }
    #endregion

    #region [Status Effect]
    private float originalMoveSpeed;
    private Coroutine activeSpeedCoroutine;
    private Coroutine activePowerCoroutine;

    public void ApplyHeal(float amount)
    {
        Debug.Log("Apply Heal!");
        HealthComponent.ChangeHealth(amount, Vector3.zero, AttackType.None);
    }

    public void ApplySpeedBoost(float boostAmount, float duration)
    {
        Debug.Log("Apply Speed Boost!");

        if (activeSpeedCoroutine != null)
        {
            StopCoroutine(activeSpeedCoroutine);
            moveSpeed = originalMoveSpeed;
        }
        else
        {
            originalMoveSpeed = moveSpeed;
        }

        activeSpeedCoroutine = StartCoroutine(SpeedBoostRoutine(boostAmount, duration));
    }

    public void ApplyPowerBoost(float multiplier, float duration)
    {
        Debug.Log("Applied Power Boost!");

        if (activePowerCoroutine != null)
        {
            StopCoroutine(activePowerCoroutine);
            Combat.DamageMultiplier = 1f;
        }

        activePowerCoroutine = StartCoroutine(PowerBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boostAmount, float duration)
    {
        moveSpeed += boostAmount;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed;
        activeSpeedCoroutine = null;
    }

    private IEnumerator PowerBoostRoutine(float multiplier, float duration)
    {
        Combat.DamageMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        Combat.DamageMultiplier = 1f;

        activePowerCoroutine = null;
    }
    #endregion

}