using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

[Serializable]
public struct ItemNumber
{
    public Item item;
    public int Number;
}

public class Player : Entity
{
    private Vector2 m_MoveAxis;

    [SerializeField]
    private float m_Speed;
    private bool isDodgeing;
    private float dodgeEnd;

    [SerializeField]
    private List<Quest> m_quests;

    public List<ItemNumber> m_Inventory;

    [Serializable]
    public struct Attacks
    {
        public AttackInfo Melee;
    }

    [SerializeField]
    private Attacks m_Attacks;

    private bool m_CanAct = true;

    // Start is called before the first frame update
    void Start()
    {
        m_MoveAxis = Vector2.zero;

        isDodgeing = false;

        m_quests = new List<Quest>();

        m_Attacks.Melee.Owner = gameObject;

        m_Inventory = new List<ItemNumber>(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDodgeing && Time.time >= dodgeEnd)
        {
            isDodgeing = false;
            m_Speed /= 2;
        }
    }

    private void FixedUpdate()
    {
        if (m_MoveAxis != Vector2.zero && !DialogManager.Instance().inDilaog)
            transform.position += new Vector3(m_MoveAxis.x, m_MoveAxis.y, 0) * m_Speed * Time.deltaTime;
    }

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
    /// Attempt to create melee attack from info template
    /// </summary>
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
    /// Attack end Callback
    /// </summary>
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

    public void AddItem(Item pItem)
    {
        for (int i = 0; i < m_Inventory.Count; i++)
        {
            if (m_Inventory[i].item == pItem)
            {
                ItemNumber iN = m_Inventory[i];
                iN.Number++;
                m_Inventory[i] = iN;
                
                return;
            }
        }

        if (m_Inventory.Count == 9) return;
        m_Inventory.Add(new ItemNumber() { item = pItem, Number = 1 });
    }

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

    void UpdateItemQuest(Item pItem)
    {
        foreach (Quest quest in m_quests)
        {
            quest.UpdateItem(pItem);
        }
    }

    void UpdateKillQuest()
    {
        foreach (Quest quest in m_quests)
        {
            quest.UpdateKill();
        }
    }
}
