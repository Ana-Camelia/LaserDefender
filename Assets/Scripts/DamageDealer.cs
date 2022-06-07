using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : NetworkBehaviour
{
    [SyncVar]
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
