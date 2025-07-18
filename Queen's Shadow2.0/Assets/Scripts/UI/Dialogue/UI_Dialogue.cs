using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : MonoBehaviour
{
    private UI ui;
    private DialogueNPCData npcData;
    private Player_QuestManager questManager;

    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakterName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI[] dialogueChoicesText;

    [SerializeField] private float textSpeed = .1f;
    private string fullText;
    private Coroutine typingCo;

    private DialogueLineSO currentLine;
    private DialogueLineSO[] currentChoices;
    private DialogueLineSO selectedChoices;
    private int selectedChoiceIndex;

    private bool waitingToConfirm;
    private bool canInteract;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        questManager = Player.instance.questManager;
    }

    public void SetupNPCData(DialogueNPCData npcData)
    {
        this.npcData = npcData;
    }

    public void PlayDialogueLine(DialogueLineSO line)
    {
        currentLine = line;
        currentChoices = line.choiceLines;
        canInteract = false;
        selectedChoices = null;
        selectedChoiceIndex = 0;
        HideAllChoices();

        speakerPortrait.sprite = line.speaker.speakerPortrait;
        speakterName.text = line.speaker.speakerName;
        fullText = line.actionType == DialogueActionType.None || line.actionType == DialogueActionType.PlayerMakeChoice ?
            line.GetRandomLine() : line.actionLine;

        typingCo = StartCoroutine(TypeTextCo(fullText));
        StartCoroutine(EnableInteractionCo());
    }

    private void HandleNextAction()
    {
        switch (currentLine.actionType)
        {
            case DialogueActionType.OpenShop:
                ui.SwitchToInGameUI();
                ui.OpenMerchantUI(true);
                break;
            case DialogueActionType.PlayerMakeChoice:
                if (selectedChoices == null)
                {
                    ShowChoices();
                }
                else
                {
                    DialogueLineSO selectedChoice = currentChoices[selectedChoiceIndex];
                    PlayDialogueLine(selectedChoice);
                }
                break;
            case DialogueActionType.OpenQuest:
                ui.SwitchToInGameUI();
                ui.OpenQustUI(npcData.quests);
                break;
            case DialogueActionType.GetQuestReward:
                ui.SwitchToInGameUI();
                questManager.TryGiveRewardFrom(npcData.npcRewardType);
                break;
            case DialogueActionType.OpenCraft:
                ui.SwitchToInGameUI();
                ui.OpenCraftUI(true);
                break;
            case DialogueActionType.Close:
                ui.SwitchToInGameUI();
                break;
        }

    }

    public void DialogueInteraction()
    {
        if (canInteract == false) return;

        if (typingCo != null)
        {
            CompleteTyping();
            if (currentLine.actionType != DialogueActionType.PlayerMakeChoice)
                waitingToConfirm = true;
            else
                HandleNextAction();

            return;
        }

        if (waitingToConfirm || selectedChoices != null)
        {
            waitingToConfirm = false;
            HandleNextAction();
        }
    }

    private void CompleteTyping()
    {
        if (typingCo != null)
        {
            StopCoroutine(typingCo);
            dialogueText.text = fullText;
            typingCo = null;
        }
    }

    private void ShowChoices()
    {
        for (int i = 0; i < dialogueChoicesText.Length; i++)
        {
            if (i < currentChoices.Length)
            {
                DialogueLineSO choice = currentChoices[i];
                string choiceText = choice.playerChoiceAnswer;

                dialogueChoicesText[i].gameObject.SetActive(true);
                dialogueChoicesText[i].text = selectedChoiceIndex == i ? $"<color=yellow> {i + 1}) {choiceText}"
                    : $"{i + 1}) {choiceText}";

                if (choice.actionType == DialogueActionType.GetQuestReward && questManager.HasCompletedQuest() == false)
                    dialogueChoicesText[i].gameObject.SetActive(false);
            }
            else
            {
                dialogueChoicesText[i].gameObject.SetActive(false);
            }
        }
        selectedChoices = currentChoices[selectedChoiceIndex];
    }

    private void HideAllChoices()
    {
        foreach (var obj in dialogueChoicesText)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public void NavigateChoice(int direction)
    {
        if (currentChoices == null || currentChoices.Length <= 1)
            return;

        selectedChoiceIndex = selectedChoiceIndex + direction;
        selectedChoiceIndex = Mathf.Clamp(selectedChoiceIndex, 0, currentChoices.Length - 1);
        ShowChoices();
    }

    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text = dialogueText.text + letter;
            yield return new WaitForSeconds(textSpeed);
        }

        if (currentLine.actionType != DialogueActionType.PlayerMakeChoice)
        {
            waitingToConfirm = true;

        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            selectedChoices = null;
            HandleNextAction();
        }

        typingCo = null;
    }

    private IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract = true;
    }
}
