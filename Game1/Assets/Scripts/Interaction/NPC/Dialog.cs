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
    /// Dialog quest flag.
    /// </summary>
    /// <remarks>
    /// As <see cref="Quest"/> is Serializeable,
    /// dialogs created in the inspector cannot be null,
    /// this extra boolean is required.
    /// </remarks>
    /// <value>
    /// Does this dialog have an associated quest?
    /// </value>
    public bool HasQuest;

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
