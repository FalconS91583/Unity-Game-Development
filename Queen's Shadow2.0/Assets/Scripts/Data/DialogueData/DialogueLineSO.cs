using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/DialogueData/New Line Data", fileName = "Line - ")]
public class DialogueLineSO : ScriptableObject
{
    [Header("Dialogue Info")]
    public string dialogueGroupName;
    public DialogueSpeakerSO speaker;

    [Header("Text Options")]
    [TextArea] public string[] textLine;

    [Header("Choices Info")]
    [TextArea] public string playerChoiceAnswer;
    public DialogueLineSO[] choiceLines;

    [Header("Dialogue action")]
    [TextArea] public string actionLine;
    public DialogueActionType actionType;

    public string GetFirstLine() => textLine[0];

    public string GetRandomLine()
    {
       return textLine[Random.Range(0, textLine.Length)];
    }
}
