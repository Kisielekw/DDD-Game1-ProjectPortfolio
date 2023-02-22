using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_MaxHealth;

    private float speed = 0.15f;
    private bool isDodgeing;
    private float dodgeEnd;

    // Start is called before the first frame update
    void Start()
    {
        isDodgeing = false;
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
}
