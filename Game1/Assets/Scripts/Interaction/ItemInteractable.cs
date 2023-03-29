using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable for dropped items
/// </summary>
public class ItemInteractable : Interactable
{
    public Item Item;

    /// <summary>
    /// Adds the item to the player inventory then destroy self
    /// </summary>
    protected override void Interact(Player player)
    {
        player.AddItem(Item);
        Destroy(gameObject);
    }
}
