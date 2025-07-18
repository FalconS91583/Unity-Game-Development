using System.Collections.Generic;
using UnityEngine;

public class Player_QuestManager : MonoBehaviour, ISaveable
{
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;

    [SerializeField] private QuestDatabase questDatabase;
    private Entity_DropManager dropManager;
    private Inventory_Player inventory;

    private void Awake()
    {
        dropManager = GetComponent<Entity_DropManager>();
        inventory = GetComponent<Inventory_Player>();
    }

    public void TryGiveRewardFrom(RewardType npcType)
    {
        List<QuestData> getRewardQuest = new List<QuestData>();

        foreach (var quest in activeQuests)
        {


            if (quest.CanGetReward() && quest.questSO.rewardType == npcType)
            {
                getRewardQuest.Add(quest);
            }
        }

        foreach (var quest in getRewardQuest)
        {
            GiveQuestReward(quest.questSO);
            CompleteQuest(quest);
        }
    }

    private void GiveQuestReward(QuestDataSO questDataSO)
    {
        foreach (var item in questDataSO.rewardItems)
        {
            if (item == null || item.itemData == null) continue;

            for (int i = 0; i < item.stackSize; i++)
            {
                dropManager.CreateItemDrop(item.itemData);
            }
        }
    }

    public void AddProgress(string questtargetID, int amount = 1)
    {
        List<QuestData> getRewardQuest = new List<QuestData>();

        foreach (var quest in activeQuests)
        {
            if (quest.questSO.questTargetID != questtargetID) continue;

            if (quest.CanGetReward() == false)
                quest.AddQuestProgress(amount);

            if (quest.questSO.rewardType == RewardType.None && quest.CanGetReward())
            {
                getRewardQuest.Add(quest);
            }
        }

        foreach (var quest in getRewardQuest)
        {
            GiveQuestReward(quest.questSO);
            CompleteQuest(quest);
        }
    }

    public int GetQuestProgress(QuestData questToCheck)
    {
        QuestData quest = activeQuests.Find(q => q == questToCheck);

        return quest != null ? quest.currentAmount : 0;
    }

    public void AcceptQuest(QuestDataSO questSo)
    {
        activeQuests.Add(new QuestData(questSo));
    }

    public void CompleteQuest(QuestData questData)
    {
        completedQuests.Add(questData);
        activeQuests.Remove(questData);
    }

    public bool QuestTaken(QuestDataSO questToCheck)
    {
        if (questToCheck == null)
            return false;

        return activeQuests.Find(q => q.questSO == questToCheck) != null;
    }

    public void LoadData(GameData data)
    {
        activeQuests.Clear();

        foreach (var entry in data.activeQuests)
        {
            string questSaveID = entry.Key;
            int progress = entry.Value;

            QuestDataSO questDataSO = questDatabase.GetQuestByID(questSaveID);

            if (questDataSO == null)
            {
                continue;
            }

            QuestData questToLoad = new QuestData(questDataSO);
            questToLoad.currentAmount = progress;

            activeQuests.Add(questToLoad);
        }
    }
    public bool HasCompletedQuest()
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            QuestData quest = activeQuests[i];  
            if (quest.questSO.questType == QuestType.Delivery)
            {
                var requiredItem = quest.questSO.itemToDeliver;
                var requiredAmount = quest.questSO.requiredAmount;

                if (inventory.HasItemAmount(requiredItem, requiredAmount))
                return true;
            }
            if(quest.CanGetReward())
                return true;
        }
        return false;
    }

    public void SaveData(ref GameData data)
    {
        data.activeQuests.Clear();
        foreach (var quest in activeQuests)
        {
            data.activeQuests.Add(quest.questSO.questSaveID, quest.currentAmount);
        }

        foreach (var quest in completedQuests)
        {
            data.completedQuest.Add(quest.questSO.questSaveID, true);
        }
    }
}

