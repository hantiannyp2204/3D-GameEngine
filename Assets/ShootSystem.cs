using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    private CameraScipt cameraScript;

    private Camera mainCam;

    private BaseWeapon currentWeapon;

    private AmmoCounter ammoCounter;

    private bool readyToShoot, isShooting;
    
    public bool isReloading;

    //makes sure bullets only -1 if it is shotgun
    private bool shellEjected;
    public GameObject bulletHole;
    private void Start()
    {
        cameraScript = GetComponentInParent<CameraScipt>();
        mainCam = GetComponentInParent<Camera>();
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
            

            if(currentWeapon.fireType == BaseWeapon.shootingStyle.Auto)
            {
                isShooting = Input.GetMouseButton(0);
            }
            else
            {
                isShooting = Input.GetMouseButtonDown(0);
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

        shellEjected = false;
        //will loop for shotguns or bursts
        for (int x = 0; x <= currentWeapon.palletAmount; x++)
        {
            //Spread
            float z = Random.Range(-currentWeapon.spread, currentWeapon.spread);
            float y = Random.Range(-currentWeapon.spread, currentWeapon.spread);
            RaycastHit hit;
            Vector3 direction = mainCam.transform.forward + new Vector3(0, y, z);
            Debug.Log(direction);
           

            if (Physics.Raycast(mainCam.transform.position, direction, out hit, 100))
            {
                //Crate c = hit.transform.GetComponent<Crate>();
                //c.OnDamaged(10);
                Debug.Log("hit");
            }


            if(shellEjected == false)
            {
                ammoCounter.currentAmmo--;
            }
            //if it is a shotgun, this will be true for the next loop iteration
            if (currentWeapon.fireType == BaseWeapon.shootingStyle.Spread)
            {
                shellEjected = true;
            }


            Instantiate(bulletHole, hit.point, Quaternion.Euler(0, 90, 0));
        }
        


        Invoke("gunReadyFire", (float)(60/currentWeapon.firerate));




        //spawn bullet holes
        cameraScript.Shake(currentWeapon.weaponKick);


    }
    private void gunReadyFire()
    {
        readyToShoot = true;
    }
    void Reload()
    {
        isReloading = false;
        ammoCounter.currentAmmo = currentWeapon.maxAmmo;
        ammoCounter.currentPallet = currentWeapon.palletAmount;
    }
}
