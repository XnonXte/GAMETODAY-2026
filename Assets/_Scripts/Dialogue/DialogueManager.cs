using UnityEngine;
using System.Collections;

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

    // Keep track of the current typing coroutine
    private Coroutine typingCoroutine;

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
        Debug.Log("StartDialogue called with: " +
            (dialogue != null ? dialogue.name : "null"));

        if (dialogue == null || dialogue.GetLineCount() == 0)
        {
            Debug.LogWarning("Dialogue is null or empty");
            return;
        }

        // Stop any previous typing coroutine
        StopTypingCoroutine();

        // Reset dialogue completely
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isTyping = false;
        isDialogueActive = true;

        Debug.Log("Starting dialogue from line 0...");

        ShowCurrentLine();
    }

    public void AdvanceDialogue()
    {
        Debug.Log("AdvanceDialogue called, isActive: " + isDialogueActive);

        if (!isDialogueActive || currentDialogue == null)
        {
            return;
        }

        // If current line is still typing,
        // finish the line instead of advancing
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
        if (currentDialogue == null ||
            currentLineIndex >= currentDialogue.GetLineCount())
        {
            return;
        }

        // Make sure the previous typing coroutine is stopped
        StopTypingCoroutine();

        DialogueData.DialogueLine line =
            currentDialogue.dialogueLines[currentLineIndex];

        Debug.Log(
            "Showing line " + currentLineIndex +
            ": " + line.speakerName +
            " - " + line.lineText
        );

        if (dialogueUI != null)
        {
            // Clear the text first
            dialogueUI.ShowDialogue(line.speakerName, "");

            // Start typing the new line
            typingCoroutine = StartCoroutine(TypeText(line.lineText));
        }
        else
        {
            Debug.LogError("dialogueUI is null, cannot show dialogue!");
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;

        string currentText = "";

        foreach (char c in text)
        {
            if (!isTyping)
            {
                yield break;
            }

            currentText += c;

            if (dialogueUI != null &&
                currentDialogue != null &&
                currentLineIndex < currentDialogue.GetLineCount())
            {
                DialogueData.DialogueLine line =
                    currentDialogue.dialogueLines[currentLineIndex];

                dialogueUI.ShowDialogue(
                    line.speakerName,
                    currentText
                );
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        // Typing finished normally
        isTyping = false;
        typingCoroutine = null;

        if (dialogueUI != null &&
            currentDialogue != null &&
            currentLineIndex < currentDialogue.GetLineCount())
        {
            DialogueData.DialogueLine line =
                currentDialogue.dialogueLines[currentLineIndex];

            dialogueUI.ShowDialogue(
                line.speakerName,
                line.lineText
            );
        }
    }

    private void CompleteCurrentLine()
    {
        if (!isTyping)
        {
            return;
        }

        isTyping = false;

        // Stop only the typing coroutine
        StopTypingCoroutine();

        if (currentDialogue != null &&
            currentLineIndex < currentDialogue.GetLineCount())
        {
            DialogueData.DialogueLine line =
                currentDialogue.dialogueLines[currentLineIndex];

            if (dialogueUI != null)
            {
                dialogueUI.ShowDialogue(
                    line.speakerName,
                    line.lineText
                );
            }
        }
    }

    private void StopTypingCoroutine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
    }

    public void EndDialogue()
    {
        Debug.Log("Ending dialogue");

        StopTypingCoroutine();

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