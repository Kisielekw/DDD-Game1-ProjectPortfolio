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
    /// Container for common entity stats.
    /// </summary>
    [Serializable]
    public struct EntityStats
    {
        /// <summary>
        /// Maximum HP.
        /// </summary>
        public float Max;
        
        /// <summary>
        /// Current HP.
        /// </summary>
        public float Current;

        /// <summary>
        /// Targetable state.
        /// </summary>
        /// <value>
        /// <list type="table">
        ///     <item>
        ///         <term>True</term>
        ///         <description>
        ///             Entity is untargetable and cannot be hit by attacks.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>False</term>
        ///         <description>
        ///             Entity acts as normal.
        ///         </description>
        ///     </item>
        /// </list>
        /// </value>
        public bool UnTargetable;

        /// <summary>
        /// Invincibility state.
        /// </summary>
        /// <value>
        /// <list type="table">
        ///     <item>
        ///         <term>True</term>
        ///         <description>
        ///             Entity is invincible, will still be treated as being <see cref="Attack.Hit(GameObject)">hit</see>,
        ///             but all damage dealt is reduced to zero.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>False</term>
        ///         <description>
        ///             Entity acts as normal.
        ///         </description>
        ///     </item>
        /// </list>
        /// </value>
        public bool Invincible;
    }

    /// <summary>
    /// <see cref="EntityStats"/>
    /// </summary>
    [SerializeField]
    protected EntityStats m_Health;

    /// <summary>
    /// Death event.
    /// </summary>
    /// <value>
    /// Passes self as paramater to determine which entity
    /// died in callback.
    /// </value>
    public UnityEvent<Entity>OnDeath;

    /// <summary>
    /// Called on creation.
    /// </summary>
    /// <remarks>
    /// Sets <see cref="EntityStats.Current">health</see> to <see cref="EntityStats.Max">max</see>.
    /// </remarks>
    void Start()
    {
        m_Health.Current = m_Health.Max;
    }

    /// <summary>
    /// Per frame update
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Deal damage to this entity.
    /// </summary>
    /// <remarks>
    /// Usually called by an <see cref="Attack"/>.
    /// </remarks>
    /// <param name="damage">Damage dealt.</param>
    /// <returns>
    /// <list type="table">
    ///     <item>
    ///         <term>True</term>
    ///         <description>
    ///             Damage successfuly dealt.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>False</term>
    ///         <description>
    ///             Damage was not dealt likely due to
    ///             <see cref="EntityStats.UnTargetable">untargetable</see> state.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
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
