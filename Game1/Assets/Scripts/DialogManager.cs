using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Manager for all things NPC including dialog, quests and shops. Controles what is diplayed when player triggers dialog or a shop.
/// </summary>
public class DialogManager : MonoBehaviour
{
    private static DialogManager instance;

    [Header("Dialog")]
    //The UI used for Dialog
    public GameObject DialogPannel;
    public TextMeshProUGUI DialogText;
    [Header("Quest")]
    //The UI used for Quests
    public GameObject QuestPannel;
    public TextMeshProUGUI QuestName;
    public TextMeshProUGUI QuestDescription;
    [Header("Shop")]
    //The UI used for Shops
    public GameObject ShopPannel;
    //The Players inventory
    public GameObject[] PlayerSlots;
    //The Shops inventory
    public GameObject[] ShopSlots;

    /// <summary>
    /// Checks if the player is ocupided by an NPC
    /// </summary>
    public bool inDilaog { get; private set; }

    //The dialog that contains all the speach fro the npc the player is curently speaking to
    private Dialog currentDialog;
    private int dialogPointer;

    //The shop the player is interacting with
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

    /// <summary>
    /// When the program is run at the start of the game it sets the inDialog feald to false and hides all the NPC interaction UI
    /// </summary>
    private void Start()
    {
        inDilaog = false;
        DialogPannel.SetActive(false);
        QuestPannel.SetActive(false);
        ShopPannel.SetActive(false);
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

        IEnumerable<Quest> playerQuestMastch = GetComponent<Player>().Quests.Where(q => q.ID == currentDialog.Quest.ID);

        if (currentDialog.Quest != null && !currentDialog.Quest.isStarted)
        {
            QuestName.text = currentDialog.Quest.QuestName;
            QuestDescription.text = currentDialog.Quest.QuestDescription;
            QuestPannel.SetActive(true);
        }
        if(currentDialog.Quest != null &&
            playerQuestMastch.Count() == 1 &&
            playerQuestMastch.First<Quest>().isCompleat)
        {
            EnterDialog(new Dialog(new string[] { string.Format("I see you compleated the Quest {0} congratulations", currentDialog.Quest.QuestName) }));
            GetComponent<Player>().RemoveQuest(playerQuestMastch.First<Quest>());
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

    /// <summary>
    /// Activates the shop UI and shows the inventory of the player and the shop on screen
    /// </summary>
    /// <param name="pShop"> The Shop the player is interacting with</param>
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


    /// <summary>
    /// Closes the Shop UI and sets inDialog to false
    /// </summary>
    public void ExitShop()
    {
        inDilaog = false;
        ShopPannel.SetActive(false);
    }
}
