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
[RequireComponent(typeof(PlayerInput))]
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
    /// Getter for dialog active state.
    /// </summary>
    /// <remarks>
    /// See <see cref="m_TargetPlayer"/>.
    /// </remarks>
    public bool InDialog { 
        get { return DialogPannel.activeInHierarchy; }
    }

    /// <summary>
    /// Current target player.
    /// </summary>
    /// <value>
    /// Whilst null dialog manager is considered as inactive.
    /// </value>
    private Player m_TargetPlayer = null;

    //The dialog that contains all the speach fro the npc the player is curently speaking to
    private Dialog currentDialog;
    private int dialogPointer;

    //The shop the player is interacting with
    private ShopNPC currentShop;

    /// <summary>
    /// Input manager.
    /// </summary>
    /// <remarks>
    /// Dialog manager's own input manager.
    /// Whilst a <see cref="Player"/> is in dialog, 
    /// their input manager is disabled and this enabled,
    /// Preventing player from acting whilst in dialog state.
    /// </remarks>
    private PlayerInput m_Input;

    private void Awake()
    {
        instance = this;

        // Get own input component and disable
        m_Input = GetComponent<PlayerInput>();
        m_Input.enabled = false;
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
        DialogPannel.SetActive(false);
        QuestPannel.SetActive(false);
        ShopPannel.SetActive(false);
    }

    /// <summary>
    /// Sets up UI to show dialog
    /// </summary>
    /// <param name="pDialog">The Dialog to show the player</param>
    /// <param name="player">Player entering dialog</param>
    public void EnterDialog(Dialog pDialog, Player player)
    {
        currentDialog = pDialog;

        SetEnabled(player);
        
        DialogPannel.SetActive(true);
        dialogPointer = 0;

        OnInteract();
    }

    /// <summary>
    /// Interact input callback.
    /// </summary>
    /// <remarks>
    /// Advances along the dialog tree if possible.
    /// </remarks>
    public void OnInteract()
    {
        if (!InDialog)
            return;

        else if (dialogPointer < currentDialog.Speech.Length && InDialog)
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

        // Check if dialog quest exists before attempting to access data
        if(currentDialog.HasQuest)
        {
            IEnumerable<Quest> playerQuestMatch = m_TargetPlayer.Quests.Where(q => q.ID == currentDialog.Quest.ID);

            if(!currentDialog.Quest.isStarted)
            {
                QuestName.text = currentDialog.Quest.QuestName;
                QuestDescription.text = currentDialog.Quest.QuestDescription;
                QuestPannel.SetActive(true);
            }

            else if (playerQuestMatch.Count() > 0)
            {
                if (playerQuestMatch.First<Quest>().isCompleat)
                {
                    EnterDialog(new Dialog(new string[] { string.Format("I see you compleated the Quest {0} congratulations", currentDialog.Quest.QuestName) }), m_TargetPlayer);
                    m_TargetPlayer.RemoveQuest(playerQuestMatch.First<Quest>());
                }
            }
        }

        else
            SetDisabled();
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
        
        SetDisabled();
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
    /// Set up UI to show shop.
    /// </summary>
    /// <param name="pShop">Shop being entered.</param>
    /// <param name="player">Player entering the shop.</param>
    public void EnterShop(ShopNPC pShop, Player player)
    {
        SetEnabled(player);

        ShopPannel.SetActive(true);
        currentShop = pShop;

        List<ItemNumber> playerInv = player.m_Inventory;
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
        SetDisabled();
        ShopPannel.SetActive(false);
    }

    /// <summary>
    /// Make dialog manager active.
    /// </summary>
    /// <remarks>
    /// Disables the <see cref="m_Player">target player</see> input component,
    /// and enambles <see cref="m_Input"/>.
    /// </remarks>
    /// <param name="player">target player</param>
    private void SetEnabled(Player player)
    {
        m_TargetPlayer = player;
        m_TargetPlayer.gameObject.GetComponent<PlayerInput>().enabled = false;
        m_Input.enabled = true;
    }

    /// <summary>
    /// Return to inactive state.
    /// </summary>
    /// <remarks>
    /// Re-enables the <see cref="m_Player">target player</see> input component,
    /// and disables <see cref="m_Input"/>.
    /// </remarks>
    private void SetDisabled()
    {
        m_Input.enabled = false;
        m_TargetPlayer.gameObject.GetComponent<PlayerInput>().enabled = true;
        m_TargetPlayer = null;
    }
}
