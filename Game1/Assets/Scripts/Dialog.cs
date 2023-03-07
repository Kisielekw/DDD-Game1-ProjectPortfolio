using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string[] Speech;

    public Quest Quest;

    public Dialog(string[] speech, Quest quest = null)
    {
        Speech = speech;
        Quest = quest;
    }
}
