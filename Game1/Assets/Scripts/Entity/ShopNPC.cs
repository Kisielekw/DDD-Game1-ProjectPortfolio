using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Shop component of a ShopNPC
/// </summary>
public class ShopNPC : MonoBehaviour
{
    public List<ItemNumber> ItemList;

    private bool isInRange;

    // Start is called before the first frame update
    void Start()
    {
        isInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isInRange && Input.GetKeyDown(KeyCode.F))
        {
            DialogManager.Instance().EnterShop(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInRange = false;
        }
    }
}
