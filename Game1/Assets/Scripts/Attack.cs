using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EndAttackCallback();
public class Attack : MonoBehaviour
{
    public float Damage;

    [HideInInspector]
    public float AttackEnd;

    [HideInInspector]
    public GameObject Owner;
    // Start is called before the first frame update    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= AttackEnd)
            Destroy(gameObject);
    }
}
