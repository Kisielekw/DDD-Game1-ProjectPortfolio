using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    private NavMeshAgent m_Agent;

    public Transform target;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updateRotation = false;
        m_Agent.updateUpAxis = false;
        m_Agent.updatePosition = false;
        m_Agent.SetDestination(target.position);        
    }

    void Update()
    {
        Vector3 nextPos = m_Agent.nextPosition;
        nextPos.z = 0;
        transform.position = nextPos;
    }
}
