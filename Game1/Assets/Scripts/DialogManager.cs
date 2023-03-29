using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
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

    public bool InDialog { 
        get { return m_TargetPlayer != null; }
    }

    private Player m_TargetPlayer = null;

    private Dialog currentDialog;
    private int dialogPointer;

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

    private void Start()
    {
        DialogPannel.SetActive(false);
    }

    /// <summary>
    /// Sets up UI to show dialog
    /// </summary>
    /// <param name="pDialog">The Dialog to show the player</param>
    /// <param name="player">Player entering dialog</param>
    public void EnterDialog(Dialog pDialog, Player player)
    {
        currentDialog = pDialog;

        m_TargetPlayer = player;
        m_TargetPlayer.gameObject.GetComponent<PlayerInput>().enabled = false;
        m_Input.enabled = true;
        
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

        if (dialogPointer < currentDialog.Speech.Length && InDialog)
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
            if(!currentDialog.Quest.isStarted)
            {
                QuestName.text = currentDialog.Quest.QuestName;
                QuestDescription.text = currentDialog.Quest.QuestDescription;
                QuestPannel.SetActive(true);
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

    public void EnterShop(ShopNPC pShop)
    {
        //InDialog = true;
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
        //InDialog = false;
        ShopPannel.SetActive(false);
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
