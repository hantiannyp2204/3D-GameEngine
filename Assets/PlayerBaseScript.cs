using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseScript : UnityEngine.MonoBehaviour
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

    [SerializeField]
    PlayerMovement playerMovement;

    [SerializeField]
    RocketManager rocketManager;

    [SerializeField]
    MissleManager missleManager;

    private void Awake()
    {
        playerMovement.runningObserver += cameraManager.FOVSprinting;
        inventory.OnAddWeapon += addWeapon;

    }
    public void addWeapon(ShootSystem gun)
    {
        gun.shootObserver += cameraManager.Shake;
        gun.shootObserver += audioManager.PlayShootSound;
        gun.shootObserver += effects.PlayParticle;

        gun.adsShootObserver += recoilSystem.RecoilFire;

        gun.bulletRendererObserver += bulletFx.shootProjectileUsingWeapon;

        gun.rocketRendererObserver += rocketManager.Render;

        gun.missleRendererObserver += missleManager.Render; 

        gun.adsObserver += cameraManager.FOVchange;
    }
}
