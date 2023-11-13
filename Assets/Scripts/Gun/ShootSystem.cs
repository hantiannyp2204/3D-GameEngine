using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    private CameraScipt cameraScript;

    private Camera mainCam;

    private BaseWeapon currentWeapon;

    private AmmoCounter ammoCounter;

    private bool readyToShoot, isShooting, isAiming;
    
    public bool isReloading;

    private GameObject crosshair;
    public Vector3 aimpoint;
    public Quaternion aimRotation;
    //makes sure bullets only -1 if it is shotgun
    private bool shellEjected;
    public GameObject bulletHole;

    public float adsSpeed;
    private void Start()
    {
        crosshair = GameObject.Find("Crosshair");
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
        HandleADS();
        currentWeapon = GetComponentInParent<WeaponInventory>().currentEquiped;
        
    }
    private void HandleADS()
    {
                    
        isAiming = Input.GetMouseButton(1);
        if(isAiming)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimpoint, adsSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, adsSpeed * Time.deltaTime);
            cameraScript.FOVchange(30, adsSpeed);
            crosshair.SetActive(false);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.zero, adsSpeed * Time.deltaTime);
            cameraScript.FOVchange(60,adsSpeed);
            crosshair.SetActive(true);
        }

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
        for (int i = 0; i <= currentWeapon.palletAmount; i++)
        {
            //Spread
            float x = Random.Range(-currentWeapon.spread, currentWeapon.spread);
            float y = Random.Range(-currentWeapon.spread, currentWeapon.spread);
            RaycastHit hit;
            //uinsg main camera's own position instead of the world's

            //direction = forward of where my cam is + Random(x axis of my camera) + Random(y axis of my camera)
            Vector3 direction = mainCam.transform.forward + mainCam.transform.right * x + mainCam.transform.up*y;
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

            //Quaternion.LookRotation(hit.normal) means where ever i am looking at's rotation
            //hit normal makes it face upwards no matter the angle
            Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
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
