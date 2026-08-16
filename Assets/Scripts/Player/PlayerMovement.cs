using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator playerAnimator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void ReadInput()
    {
        if (InputManager.instance.GetPlayerMovement() != null)
        {
            moveInput = InputManager.instance.GetPlayerMovement();
        }
        else
        {
            moveInput = Vector2.zero;
        }

        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }

    private void MovePlayer()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        playerAnimator.SetBool("isWalking", rb.linearVelocity.magnitude > 0.01f);
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