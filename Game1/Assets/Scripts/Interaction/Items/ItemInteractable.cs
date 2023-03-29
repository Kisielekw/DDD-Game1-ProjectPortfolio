using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable for dropped items.
/// Requires a sprite to draw the item to.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ItemInteractable : Interactable
{
    void Awake()
    {
        // Apply item sprite to the sprite renderer
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = Item.Sprite;
    }

    /// <summary>
    /// Actual item to be picked up.
    /// </summary>
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
