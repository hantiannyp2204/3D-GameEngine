using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class BaseWeapon : ScriptableObject
{
    public ShootSystem weaponPrefab;
    public int damage;
    public float weaponKnockback;

    public float muzzleVelocity;

    public float firerate;
    public int maxAmmo;
    public float spread;
    public int palletAmount;
    public float timeBetweenShots;
    public float AdsSpeed;
    public int AdsFOV;

    public Vector3 recoilSpread;

    public bool isHoming;
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
    public enum bulletType
    {
        Raycast,
        Rocket,
        HomingMissle
    }
    public bulletType BulletType;
    public weaponType inventorySlot;
    public float reloadTime;
    public float weaponKick;
    public AudioClip weaponSound;
}
