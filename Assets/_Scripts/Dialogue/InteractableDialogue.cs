using UnityEngine;

public class InteractableDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue Settings")]
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private bool canInteractMultipleTimes = false;
    [SerializeField] private string interactionPrompt = "to interact";

    private bool hasInteracted = false;

    public void Interact(Player _)
    {
        Debug.Log("InteractableDialogue.Interact() called on " + gameObject.name);

        if (dialogueData == null)
        {
            Debug.LogWarning("DialogueData is null on " + gameObject.name);
            return;
        }

        Debug.Log("DialogueData found: " + dialogueData.name + " with " + dialogueData.GetLineCount() + " lines");

        if (!canInteractMultipleTimes && hasInteracted)
        {
            Debug.Log("Already interacted, skipping");
            return;
        }

        if (DialogueManager.instance == null)
        {
            Debug.LogError("DialogueManager not found in scene!");
            return;
        }

        Debug.Log("Starting dialogue...");
        DialogueManager.instance.StartDialogue(dialogueData);
        hasInteracted = true;
    }

    public void ResetInteraction()
    {
        hasInteracted = false;
    }

    public string GetInteractionPrompt()
    {
        return interactionPrompt;
    }
}
