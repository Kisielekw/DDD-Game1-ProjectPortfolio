using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="Interactable"/> for any pick-upable <see cref="Item"/>.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ItemInteractable : Interactable
{
    /// <summary>
    /// Called on script instancing.
    /// </summary>
    /// <remarks>
    /// Set sprite to be same as <see cref="Item.Sprite"/>
    /// </remarks>
    void Awake()
    {
        // Apply item sprite to the sprite renderer
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = mItem.Sprite;
    }

    /// <summary>
    /// Actual item.
    /// </summary>
    public Item mItem;

    /// <summary>
    /// See <see cref="Interactable.Interact(Player)">parent class</see>.
    /// </summary>
    /// <remarks>
    /// Adds the item to the <see cref="Player"/> inventory then destroy self.
    /// </remarks>
    protected override void Interact(Player player)
    {
        player.AddItem(mItem);
        Destroy(gameObject);
    }
}
