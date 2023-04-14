using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Component of ItemObjects
/// </summary>
[System.Serializable]
public class ItemObject : MonoBehaviour
{
    /// <summary>
    /// Chacks is the player is in range to pick it up
    /// </summary>
    private bool inPlayerRange;

    /// <summary>
    /// The item that the GameObject represents
    /// </summary>
    public Item Item;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Item.Sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inPlayerRange)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().AddItem(Item);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inPlayerRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inPlayerRange = false;
        }
    }
}
