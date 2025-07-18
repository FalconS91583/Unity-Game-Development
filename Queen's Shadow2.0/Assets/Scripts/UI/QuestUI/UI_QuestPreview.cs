using TMPro;
using UnityEngine;

public class UI_QuestPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [SerializeField] private UI_QuestRewardSlot[] questReward;

    [SerializeField] private GameObject[] additionalObjects;
    private UI_Quest questUI;
    private QuestDataSO currentQuest;
    public void SetupQuestPreview(QuestDataSO questDataSO)
    {
        questUI = transform.root.GetComponentInChildren<UI_Quest>();
        currentQuest = questDataSO;

        EnableAdditionalObjects(true);
        EnableQuestrewardObejcts(false);

        questName.text = questDataSO.questName;
        questDescription.text = questDataSO.description;
        questGoal.text = questDataSO.questGoal + " " + questDataSO.requiredAmount;


        for (var i = 0; i < questDataSO.rewardItems.Length; i++)
        {
            Inventory_Item rewardItem = new Inventory_Item(questDataSO.rewardItems[i].itemData);
            rewardItem.stackSize = questDataSO.rewardItems[i].stackSize;

            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(rewardItem);
        }
    }

    public void AccepctQuestBTN()
    {
        MakeQuestPreviewEmpty();

        questUI.questManager.AcceptQuest(currentQuest);
        questUI.UpdateQuestList();
    }

    public void MakeQuestPreviewEmpty()
    {
        questName.text = "";
        questDescription.text = "";

        EnableAdditionalObjects(false);

        EnableQuestrewardObejcts(false);
    }

    private void EnableAdditionalObjects(bool enable)
    {
        foreach (var obj in additionalObjects)
        {
            obj.SetActive(enable);
        }
    }

    private void EnableQuestrewardObejcts(bool enable)
    {
        foreach (var obj in questReward)
        {
            obj.gameObject.SetActive(enable);
        }
    }
}
