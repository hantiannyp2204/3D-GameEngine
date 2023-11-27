using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseScript : MonoBehaviour
{
    [SerializeField]
    CameraScipt cameraShake;
    [SerializeField]
    WeaponInventory inventory;
    [SerializeField]
    AudioManager audioManager;
    private void Start()
    {
        inventory.OnAddWeapon += addWeapon;
    }
    public void addWeapon(ShootSystem gun)
    {
        gun.shootObserver += cameraShake.Shake;
        //gun.shootObserver += cameraShake.FOVchange;
        gun.shootObserver += audioManager.PlayShootSound;

        gun.adsObserver += cameraShake.FOVchange;
    }
}
