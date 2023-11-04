using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu]
public class BaseWeapon : ScriptableObject
{
    public GameObject weaponPrefab;
    public int damage;
    public int firerate;
    public enum shootingStyle
    {
        Single,
        Auto,
        Burst

    }
    public shootingStyle fireType;
    public enum weaponType
    { 
        Primary,
        Secondary,
        None
    }
    public weaponType inventorySlot;
    public int reloadTime;
}
