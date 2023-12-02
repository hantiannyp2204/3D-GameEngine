using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseScript : MonoBehaviour
{
    [SerializeField]
    WeaponInventory inventory;
    [SerializeField]
    CameraScipt cameraManager;
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    EffectsManager effects;
    [SerializeField]
    ProjectileManager bulletFx;
    [SerializeField]
    RecoilSystem recoilSystem;
    private void Start()
    {
        inventory.OnAddWeapon += addWeapon;
    }
    public void addWeapon(ShootSystem gun)
    {
        gun.shootObserver += cameraManager.Shake;
        gun.shootObserver += audioManager.PlayShootSound;
        gun.shootObserver += effects.PlayParticle;

        gun.adsShootObserver += recoilSystem.RecoilFire;

        gun.bulletRendererObserver += bulletFx.shootProjectile;

        gun.adsObserver += cameraManager.FOVchange;
    }
}
