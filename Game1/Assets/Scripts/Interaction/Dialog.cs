using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing NPC dialog and any quests they give
/// </summary>
[System.Serializable]
public class Dialog
{
    /// <summary>
    /// Array containing the NPC's dialog 
    /// </summary>
    public string[] Speech;

    /// <summary>
    /// The Quest that the NPC give out to the playe after its dialog
    /// </summary>
    public Quest Quest;

    public Dialog(string[] speech, Quest quest = null)
    {
        Speech = speech;
        Quest = quest;
    }
}
