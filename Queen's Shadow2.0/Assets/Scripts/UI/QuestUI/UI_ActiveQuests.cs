using System.Collections.Generic;
using UnityEngine;

public class UI_ActiveQuests : MonoBehaviour
{
    private Player_QuestManager questManger;
    private UI_ActiveQuestSlot[] questsSlot;

    private void Awake()
    {
        questManger = Player.instance.questManager;
        questsSlot = GetComponentsInChildren<UI_ActiveQuestSlot>(true);
    }

    private void OnEnable()
    {
        List<QuestData> quests = questManger.activeQuests;

        foreach (var slot in questsSlot)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < quests.Count; i++)
        {
            questsSlot[i].gameObject.SetActive(true);
            questsSlot[i].SetupActiveQuestSlot(quests[i]);
        }

        if (quests.Count > 0)
            questsSlot[0].SetupPreviwBTN();
    }
}
