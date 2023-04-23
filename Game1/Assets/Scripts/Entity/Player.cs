using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;
using UnityEngine.Events;

[Serializable]
public struct ItemNumber
{
    public Item item;
    public int Number;
}

/// <summary>
/// Entity describing a Player, mostly handles input such as movement and attacks
/// but also stores any required data such as held items.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class Player : Entity
{
    /// <summary>
    /// Current direction of movement.
    /// </summary>
    /// <remarks>
    /// Set by <see cref="OnMove"/>.
    /// </remarks>
    private Vector2 m_MoveAxis;

    /// <summary>
    /// Movement speed.
    /// </summary>
    [SerializeField]
    private float m_Speed;

    private bool isDodgeing;
    private float dodgeEnd;

    [SerializeField]
    private List<Quest> m_quests;
    public List<Quest> Quests { get { return m_quests; } }

    public List<ItemNumber> m_Inventory;

    /// <summary>
    /// Player interaction event.
    /// </summary>
    /// <value>
    /// Passes self to callback for identification.
    /// </value>
    public UnityEvent<Player> InteractEvent;

    /// <summary>
    /// Struct container for all player attacks.
    /// </summary>
    /// <remarks>
    /// Mainly for sorting in unity editor,
    /// contains various <see cref="AttackInfo"/> templates.
    /// </remarks>
    [Serializable]
    public struct Attacks
    {
        /// <summary>
        /// Basic melee attack.
        /// </summary>
        public AttackInfo Melee;
    }

    /// <summary>
    /// See <see cref="Attacks"/>.
    /// </summary>
    [SerializeField]
    private Attacks m_Attacks;

    /// <summary>
    /// Can the player take new actions?
    /// Boolean describing whether player can take new actions
    /// Used for locking input whilst in animation i.e. whilst dodgeing
    /// </summary>
    /// <value>
    /// <list type="table">
    ///     <item>
    ///         <term>True</term>
    ///         <description>
    ///             Can act as normal.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>False</term>
    ///         <description>
    ///             Cannot take new actions,
    ///             likely already taking an action such as dodging.
    ///         </description>
    ///     </item>
    /// </list>
    /// </value>
    private bool m_CanAct = true;

    /// <summary>
    /// Called on creation.
    /// </summary>
    /// <remarks>
    /// Setup default values. Check for interaction with inheritance.
    /// </remarks>
    void Start()
    {
        m_MoveAxis = Vector2.zero;

        isDodgeing = false;

        m_quests = new List<Quest>();

        m_Attacks.Melee.Owner = gameObject;

        m_Inventory = new List<ItemNumber>(10);
    }

    /// <summary>
    /// Per frame update.
    /// </summary>
    /// <remarks>
    /// If a dodge ends, reverts to normal behaviour.
    /// </remarks>
    void Update()
    {
        if (isDodgeing && Time.time >= dodgeEnd)
        {
            isDodgeing = false;
            m_Speed /= 2;
        }
    }

    /// <summary>
    /// Physics synced update.
    /// </summary>
    /// <remarks>
    /// Apply <see cref="m_MoveAxis"/> to player position using
    /// <see cref="m_Speed"/>.
    /// </remarks>
    private void FixedUpdate()
    {
        if (m_MoveAxis != Vector2.zero && !DialogManager.Instance().InDialog)
            transform.position += new Vector3(m_MoveAxis.x, m_MoveAxis.y, 0) * m_Speed * Time.deltaTime;
    }

    /// <summary>
    /// Capture current movement axis using Unity's Input Event System.
    /// </summary>
    /// <param name="val">Movement axis.</param>
    public void OnMove(InputValue val)
    {
        m_MoveAxis = val.Get<Vector2>();

        if (m_MoveAxis.sqrMagnitude < 0.01f)
            m_MoveAxis = Vector2.zero;
    }

    /// <summary>
    /// Method that contains player dodgeing
    /// </summary>
    public void OnDodge()
    {
        if (isDodgeing)
            return;

        isDodgeing = true;
        m_Speed *= 2;
        dodgeEnd = Time.time + 0.25f;
        Debug.Log("I dodge");
    }

    /// <summary>
    /// Basic melee attack.
    /// </summary>
    /// <remarks>
    /// Attack is constructed from <see cref="Attacks.Melee"/>,
    /// rotated towards current mouse position, relative to self.
    /// Disables <see cref="m_CanAct"/> until attack ends.
    /// </remarks>
    public void OnAttack()
    {
        if (!m_CanAct)
            return;

        Attack attack = Attack.Create(m_Attacks.Melee);

        // Rotate attack towards mouse position
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 0;
        Vector3 attackDir = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        attackDir.z = 0;
        attack.transform.Rotate(new Vector3(0, 0, 
            Vector3.SignedAngle(new Vector3(1, 0, 0), attackDir, new Vector3(0, 0, 1))));
        
        m_CanAct = false;
    }

    /// <summary>
    /// <see cref="Attacks.Melee.OnEnd"/> callback. 
    /// </summary>
    /// <remarks>
    /// Re-enables <see cref="m_CanAct"/> after <see cref="OnAttack"/>
    /// </remarks.
    public void AttackEnd(Entity[] hitList)
    {
        m_CanAct = true;
    }

    /// <summary>
    /// Adds a quest to the playes list of quests
    /// </summary>
    /// <param name="pQuest">The quest you want to add to the player</param>
    public void AddQuest(Quest pQuest)
    {
        if (!m_quests.Contains(pQuest))
        { 
            m_quests.Add(pQuest);
            m_quests[m_quests.Count - 1].UpdateItemInitial(m_Inventory);
        }
    }


    /// <summary>
    /// Adds item to the players inventory
    /// </summary>
    /// <param name="pItem">The item you want to add to inventory</param>
    public void AddItem(Item pItem)
    {
        for (int i = 0; i < m_Inventory.Count; i++)
        {
            if (m_Inventory[i].item == pItem)
            {
                ItemNumber iN = m_Inventory[i];
                iN.Number++;
                m_Inventory[i] = iN;
                UpdateItemQuest(pItem);
                return;
            }
        }

        if (m_Inventory.Count == 9) return;
        m_Inventory.Add(new ItemNumber() { item = pItem, Number = 1 });
        UpdateItemQuest(pItem);
    }


    /// <summary>
    /// Removes item from inventory
    /// </summary>
    /// <param name="pItem">The item you want to remove</param>
    void RemoveItem(Item pItem)
    {
        for (int i = 0; i < m_Inventory.Count; i++)
        {
            if (m_Inventory[i].item == pItem)
            {
                ItemNumber iN = m_Inventory[i];
                iN.Number--;

                if (m_Inventory[i].Number == 0) m_Inventory.Remove(iN);

                return;
            }
        }
    }


    /// <summary>
    /// Updates Quests of the Fetch veriaty
    /// </summary>
    /// <param name="pItem">The item the playe picked up</param>
    void UpdateItemQuest(Item pItem)
    {
        foreach (Quest quest in m_quests)
        {
            quest.UpdateItem(pItem);
        }
    }

    /// <summary>
    /// Updates the Quests of Kill veriaty
    /// </summary>
    void UpdateKillQuest()
    {
        foreach (Quest quest in m_quests)
        {
            quest.UpdateKill();
        }
    }

    public void RemoveQuest(Quest pQuest)
    {
        if (!m_quests.Contains(pQuest)) return;
        m_quests.Remove(pQuest);
    }

    /// <summary>
    /// Player interaction handling.
    /// </summary>
    /// <remarks>
    /// Passes local interact input event to a public event,
    /// allowing other objects to easily handle interaction events,
    /// such as with <see cref="Interactables"/>.
    /// Can only interact if <see cref="m_CanAct">actable</see>.
    /// </remarks>
    public void OnInteract()
    {
        if (m_CanAct)
            InteractEvent.Invoke(this);
    }
}
