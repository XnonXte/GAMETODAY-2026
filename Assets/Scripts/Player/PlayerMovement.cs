using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 18f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.8f;
    [SerializeField] private LayerMask dashCollisionMask;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.right;
    private Animator playerAnimator;
    private bool isDashing;
    private float currentDashCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ReadInput();

        if (currentDashCooldown > 0f)
        {
            currentDashCooldown -= Time.deltaTime;
        }
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

        if (moveInput.magnitude > 0.1f)
        {
            lastMoveDirection = moveInput.normalized;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && currentDashCooldown <= 0f)
        {
            StartCoroutine(PerformDash());
        }
    }

    private System.Collections.IEnumerator PerformDash()
    {
        isDashing = true;
        currentDashCooldown = dashCooldown;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = lastMoveDirection * dashForce;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void MovePlayer()
    {
        if (isDashing)
        {
            playerAnimator.SetBool("isWalking", false);
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;

        playerAnimator.SetBool("isWalking", rb.linearVelocity.magnitude > 0.01f);

        if (moveInput.x > .1f) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < -.1f) transform.localScale = new Vector3(-1, 1, 1);
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