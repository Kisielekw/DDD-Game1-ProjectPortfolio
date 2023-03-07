using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_MaxHealth;

    [SerializeField]
    private Attack m_Melee;
    [SerializeField]
    private float m_AttackSpeed;
    private float m_AttackEnd;


    private Vector2 m_MoveAxis;

    [SerializeField]
    private float m_Speed;
    private bool isDodgeing;
    private float dodgeEnd;

    [SerializeField]
    private List<Quest> m_quests;

    // Start is called before the first frame update
    void Start()
    {
        m_MoveAxis = Vector2.zero;

        isDodgeing = false;

        m_Melee.Owner = gameObject;
        m_AttackEnd = -1;

        m_quests = new List<Quest>();
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

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MoveAxis = context.ReadValue<Vector2>();

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

    public void OnAttack()
    {
        if (m_AttackEnd > Time.time)
            return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 0;
        Vector3 attackDir = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;

        Attack attack = Instantiate(m_Melee, transform.position, 
            Quaternion.LookRotation(new Vector3(0, 0, -1), attackDir), transform);
        m_AttackEnd = Time.time + m_AttackSpeed;
        attack.AttackEnd = m_AttackEnd;
    }

    public void AddQuest(Quest pQuest)
    {
        if(!m_quests.Contains(pQuest)) m_quests.Add(pQuest);
    }
}
