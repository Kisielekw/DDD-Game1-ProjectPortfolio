using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Generic attack.
/// </summary>
/// <remarks>
/// Attacks are defined as prefabs, then created from an <see cref="AttackInfo"/> template,
/// using the <see cref="Attack.Create(AttackInfo)"/> function.
/// </remarks>
[Serializable]
public class Attack : MonoBehaviour
{
    /// <summary>
    /// UID for the owner gameobject
    /// </summary>
    /// <remarks>
    /// Used to avoid attacks colliding with their owner.
    /// </remarks>
    /// <value>
    /// Often a <see cref="GameObject"/> with an <see cref="Entity"/> component.
    /// </value>
    public int OwnerID { get; private set; }

    /// <summary>
    /// Damage dealt.
    /// </summary>
    [SerializeField]
    private float m_Damage;

    /// <summary>
    /// The travelspeed for this attack.
    /// </summary>
    /// <remarks>
    /// Mostly applicable to projectiles.
    /// </remarks>
    [SerializeField]
    private float m_Speed;

    /// <summary>
    /// How long in seconds attack lasts.
    /// </summary>
    /// <remarks>
    /// Used with <see cref="Type.Timeout"/> and <see cref="m_End"/>.
    /// </remarks>
    [SerializeField, Min(0f)]
    private float m_Duration;

    /// <summary>
    /// Timestamp for timeout destruction.
    /// </summary>
    /// <value>
    /// Determined by <see cref="m_Duration"/> and used for <see cref="Type.Timeout"/>.
    /// </value>
    private float m_End;

    /// <summary>
    /// Conditions under which attack will destroy self.
    /// </summary>
    [Serializable, Flags]
    public enum Type
    {
        /// <summary>
        /// No destroy conditions.
        /// </summary>
        Persistant = 0,

        /// <summary>
        /// Destroy after set time.
        /// </summary>
        /// <remarks>
        /// Makes use of <see cref="Attack.m_Duration"/> and <see cref="Attack.m_End"/>.
        /// </remarks>
        Timeout = (1 << 0),

        /// <summary>
        /// Destroy on successful hit with a <see cref="Entity"/>.
        /// </summary>
        OnHit = (1 << 2),

        /// <summary>
        /// Destroy on any collision, regardless of outcome.
        /// </summary>
        OnCollide = (1 << 3),
    }

    /// <summary>
    /// See <see cref="Type"/>.
    /// </summary>
    [SerializeField]
    private Type m_Type;

    /// <summary>
    /// Hashmap of each entity hit.
    /// </summary>
    /// <remarks>
    /// Used for passing to <see cref="OnEnd"/> event,
    /// and avoiding hitting entities multiple times.
    /// </remarks>
    /// <value>
    /// <list type="table">
    ///     <item>
    ///         <term>Key</term>
    ///         <description>
    ///             UID of Entity.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>Value</term>
    ///         <description>
    ///             Reference to hit <see cref="Entity"/>
    ///         </description>
    ///     </item>
    /// </list>
    /// </value>
    private Dictionary<int, Entity> m_ObjectsHit;

    /// <summary>
    /// Per hit event.
    /// </summary>
    /// <remarks>
    /// Called by <see cref="Attack.Hit(GameObject)"/> function.
    /// </remarks>
    /// <value>
    /// Passes reference to hit entity.
    /// </value>

    public UnityEvent<Entity> OnHit;

    /// <summary>
    /// Destruction event.
    /// </summary>
    /// <remarks>
    /// Called by <see cref="OnDestroy"/> function.
    /// </remarks>
    /// <value>
    /// Passes array of all entities hit over attack lifetime.
    /// </value>
    public UnityEvent<Entity[]> OnEnd;

    public Attack()
    {
        m_ObjectsHit = new();
    }

    /// <summary>
    /// Called on creation.
    /// </summary>
    /// <remarks>
    /// Sets the timestamp for <see cref="Type.Timeout"/>.
    /// </remarks>
    void Start()
    {
        m_End = Time.time + m_Duration;
    }

    /// <summary>
    /// Per frame update.
    /// </summary>
    /// <remarks>
    /// Checks for <see cref="Type.Timeout"/> condition.
    /// </remarks>
    void Update()
    {
        // Check lifespan
        if ((m_Type & Type.Timeout) != 0 && m_End <= Time.time)
            Destroy(gameObject);
    }

    /// <summary>
    /// Physics synced update.
    /// </summary>
    /// <remarks>
    /// Moves attack by <see cref="m_Speed"/> along local X axis.
    /// </remarks>
    void FixedUpdate()
    {
        // Move if object has speed
        if (m_Speed != 0)
            transform.Translate(new Vector3(m_Speed * Time.deltaTime, 0, 0));
    }

    /// <summary>
    /// Called for each intersecting collider during physics update,
    /// passing each into the <see cref="Hit"/> function. 
    /// </summary>
    /// <remarks>
    /// Could use OnTrigger enter to avoid multi collisions, but would lead to issue
    /// where an enitity who lost the <see cref="Entity.EntityStats.UnTargetable"/> state whilst in an attack collider,
    /// ignoring further collisions and not taking damage.
    /// </remarks>
    private void OnTriggerStay2D(Collider2D collider)
    {
        // Funky conditional to avoid repeated calls to Destroy self from overlap of OnHit and OnCollide conditions
        // Destroy is only called if attack destroys on hit AND attack hit
        // IF DestroyOnCollide AND (Hit NAND DestroyOnHit): Destroy
        if ((m_Type & Type.OnCollide) != 0 && 
            !(Hit(collider.gameObject) && (m_Type & Type.OnHit) != 0))
            Destroy(gameObject);
    }

    /// <summary>
    /// Called per collision.
    /// </summary>
    /// <remarks>
    /// checks if target object is an entity,
    /// and if a damaging collision took place calling
    /// <see cref="OnHit"/> event if necessary.
    /// </remarks>
    /// <param name="collisionObject">collider</param>
    /// <returns>
    /// <list type="table">
    ///     <listheader>
    ///         <term>State</term>
    ///         <description>Effect</description>
    ///     </listheader>
    ///     <item>
    ///         <term>True</term>
    ///         <description>
    ///             Attack collided with targetable entity.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>False</term>
    ///          <description>
    ///            Collider was  either not an entity or un-targetable.
    ///          </description>
    ///     </item>
    /// </list>
    private bool Hit(GameObject collisionObject)
    {
        // Check object ID to avoid hitting owner or multiple collisions with same object
        int objectID = collisionObject.GetInstanceID();
        if (objectID == OwnerID || m_ObjectsHit.ContainsKey(objectID))
            return false;

        // Attempt to get and verify Entity component of collider
        Entity target = collisionObject.GetComponent<Entity>();
        if (!target)
            return false;
        
        // Attempt to deal damage
        if (!target.Damage(m_Damage))
            return false;

        // Add target to dictionary and trigger event
        m_ObjectsHit.Add(objectID, target);
        OnHit.Invoke(target);

        // Check for destruction condition
        if ((m_Type & Type.OnHit) != 0)
            Destroy(gameObject);

        return true;
    }

    /// <summary>
    /// Called on attack destruction.
    /// </summary>
    /// <remarks>
    /// <see cref="OnEnd"/> called here.
    /// </remarks>
    void OnDestroy()
    {
        // Before destruction post event with entities hit
        OnEnd.Invoke(m_ObjectsHit.Values.ToArray());
    }

    /// <summary>
    /// Psuedo factory, creating attacks from templates.
    /// </summary>
    /// <remarks>
    /// Had difficulty using a true factory due to how prefabs and objects are constructed by Unity.
    /// </remarks>
    /// <param name="info"><see cref="AttackInfo"/> template.</param>
    /// <returns>Created attack object.</returns>
    public static Attack Create(AttackInfo info)
    {
        Transform parent = info.Owner.transform;

        if (info.UseWorldSpace)
            parent = parent.parent;

        Attack attack = UnityEngine.Object.Instantiate(info.Prefab, parent, info.UseWorldSpace);

        attack.OnHit = info.OnHit;
        attack.OnEnd = info.OnEnd;

        attack.OwnerID = info.Owner.GetInstanceID();

        return attack;
    }
}

/// <summary>
/// Struct container, describing an <see cref="Attack"/>.
/// </summary>
/// <remarks>
/// Used to export additional variables on an imported prefab.
/// Stored <see cref="Attack"/> created using <see cref="Attack.Create(AttackInfo)"/> function.
/// </remarks>
[Serializable]
public struct AttackInfo
{
    /// <summary>
    /// See <see cref="Attack.Owner"/>.
    /// </summary>
    [HideInInspector]
    public GameObject Owner;

    /// <summary>
    /// Prefab describing actual attack to be spawned.
    /// </summary>
    /// <remarks>
    /// Prefab can be used to define hitboxes, logic, etc.
    /// </remarks>
    public Attack Prefab;

    /// <summary>
    /// See <see cref="Attack.OnHit"/>.
    /// </summary>
    public UnityEvent<Entity> OnHit;

    /// <summary>
    /// See <see cref="Attack.OnEnd"/>.
    /// </summary>
    public UnityEvent<Entity[]> OnEnd;    

    /// <summary>
    /// Determines the parent for the spawned attack object.
    /// </summary>
    /// <remarks>
    /// Often used for projectiles, which travel independant of the <see cref="AttackInfo.Owner"/>.
    /// </remarks>
    /// <value>
    /// <list type="table">
    ///     <item>
    ///         <term>True</term>
    ///         <description>
    ///             Attack will become a child of the owners parent.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>False</term>
    ///          <description>
    ///             Owner will be set as the parent object.
    ///          </description>
    ///     </item>
    /// </list>
    /// </value>
    public bool UseWorldSpace;
}