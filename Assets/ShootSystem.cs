using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    private CameraScipt cameraScript;

    public BaseWeapon currentWeapon;

    private bool readyToShoot, isShooting,isReloading;
    
    private void Start()
    {
        cameraScript = GetComponentInChildren<CameraScipt>();
        readyToShoot = true;

    }
    // Update is called once per frame
    void Update()
    {
        HandleShooting();
        currentWeapon = GetComponent<WeaponInventory>().currentEquiped;
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
            if (isShooting && readyToShoot)
            {
                Debug.Log("PEW");
                Shoot();
            }
        }
        
        
    }

    private void Shoot()
    {
        readyToShoot = false;
        RaycastHit hit;
        cameraScript.Shake();
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100))
        {
            //Crate c = hit.transform.GetComponent<Crate>();
            //c.OnDamaged(10);
            Debug.Log("hit");
        }
        //currentWeapon.ammoCount--;
        Invoke("gunReadyFire", 1/currentWeapon.firerate);
    }
    private void gunReadyFire()
    {
        readyToShoot = true;
    }
    void Reload()
    {

    }
}
