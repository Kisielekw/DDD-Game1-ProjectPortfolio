using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    [Serializable]
    public struct EntityStats
    {
        public float Max;
        
        public float Current;

        public bool UnTargetable;

        public bool Invincible;
    }

    [SerializeField]
    protected EntityStats m_Health;

    public UnityEvent<Entity>OnDeath;

    // Start is called before the first frame update
    void Start()
    {
        m_Health.Current = m_Health.Max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool Damage(float damage)
    {
        if (m_Health.UnTargetable)
            return false;

        m_Health.Current -= m_Health.Invincible ? 0f : damage;

        if (m_Health.Current <= 0f)
            OnDeath.Invoke(this);

        return true;
    }
}
