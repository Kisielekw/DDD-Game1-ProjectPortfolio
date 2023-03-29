using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract class applicable to all interactables.
/// Capable of listening to interact events whilst the player is in range.
/// For best functionality all interactable objects should be placed on the Interactable collision layer
/// where collisions can only take place with the player.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();

        if (player)
            player.InteractEvent.AddListener(Interact);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();

        if (player)
            player.InteractEvent.RemoveListener(Interact);
    }

    /// <summary>
    /// Abstract function for actual interaction logic
    /// </summary>
    /// <param name="player">Reference to player calling the interact action</param>
    protected abstract void Interact(Player player);
}
