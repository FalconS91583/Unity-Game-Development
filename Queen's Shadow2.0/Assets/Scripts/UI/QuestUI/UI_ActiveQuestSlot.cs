using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActiveQuestSlot : MonoBehaviour
{
    private QuestData questData;
    private UI_ActoveQuestPreview questPreview;

    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image[] questRewardPreview;


    public void SetupActiveQuestSlot(QuestData questToSetup)
    {
        questPreview = transform.root.GetComponentInChildren<UI_ActoveQuestPreview>();
        questData = questToSetup;

        questName.text = questToSetup.questSO.questName;

        Inventory_Item[] reward = questToSetup.questSO.rewardItems;

        foreach (var previwIcon in questRewardPreview)
        {
            previwIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < reward.Length; i++)
        {
            if (reward[i] == null) continue;
            Image previw = questRewardPreview[i];

            previw.gameObject.SetActive(true);
            previw.sprite = reward[i].itemData.itemIcon;
            previw.GetComponentInChildren<TextMeshProUGUI>().text = reward[i].stackSize.ToString();
        }
    }

    public void SetupPreviwBTN()
    {
        questPreview.SetupQuestPreviw(questData);
    }
}
