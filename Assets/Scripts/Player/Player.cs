using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator playerAnimator { get; set; }
    public PlayerCombat playerCombat { get; private set; }

    #region PlayerStateMachine Variables
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new();
        idleState = new(this, stateMachine);
        walkState = new(this, stateMachine);
        attackState = new(this, stateMachine);

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
        stateMachine.currentPlayerState.Update();

        if (InputManager.instance != null && InputManager.instance.PlayerInteract())
        {
            if (GameSceneManager.instance != null)
            {
                GameSceneManager.instance.RestartScene();
            }
        }
    }

    private void FixedUpdate()
    {
        stateMachine.currentPlayerState.FixedUpdate();
    }

    private void ReadInput()
    {
        moveInput = InputManager.instance.GetPlayerMovement();

        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    public bool GetAttackInput()
    {
        return InputManager.instance.GetPlayerAttack();
    }

    public void MovePlayer()
    {
        rb.linearVelocity = moveInput * moveSpeed;

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
}