using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;
    private IInteractable previousInteractable;

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
            return; // Don't detect new interactables while in dialogue
        }

        DetectInteractable();

        // Log only when state changes (not every frame)
        if (currentInteractable != previousInteractable)
        {
            if (currentInteractable != null)
            {
                Debug.Log("[PlayerInteract] Interactable FOUND: " + ((MonoBehaviour)currentInteractable).gameObject.name);
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
            currentInteractable.Interact();
        }
    }

    private bool GetInteractInput()
    {
        if (InputManager.instance != null)
        {
            return InputManager.instance.PlayerInteract();
        }
        else
        {
            return Input.GetKeyDown(KeyCode.E);
        }
    }

    private void DetectInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

        currentInteractable = null;

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
