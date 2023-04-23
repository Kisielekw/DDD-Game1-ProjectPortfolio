using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Item class contains all infformation about an item
/// </summary>
[System.Serializable]
public class Item
{
    /// <summary>
    /// The Name of the item
    /// </summary>
    [SerializeField]
    public string Name;
    /// <summary>
    /// The Description of the item
    /// </summary>
    public string Description;
    /// <summary>
    /// The Sprite of the item
    /// </summary>
    public Sprite Sprite;
}
