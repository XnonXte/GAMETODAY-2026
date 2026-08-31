using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue Settings")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("References")]
    [Tooltip("Assign the DialogueUI manually. If left empty, it will be found automatically.")]
    [SerializeField] private DialogueUI dialogueUI;

    private DialogueData currentDialogue;
    private int currentLineIndex;
    private bool isTyping;
    private bool isDialogueActive;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("DialogueManager instance created");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (dialogueUI == null)
        {
            // FindAnyObjectByType can find components on inactive GameObjects
            dialogueUI = FindAnyObjectByType<DialogueUI>(FindObjectsInactive.Include);
        }

        if (dialogueUI != null)
        {
            Debug.Log("DialogueUI found: " + dialogueUI.gameObject.name);
        }
        else
        {
            Debug.LogError("DialogueUI NOT found in scene! Make sure a GameObject with DialogueUI component exists.");
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        Debug.Log("StartDialogue called with: " + (dialogue != null ? dialogue.name : "null"));

        if (dialogue == null || dialogue.GetLineCount() == 0)
        {
            Debug.LogWarning("Dialogue is null or empty");
            return;
        }

        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        Debug.Log("Showing first line...");
        ShowCurrentLine();
    }

    public void AdvanceDialogue()
    {
        Debug.Log("AdvanceDialogue called, isActive: " + isDialogueActive);
        if (!isDialogueActive || currentDialogue == null)
        {
            return;
        }

        if (isTyping)
        {
            CompleteCurrentLine();
            return;
        }

        currentLineIndex++;

        if (currentLineIndex >= currentDialogue.GetLineCount())
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    private void ShowCurrentLine()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.GetLineCount())
        {
            return;
        }

        DialogueData.DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];
        Debug.Log("Showing line " + currentLineIndex + ": " + line.speakerName + " - " + line.lineText);

        if (dialogueUI != null)
        {
            dialogueUI.ShowDialogue(line.speakerName, "");
            StartCoroutine(TypeText(line.lineText));
        }
        else
        {
            Debug.LogError("dialogueUI is null, cannot show dialogue!");
        }
    }

    private System.Collections.IEnumerator TypeText(string text)
    {
        isTyping = true;

        string currentText = "";
        foreach (char c in text)
        {
            if (!isTyping)
            {
                break;
            }

            currentText += c;

            if (dialogueUI != null && currentDialogue != null && currentLineIndex < currentDialogue.GetLineCount())
            {
                dialogueUI.ShowDialogue(currentDialogue.dialogueLines[currentLineIndex].speakerName, currentText);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        CompleteCurrentLine();
    }

    private void CompleteCurrentLine()
    {
        isTyping = false;
        StopAllCoroutines();

        if (currentDialogue != null && currentLineIndex < currentDialogue.GetLineCount())
        {
            DialogueData.DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];
            if (dialogueUI != null)
            {
                dialogueUI.ShowDialogue(line.speakerName, line.lineText);
            }
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Ending dialogue");
        isDialogueActive = false;
        currentDialogue = null;
        currentLineIndex = 0;

        if (dialogueUI != null)
        {
            dialogueUI.HideDialogue();
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
