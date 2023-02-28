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


    private float speed = 0.15f;
    private bool isDodgeing;
    private float dodgeEnd;

    // Start is called before the first frame update
    void Start()
    {
        isDodgeing = false;

        m_Melee.Owner = gameObject;
        m_AttackEnd = -1;
    }

    // Update is called once per frame
    void Update()
    {
        Dodge();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    /// <summary>
    /// Method that contains all the movement for the player
    /// </summary>
    private void Movement()
    {
        
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1;
        }

        direction.Normalize();

        transform.position += new Vector3(direction.x * speed, direction.y * speed, direction.z);
    }

    /// <summary>
    /// Method that contains player dodgeing
    /// </summary>
    private void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDodgeing)
        {
            isDodgeing = true;
            speed = 0.3f;
            dodgeEnd = Time.time + 0.25f;
            Debug.Log("I dodge");
        }

        if (Time.time >= dodgeEnd)
        {
            isDodgeing = false;
            speed = 0.15f;
        }
    }

    private void OnAttack()
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
}
