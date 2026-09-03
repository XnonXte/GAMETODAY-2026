using System;
using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Player Attribute
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator playerAnimator { get; set; }
    public PlayerCombat playerCombat { get; private set; }

    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 18f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.8f;

    //private attribute
    private float currentDashCooldown;
    private Vector2 lastMoveDirection = Vector2.right;
    #endregion

    #region PlayerStateMachine Variables

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerDashState dashState { get; private set; }

    #endregion

    private void Awake()
    {
        stateMachine = new();
        idleState = new(this, stateMachine);
        walkState = new(this, stateMachine);
        attackState = new(this, stateMachine);
        dashState = new(this, stateMachine);

        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        ReadInput();
        stateMachine.CurrentPlayerState.Update();

        if (InputManager.instance != null && InputManager.instance.PlayerInteract())
        {
            if (GameSceneManager.instance != null)
            {
                GameSceneManager.instance.RestartScene();
            }
        }

        if (currentDashCooldown > 0f)
        {
            currentDashCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentPlayerState.FixedUpdate();
    }

    private void ReadInput()
    {
        moveInput = InputManager.instance.GetPlayerMovement();

        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }

    public void MovePlayer()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        if (moveInput.sqrMagnitude > .01f) lastMoveDirection = moveInput.normalized;

        if (moveInput.x > .1f) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < -.1f) transform.localScale = new Vector3(-1, 1, 1);
    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = Mathf.Max(0f, speed);
    }

    public Vector2 GetCurrentVelocity()
    {
        return rb.linearVelocity;
    }

    public float GetDashDuration()
    {
        return dashDuration;
    }

    public bool CanDash()
    {
        return currentDashCooldown <= 0f;
    }

    public void StartDash()
    {
        currentDashCooldown = dashCooldown;

        rb.linearVelocity = lastMoveDirection * dashForce;
    }

    public void StopDash()
    {
        rb.linearVelocity = Vector2.zero;
    }


    public void playerAttackAnimationHit()
    {
        if (stateMachine.CurrentPlayerState is PlayerAttackState attackState)
        {
            attackState.AnimationAttackHit();
        }
    }

    public void playerAttackOpenComboWindow()
    {
        if (stateMachine.CurrentPlayerState is PlayerAttackState attackState)
        {
            attackState.OpenComboWindow();
        }
    }

    public void playerAttackFinished()
    {
        if (stateMachine.CurrentPlayerState is PlayerAttackState attackState)
        {
            attackState.AttackFinished();
        }
    }

    public void playerAttackComboFinished()
    {
        if (stateMachine.CurrentPlayerState is PlayerAttackState attackState)
        {
            attackState.ComboFinished();
        }
    }
}