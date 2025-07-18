using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/DialogueData/New Speaker Data", fileName = "Speaker - ")]//speaker -> person who talk
public class DialogueSpeakerSO : ScriptableObject
{
    public string speakerName;
    public Sprite speakerPortrait;
}
