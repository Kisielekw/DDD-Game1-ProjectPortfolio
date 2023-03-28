using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemObject : MonoBehaviour
{
    private bool inPlayerRange;

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
