using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    static private int questNum;

    private int QuestID;
    public int ID { get {  return QuestID; } }
    public string QuestName;
    public string QuestDescription;

    Quest(string questName, string questDescription)
    {
        QuestID = questNum++;
        QuestName = questName;
        QuestDescription = questDescription;
    }
}
