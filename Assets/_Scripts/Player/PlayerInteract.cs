using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject interactionIcon;

    private Vector3 interactionIconOriginalScale;

    private IInteractable currentInteractable;
    private IInteractable previousInteractable;

    private void Start()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
            interactionIconOriginalScale = interactionIcon.transform.localScale;
        }
    }

    private void Update()
    {
        // Get interact input once per frame
        bool interactPressed = GetInteractInput();

        // If dialogue is active, pressing interact advances the dialogue instead
        if (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive())
        {
            if (interactPressed)
            {
                DialogueManager.instance.AdvanceDialogue();
            }

            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false);
            }

            return; // Don't detect new interactables while in dialogue
        }

        DetectInteractable();
        KeepInteractionIconFacingCorrectly();

        // Log only when state changes
        if (currentInteractable != previousInteractable)
        {
            if (currentInteractable != null)
            {
                Debug.Log(
                    "[PlayerInteract] Interactable FOUND: " +
                    ((MonoBehaviour)currentInteractable).gameObject.name
                );
            }
            else
            {
                Debug.Log("[PlayerInteract] Interactable LOST — nothing in range");
            }

            previousInteractable = currentInteractable;
        }

        if (currentInteractable != null && interactPressed)
        {
            Debug.Log("[PlayerInteract] E pressed → calling Interact()");
            currentInteractable.Interact(null);
        }
    }

    private bool GetInteractInput()
    {
        if (InputManager.Instance != null)
        {
            return InputManager.Instance.PlayerInteract();
        }
        else
        {
            return Input.GetKeyDown(KeyCode.E);
        }
    }

    private void DetectInteractable()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, 2, 0), interactionRadius, interactableLayer);

        currentInteractable = null;

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactionIcon.SetActive(true);
                currentInteractable = interactable;
                break;
            }
        }
    }

    private void KeepInteractionIconFacingCorrectly()
    {
        if (interactionIcon == null)
            return;

        // Get the player's current facing direction.
        float playerScaleX = transform.localScale.x;

        // If the player is flipped, compensate the icon's X scale.
        float direction = playerScaleX < 0 ? -1f : 1f;

        Vector3 scale = interactionIconOriginalScale;
        scale.x *= direction;

        interactionIcon.transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 2, 0), interactionRadius);
    }
}
