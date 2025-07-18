using UnityEngine;

[System.Serializable]
public class QuestData 
{
    public QuestDataSO questSO;
    public int currentAmount;
    public bool canGetReward;

    public void AddQuestProgress(int amount = 1)
    {
        currentAmount = currentAmount + amount;
        canGetReward = CanGetReward();
    }

    public bool CanGetReward() => currentAmount >= questSO.requiredAmount;

    public QuestData(QuestDataSO questSO)
    {
        this.questSO = questSO; 
    }
}
