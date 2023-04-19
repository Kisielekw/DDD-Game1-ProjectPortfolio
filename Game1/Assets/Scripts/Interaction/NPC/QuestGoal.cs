using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The goal of a quest
/// </summary>
[System.Serializable]
public class QuestGoal
{
    /// <summary>
    /// What type of Quest is it
    /// </summary>
    public GoalType Type;
    /// <summary>
    /// What amout of Items/Kills are required to compleat
    /// </summary>
    public int RequiredAmount;
    /// <summary>
    /// What is the current amout of Items/Kills
    /// </summary>
    public int CurrentAmount;
    /// <summary>
    /// What item is needed
    /// </summary>
    public Item ItemNeeded;

    /// <summary>
    /// Checks if the The current amount of Items/Kills is equal to or more than the required 
    /// </summary>
    /// <returns></returns>
    public bool IsReached()
    {
        return RequiredAmount <= CurrentAmount;
    }
}

/// <summary>
/// The Type of quests available
/// </summary>
public enum GoalType
{
    Fetch,
    Kill
}
