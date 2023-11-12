using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    private CameraScipt cameraScript;

    private BaseWeapon currentWeapon;

    private AmmoCounter ammoCounter;

    private bool readyToShoot, isShooting;
    
    public bool isReloading;
    private void Start()
    {
        cameraScript = GetComponentInParent<CameraScipt>();
        ammoCounter = GetComponent<AmmoCounter>();
        readyToShoot = true;
        isReloading = false;
    }
    // Update is called once per frame
    void Update()
    {
        HandleShooting();
        currentWeapon = GetComponentInParent<WeaponInventory>().currentEquiped;
    }
    private void HandleShooting()
    {
        if(currentWeapon !=null)
        {
            
            if(currentWeapon.fireType == BaseWeapon.shootingStyle.Single)
            {
                isShooting = Input.GetButtonDown("Fire1");
            }
            else if(currentWeapon.fireType == BaseWeapon.shootingStyle.Auto)
            {
                isShooting = Input.GetButton("Fire1");
            }
            if (isShooting && readyToShoot && ammoCounter.currentAmmo > 0 && !isReloading)
            {
                Debug.Log("PEW");
                Shoot();
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            Invoke("Reload", currentWeapon.reloadTime);
        }
        
    }

    private void Shoot()
    {
        readyToShoot = false;

        RaycastHit hit;
        cameraScript.Shake(currentWeapon.weaponKick);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100))
        {
            //Crate c = hit.transform.GetComponent<Crate>();
            //c.OnDamaged(10);
            Debug.Log("hit");
        }
        ammoCounter.currentAmmo--;



        Invoke("gunReadyFire", (float)(60/currentWeapon.firerate));
    }
    private void gunReadyFire()
    {
        readyToShoot = true;
    }
    void Reload()
    {
        isReloading = false;
        ammoCounter.currentAmmo = currentWeapon.maxAmmo;
    }
}
