using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private InputSystem_Actions playerControls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        if (playerControls != null)
        {
            playerControls.Enable();
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }

    public Vector2 GetPlayerMovement()
    {
        if (playerControls == null) return Vector2.zero;
        return playerControls.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        if (playerControls == null) return Vector2.zero;
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool GetPlayerAttack()
    {
        if (playerControls == null) return false;
        return playerControls.Player.Attack.triggered;
    }

    public bool PlayerInteract()
    {
        if (playerControls == null) return false;
        return playerControls.Player.Interact.triggered;
    }

    public bool GetPlayerDash()
    {
        if (playerControls == null) return false;
        return playerControls.Player.Dash.WasPressedThisFrame();
    }
}
