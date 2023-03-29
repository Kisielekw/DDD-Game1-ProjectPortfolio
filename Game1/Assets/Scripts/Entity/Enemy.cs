using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Basic enemy capable of pathfinding to a target position.
/// </summary>
public class Enemy : Entity
{
    /// <summary>
    /// Agent used to generate navigation path.
    /// </summary>
    /// <remarks>
    /// Unity's built in pathfinding doesn't work too well with 2D scenes.
    /// Path travelling should be handled by the entity not by the agent.
    /// Utilizing built in transform updates results in unwanted changes to rotation and z position,
    /// rendering the entity invisible to the player.
    /// </remarks>
    private NavMeshAgent m_Agent;

    /// <summary>
    /// Pathfinding goal.
    /// </summary>
    /// <remarks>
    /// Currently incapable of automatically updating pathing target.
    /// </remarks>
    public Transform target;

    /// <summary>
    /// Called on startup.
    /// </summary>
    /// <remarks>
    /// Fetch and disable updates for <see cref="m_Agent"/>,
    /// and set initial target position.
    /// </remarks>
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        m_Agent.updateRotation = false;
        m_Agent.updateUpAxis = false;
        m_Agent.updatePosition = false;

        m_Agent.SetDestination(target.position);        
    }

    /// <summary>
    /// Per frame update.
    /// </summary>
    /// <remarks>
    /// Manually move to next position according to <see cref="m_Agent"/>.
    /// </remarks>
    void Update()
    {
        // Manually traverse path
        Vector3 nextPos = m_Agent.nextPosition;
        nextPos.z = 0;
        transform.position = nextPos;
    }
}
