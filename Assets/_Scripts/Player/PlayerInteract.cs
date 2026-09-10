using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private Vector3 iconOffset = new Vector3(0f, 1.5f, 0f);
    [SerializeField] private string iconText = "E to inspect";

    private Vector3 interactionIconOriginalScale;

    private IInteractable currentInteractable;
    private IInteractable previousInteractable;

    private void Start()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
            interactionIconOriginalScale = interactionIcon.transform.localScale;
            
            // Detach icon from player so it moves independently
            if (interactionIcon.transform.parent == transform)
            {
                interactionIcon.transform.SetParent(null);
            }
        }
    }

    private void Update()
    {
        // Get interact input once per frame
        bool interactPressed = GetInteractInput();

        // If dialogue is active, pressing interact closes the dialogue
        if (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive())
        {
            if (interactPressed)
            {
                DialogueManager.instance.AdvanceDialogue();
            }

            // Keep icon visible above the interactable object (static, not following player)
            if (interactionIcon != null && currentInteractable != null)
            {
                interactionIcon.SetActive(true);
                Transform targetTransform = ((MonoBehaviour)currentInteractable).transform;
                interactionIcon.transform.position = targetTransform.position + iconOffset;
                // Don't call KeepInteractionIconFacingCorrectly() here - icon stays static
            }

            return; // Don't detect new interactables while in dialogue
        }

        DetectInteractable();
        // Don't call KeepInteractionIconFacingCorrectly() - icon stays static, not affected by player direction

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

    private void UpdateIconText()
    {
        if (interactionIcon == null) return;

        // Try TextMeshPro (World Space)
        TextMeshPro tmp3D = interactionIcon.GetComponentInChildren<TextMeshPro>(true);
        if (tmp3D != null)
        {
            tmp3D.text = iconText;
            return;
        }

        // Try TextMeshProUGUI (UI Canvas)
        TextMeshProUGUI tmpUI = interactionIcon.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmpUI != null)
        {
            tmpUI.text = iconText;
            return;
        }

        // Try standard UI Text
        Text uiText = interactionIcon.GetComponentInChildren<Text>(true);
        if (uiText != null)
        {
            uiText.text = iconText;
        }
    }

    private void DetectInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, 2, 0), interactionRadius, interactableLayer);

        currentInteractable = null;
        string currentPrompt = null;

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                currentPrompt = interactable.GetInteractionPrompt();
                break;
            }
        }

        // Only show interaction icon if the interactable provides a prompt
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(currentPrompt != null);
            if (currentPrompt != null && currentInteractable != null)
            {
                iconText = currentPrompt;
                UpdateIconText();

                // Position icon ABOVE THE INTERACTABLE OBJECT (not player)
                Transform targetTransform = ((MonoBehaviour)currentInteractable).transform;
                interactionIcon.transform.position = targetTransform.position + iconOffset;
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
