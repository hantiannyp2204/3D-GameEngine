using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseScript : MonoBehaviour
{

    [SerializeField]
    WeaponInventory inventory;
    [SerializeField]
    CameraScipt cameraShake;
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    EffectsManager effects;
    [SerializeField]
    ProjectileManager bulletFx;
    private void Start()
    {
        inventory.OnAddWeapon += addWeapon;
    }
    public void addWeapon(ShootSystem gun)
    {
        gun.shootObserver = cameraShake.Shake;
        //gun.shootObserver += cameraShake.FOVchange;
        gun.shootObserver += audioManager.PlayShootSound;
        gun.shootObserver += effects.PlayParticle;
        gun.shootObserver += bulletFx.shootProjectile;

        gun.adsObserver += cameraShake.FOVchange;
    }
}
