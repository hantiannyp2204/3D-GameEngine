using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class BaseWeapon : ScriptableObject
{
    public GameObject weaponPrefab;
    public int damage;
    public float firerate;
    public int maxAmmo;

    public float spread;
    public int palletAmount;
    public float timeBetweenShots;
    public enum shootingStyle
    {
        Single,
        Auto,
        Burst,
        Spread

    }
    public shootingStyle fireType;
    public enum weaponType
    { 
        Primary,
        Secondary,
        None
    }
    public weaponType inventorySlot;
    public float reloadTime;
    public float weaponKick;
}
