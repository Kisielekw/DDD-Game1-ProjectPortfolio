using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    private static DialogManager instance;

    [Header("Dialog")]
    public GameObject DialogPannel;
    public TextMeshProUGUI DialogText;
    [Header("Quest")]
    public GameObject QuestPannel;
    public TextMeshProUGUI QuestName;
    public TextMeshProUGUI QuestDescription;

    public bool inDilaog { get; private set; }

    private Dialog currentDialog;
    private int dialogPointer;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Returns the instance of the dialog manager
    /// </summary>
    /// <returns></returns>
    public static DialogManager Instance() 
    {
        return instance;
    }

    private void Start()
    {
        inDilaog = false;
        DialogPannel.SetActive(false);
    }

    private void Update()
    {
        if (inDilaog)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ContinueDialog();
            }
        }
    }

    /// <summary>
    /// Sets up UI to show dialog
    /// </summary>
    /// <param name="pDialog">The Dialog to show the player</param>
    public void EnterDialog(Dialog pDialog)
    {
        currentDialog = pDialog;
        inDilaog = true;
        DialogPannel.SetActive(true);
        dialogPointer = 0;

        ContinueDialog();
    }

    /// <summary>
    /// Continues the dialog and checks weather to end or not
    /// </summary>
    public void ContinueDialog()
    {
        if (dialogPointer < currentDialog.Speech.Length && inDilaog) DialogText.text = currentDialog.Speech[dialogPointer++];
        else ExitDialog();
    }

    /// <summary>
    /// Exits the dialog and show the quest menu if necesery
    /// </summary>
    private void ExitDialog()
    {
        inDilaog = false;
        DialogPannel.SetActive(false);
        DialogText.text = "";

        if(currentDialog.Quest != null)
        {
            QuestName.text = currentDialog.Quest.QuestName;
            QuestDescription.text = currentDialog.Quest.QuestDescription;
            QuestPannel.SetActive(true);
        }
    }

    /// <summary>
    /// Adds a quest to the player class
    /// </summary>
    public void AcceptQuest()
    {
        gameObject.GetComponent<Player>().AddQuest(currentDialog.Quest);
    }
}
