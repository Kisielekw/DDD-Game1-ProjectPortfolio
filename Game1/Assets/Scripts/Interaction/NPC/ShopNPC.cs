using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : Interactable
{
    public List<ItemNumber> ItemList;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    { }

    protected override void Interact(Player player)
    {
        DialogManager.Instance().EnterShop(this, player);
    }
}
