using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciptableStupid : MonoBehaviour
{
    public GameObject weaponPrefab;
    public int damage;
    public float firerate;
    public int maxAmmo;

    public float spread;
    public int palletAmount;
    public float timeBetweenShots;
    public float AdsSpeed;
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
    public AudioClip weaponSound;
}
