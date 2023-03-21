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
    [Header("Shop")]
    public GameObject ShopPannel;

    public bool inDilaog { get; private set; }

    private Dialog currentDialog;
    private int dialogPointer;

    private ShopNPC currentShop;

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
        //if (inDilaog)
        //{
        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        ContinueDialog();
        //    }
        //}
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
        if (!inDilaog) return;
        if (dialogPointer < currentDialog.Speech.Length && inDilaog) DialogText.text = currentDialog.Speech[dialogPointer++];
        else ExitDialog();
    }

    /// <summary>
    /// Exits the dialog and show the quest menu if necesery
    /// </summary>
    private void ExitDialog()
    {
        DialogPannel.SetActive(false);
        DialogText.text = "";

        if(currentDialog.Quest != null)
        {
            QuestName.text = currentDialog.Quest.QuestName;
            QuestDescription.text = currentDialog.Quest.QuestDescription;
            QuestPannel.SetActive(true);
        }
        else inDilaog = false;
    }

    /// <summary>
    /// Method for the quest butons
    /// </summary>
    /// <param name="pAccept">If it was acceped</param>
    public void QuestButtomn(bool pAccept)
    {
        if (pAccept)
        {
            AcceptQuest();
        }
        inDilaog = false;
        QuestPannel.SetActive(false);
    }

    /// <summary>
    /// Adds a quest to the player class
    /// </summary>
    private void AcceptQuest()
    {
        gameObject.GetComponent<Player>().AddQuest(currentDialog.Quest);
    }

    public void EnterShop(ShopNPC pShop)
    {
        inDilaog = true;
        ShopPannel.SetActive(true);
        currentShop = pShop;
    }

    public void ExitShop()
    {
        inDilaog = false;
        ShopPannel.SetActive(false);
    }
}
