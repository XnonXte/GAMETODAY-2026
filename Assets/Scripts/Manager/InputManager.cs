using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private InputSystem_Actions playerControls;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool GetPlayerAttack()
    {
        return playerControls.Player.Attack.triggered;
    }

    public bool PlayerInteract()
    {
        return playerControls.Player.Interact.WasPressedThisFrame();
    }
}