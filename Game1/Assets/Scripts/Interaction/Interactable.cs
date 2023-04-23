using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract class applicable to all interactables.
/// </summary>
/// <remarks>
/// Capable of listening to 
/// <see cref="Player.InteractEvent">Player interact events</see> 
/// whilst in range.
/// For best functionality all interactable objects should be placed on the Interactable collision layer
/// where collisions can only take place with the <see cref="Player"/>.
/// </remarks>
[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    /// <summary>
    /// Called when other collider enters trigger.
    /// </summary>
    /// <remarks>
    /// If collided gameobject has a <see cref="Player"/> component,
    /// start listening to its <see cref="Player.InteractEvent">interact event</see>.
    /// </remarks>
    /// <param name="collider">Other collider.</param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();

        if (player)
            player.InteractEvent.AddListener(Interact);
    }

    /// <summary>
    /// Called when other collider exits trigger.
    /// </summary>
    /// <remarks>
    /// If collider gameobject has a <see cref="Player"/> component,
    /// stop listening to its <see cref="Player.InteractEvent">interact event</see>.
    /// <param name="collider">Other collider.</param>
    private void OnTriggerExit2D(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();

        if (player)
            player.InteractEvent.RemoveListener(Interact);
    }

    /// <summary>
    /// Abstract function for actual interaction logic.
    /// </summary>
    /// <param name="player">Reference to player calling the interact action.</param>
    protected abstract void Interact(Player player);
}
