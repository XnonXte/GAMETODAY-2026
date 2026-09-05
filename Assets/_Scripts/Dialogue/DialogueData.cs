using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;
        [TextArea(3, 5)] public string lineText;
    }

    public DialogueLine[] dialogueLines;

    public int GetLineCount()
    {
        return dialogueLines != null ? dialogueLines.Length : 0;
    }
}
