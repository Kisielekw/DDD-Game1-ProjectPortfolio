using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Attack : MonoBehaviour
{
    /// <summary>
    /// UID for the owner gameobject
    /// </summary>
    public int OwnerID { get; private set; }

    [SerializeField]
    private float m_Damage;

    [SerializeField]
    private float m_Speed;

    [SerializeField, Min(0f)]
    private float m_Duration;

    /// <summary>
    /// Timestamp for timeout destruction
    /// Set when entering scene
    /// </summary>
    private float m_End;

    [Serializable, Flags]
    /// <summary>
    /// Conditions under which attack will destroy self
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// No destroy condition due to no flags set
        /// </summary>
        Persistant = 0,
        /// <summary>
        /// Destroys attack after set lifespan
        /// </summary>
        Timeout = (1 << 0),
        /// <summary>
        /// Destroy on hit
        /// </summary>
        OnHit = (1 << 2),
        /// <summary>
        /// Destroy on any collision
        /// </summary>
        OnCollide = (1 << 3),
    }

    [SerializeField]
    private Type m_Type;

    /// <summary>
    /// Hashmap of each entity hit, keyed by UID
    /// </summary>
    private Dictionary<int, Entity> m_ObjectsHit;

    /// <summary>
    /// Per hit event
    /// </summary>
    public UnityEvent<Entity> OnHit;

    /// <summary>
    /// Destruction event, passes array of all entities hit
    /// </summary>
    public UnityEvent<Entity[]> OnEnd;

    public Attack()
    {
        m_ObjectsHit = new();
    }

    void Start()
    {
        m_End = Time.time + m_Duration;
    }

    void Update()
    {
        // Check lifespan
        if ((m_Type & Type.Timeout) != 0 && m_End <= Time.time)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        // Move if object has speed
        if (m_Speed != 0)
            transform.Translate(new Vector3(m_Speed * Time.deltaTime, 0, 0));
    }

    /// <summary>
    /// Called for each intersecting collider during physics update
    /// </summary>
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
    /// Called per collision
    /// </summary>
    /// <param name="collisionObject">collider</param>
    /// <returns>True if collided with targetable entity</returns>
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

    void OnDestroy()
    {
        // Before destruction post event with entities hit
        OnEnd.Invoke(m_ObjectsHit.Values.ToArray());
    }

    /// <summary>
    /// Creates and returns new attack from given template
    /// </summary>
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

[Serializable]
/// <summary>
/// Struct container, describing values to be set
/// Used to export additional variables on an imported prefab
/// </summary>
public struct AttackInfo
{
    /// <summary>
    /// Owner for the attack being spawned
    /// </summary>
    [HideInInspector]
    public GameObject Owner;

    /// <summary>
    /// Prefab describing actual attack to be spawned
    /// </summary>
    public Attack Prefab;

    /// <summary>
    /// On hit event listeners
    /// </summary>
    public UnityEvent<Entity> OnHit;

    /// <summary>
    /// Attack end event listeners
    /// </summary>
    public UnityEvent<Entity[]> OnEnd;    

    /// <summary>
    /// Determines the parent for the spawned attack object
    /// When false the owner will be set as the parent object
    /// When true attack will become a child of the owners parent
    /// </summary>
    public bool UseWorldSpace;
}