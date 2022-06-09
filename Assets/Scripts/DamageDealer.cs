using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : NetworkBehaviour
{
    
    [SerializeField] int damage = 100;
    
    [Server]
    public int GetDamage()
    {
        return damage;
    }

    [Server]
    public void Hit()
    {
        NetworkServer.Destroy(gameObject);
    }
}
