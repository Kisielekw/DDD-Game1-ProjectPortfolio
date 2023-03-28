using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Basic enemy capable of pathfinding to a target position
/// </summary>
public class Enemy : Entity
{
    private NavMeshAgent m_Agent;

    /// <summary>
    /// Pathfinding goal
    /// </summary>
    public Transform target;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        // Unity's built in pathfinding doesn't work well with 2D
        // Path travelling should be handled by the entity not by the navagent
        // Utilizing built in transform updates results in unwanted changes to rotation and z position rendering the entity invisible to the player
        m_Agent.updateRotation = false;
        m_Agent.updateUpAxis = false;
        m_Agent.updatePosition = false;

        m_Agent.SetDestination(target.position);        
    }

    void Update()
    {
        // Manually traverse path
        Vector3 nextPos = m_Agent.nextPosition;
        nextPos.z = 0;
        transform.position = nextPos;
    }
}
