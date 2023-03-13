using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Serializable]
    public struct HealthStats
    {
        public float Max;

        
        public float Current;

        public bool UnTargetable;

        public bool Invincible;
    }

    [SerializeField]
    protected HealthStats m_Health;

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
            OnDeath();

        return true;
    }

    protected abstract void OnDeath();
}
