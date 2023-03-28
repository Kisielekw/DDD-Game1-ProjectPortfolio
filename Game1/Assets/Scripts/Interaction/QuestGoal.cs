using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{

    public GoalType Type;
    public int RequiredAmount;
    public int CurrentAmount;
    public Item ItemNeeded;

    public bool IsReached()
    {
        return RequiredAmount <= CurrentAmount;
    }
}

public enum GoalType
{
    Fetch,
    Kill
}
