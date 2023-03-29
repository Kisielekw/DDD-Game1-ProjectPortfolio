using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    static private int questNum;

    private int QuestID;
    public QuestGoal Goal;
    public int ID { get {  return QuestID; } }
    public string QuestName;
    public string QuestDescription;
    public bool isCompleat;
    public bool isStarted;

    Quest(string questName, string questDescription)
    {
        QuestID = questNum++;
        QuestName = questName;
        QuestDescription = questDescription;
    }

    public void UpdateKill()
    {
        if (Goal.Type == GoalType.Kill) Goal.CurrentAmount++;
        isCompleat = Goal.IsReached();
    }

    public void UpdateItem(Item pItem)
    {
        if (pItem == Goal.ItemNeeded && Goal.Type == GoalType.Fetch) Goal.CurrentAmount++;
        isCompleat = Goal.IsReached();
    }

    public void UpdateItemInitial(List<ItemNumber> pCurrentInv)
    {
        foreach(ItemNumber iN in  pCurrentInv)
        {
            if(iN.item == Goal.ItemNeeded)
            {
                Goal.CurrentAmount = iN.Number;
                break;
            }
        }

        Goal.IsReached();
    }
}
