using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to draw a <see cref="BoxCollider2D"/>.
/// </summary>
/// <remarks>
/// Sourced from <a href="https://gamedev.stackexchange.com/questions/197313/show-colliders-in-a-build-game-in-unity">Bo Monroe</a> on Stack Overflow.
/// Used for debugging collisions.
/// </remarks>
[RequireComponent(typeof(BoxCollider2D))]
public class DrawBox2D : MonoBehaviour
{
    /// <summary>
    /// Prefab line.
    /// </summary>
    /// <remarks>
    /// Describes how the outline will look
    /// </remarks>
    [SerializeField]
    private GameObject m_LinePrefab;

    /// <summary>
    /// Actual line renderer component used.
    /// </summary>
    [SerializeField]
    private LineRenderer m_LineRenderer;

    /// <summary>
    /// Collider being drawn.
    /// </summary>
    [SerializeField]
    private BoxCollider2D m_BoxCollider2D;

    /// <summary>
    /// Called on creation.
    /// </summary>
    /// <remarks>
    /// Fetches <see cref="m_LineRenderer"/> and <see cref="m_BoxCollider2D"/> components.
    /// </remarks>
    void Start()
    {
        m_LineRenderer = Instantiate(m_LinePrefab).GetComponent<LineRenderer>();
        m_LineRenderer.transform.SetParent(transform);
        m_LineRenderer.transform.localPosition = Vector3.zero;
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Per frame update.
    /// </summary>
    /// <remarks>
    /// Simply calls <see cref="HiliteBox"/> to update outline.
    /// </remarks>
    void Update()
    {
        HiliteBox();
    }

    /// <summary>
    /// Draws box outline.
    /// </summary>
    /// <remarks>
    /// Changed from <a href="https://gamedev.stackexchange.com/questions/197313/show-colliders-in-a-build-game-in-unity">source</a>
    /// to account for collider offset.
    /// </remarks>
    void HiliteBox()
    {
        Vector3 offset = new Vector3(m_BoxCollider2D.offset.x, m_BoxCollider2D.offset.y, 0);

        Vector3[] positions = new Vector3[4];
        positions[0] = transform.TransformPoint( new Vector3(m_BoxCollider2D.size.x / 2.0f, m_BoxCollider2D.size.y / 2.0f,0) + offset);
        positions[1] = transform.TransformPoint(new Vector3(-m_BoxCollider2D.size.x / 2.0f, m_BoxCollider2D.size.y / 2.0f,0) + offset);
        positions[2] = transform.TransformPoint(new Vector3(-m_BoxCollider2D.size.x / 2.0f, -m_BoxCollider2D.size.y / 2.0f,0) + offset);
        positions[3] = transform.TransformPoint(new Vector3(m_BoxCollider2D.size.x / 2.0f, -m_BoxCollider2D.size.y / 2.0f,0) + offset);
        m_LineRenderer.SetPositions(positions);
    }
}