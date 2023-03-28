using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

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
    public GameObject[] PlayerSlots;
    public GameObject[] ShopSlots;

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

        OnDialogContinue();
    }

    /// <summary>
    /// Continues the dialog and checks weather to end or not
    /// </summary>
    public void OnDialogContinue()
    {
        if (!inDilaog) 
            return;

        if (dialogPointer < currentDialog.Speech.Length && inDilaog)
            DialogText.text = currentDialog.Speech[dialogPointer++];

        else 
            ExitDialog();
    }

    /// <summary>
    /// Exits the dialog and show the quest menu if necesery
    /// </summary>
    private void ExitDialog()
    {
        DialogPannel.SetActive(false);
        DialogText.text = "";

        if(currentDialog.Quest != null && !currentDialog.Quest.isStarted)
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
    public void QuestButton(bool pAccept)
    {
        if (pAccept)
        {
            AcceptQuest();
            currentDialog.Quest.isStarted = true;
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

        List<ItemNumber> playerInv = GetComponent<Player>().m_Inventory;
        List<ItemNumber> shopInv = currentShop.ItemList;

        for(int i = 0; i < 9; i++)
        {
            if(playerInv.Count > i)
            {
                PlayerSlots[i].GetComponent<Image>().sprite = playerInv[i].item.Sprite;
            }

            if (shopInv.Count > i)
            {
                ShopSlots[i].GetComponent<Image>().Equals(shopInv[i].item);
            }
        }
    }

    public void ExitShop()
    {
        inDilaog = false;
        ShopPannel.SetActive(false);
    }
}
