using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the information about a quest
/// </summary>
[System.Serializable]
public class Quest
{
    //iterates everytime a quest is created
    static private int questNum;

    /// <summary>
    /// The numerical ID of the quest
    /// </summary>
    [SerializeField]
    private int QuestID;
    /// <summary>
    /// The Goal of the quest
    /// </summary>
    public QuestGoal Goal;
    /// <summary>
    /// The Numerical ID of the quest (getter)
    /// </summary>
    public int ID { get {  return QuestID; } }
    /// <summary>
    /// The name of the Quest
    /// </summary>
    public string QuestName;
    /// <summary>
    /// A description of the quest
    /// </summary>
    public string QuestDescription;
    /// <summary>
    /// Checks if the quest is compleated
    /// </summary>
    public bool isCompleat;
    /// <summary>
    /// Checks if the Quest has been started
    /// </summary>
    public bool isStarted;

    /// <summary>
    /// The Quest constructor
    /// </summary>
    /// <param name="questName">The name of the Quest</param>
    /// <param name="questDescription">The description of the quest</param>
    /// <peram name="goal">The type of quest this is</peram>
    Quest(string questName, string questDescription, QuestGoal goal)
    {
        QuestID = questNum++;
        QuestName = questName;
        QuestDescription = questDescription;
        Goal = goal;
    }

    /// <summary>
    /// Updates the Kill Quest
    /// </summary>
    public void UpdateKill()
    {
        if (Goal.Type == GoalType.Kill) Goal.CurrentAmount++;
        isCompleat = Goal.IsReached();
    }

    /// <summary>
    /// Updates the Fetch Quest
    /// </summary>
    /// <param name="pItem">The item the player picked up</param>
    public void UpdateItem(Item pItem)
    {
        if (pItem == Goal.ItemNeeded && Goal.Type == GoalType.Fetch) Goal.CurrentAmount++;
        isCompleat = Goal.IsReached();
    }

    /// <summary>
    /// Checks the items in the players inventory when starting quest
    /// </summary>
    /// <param name="pCurrentInv">the players inventory</param>
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
