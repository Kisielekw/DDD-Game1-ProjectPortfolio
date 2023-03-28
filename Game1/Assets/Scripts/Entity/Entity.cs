using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract class used to represent all targetable objects
/// At base all entities must have a health value, targetable and invincible flags see <see cref="Entity.EntityStats"/>
/// </summary>
public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// Container for common entity stats
    /// </summary>
    [Serializable]
    public struct EntityStats
    {
        /// <summary>
        /// Maximum HP
        /// </summary>
        public float Max;
        
        /// <summary>
        /// Current HP
        /// </summary>
        public float Current;

        /// <summary>
        /// When true this entity is considered untargetable, ignoring any calls to the <see cref="Entity.Damage"/> function
        /// </summary>
        public bool UnTargetable;

        /// <summary>
        /// Distinct from the <see cref="Untargetable"/> state, entity will still be considered hit, but no damage will be dealt
        /// </summary>
        public bool Invincible;
    }

    /// <summary>
    /// <see cref="EntityStats"/>
    /// </summary>
    [SerializeField]
    protected EntityStats m_Health;

    /// <summary>
    /// Event called when this entity dies
    /// </summary>
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

    /// <summary>
    /// Deal damage to this entity
    /// </summary>
    /// <param name="damage">Damage dealt</param>
    /// <returns>False if entity is in untargetable state</returns>
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
