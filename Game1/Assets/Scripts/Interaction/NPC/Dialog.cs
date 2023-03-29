using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
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

    public Quest Quest;

    public Dialog(string[] speech, Quest quest = null)
    {
        Speech = speech;
        Quest = quest;
    }
}
