using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{

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

    float adsSpeed;
    public System.Action<BaseWeapon,Transform> shootObserver;
    public System.Action<BaseWeapon, Transform> bulletRendererObserver;
    public System.Action<BaseWeapon, bool> adsObserver;

    private LayerMask layerMaskIgnore;
    
    ProjectileManager projectileManager;

    public Transform muzleFlashTransform;

    private void Start()
    {
        crosshair = GameObject.Find("Crosshair");
        mainCam = GetComponentInParent<Camera>();
        ammoCounter = GetComponentInChildren<AmmoCounter>();
        readyToShoot = true;
        isReloading = false;
        layerMaskIgnore.value = 255;
        projectileManager = GetComponent<ProjectileManager>();
    }
    // Update is called once per frame
    void Update()
    {

        HandleShooting();
        HandleADS();
        currentWeapon = GetComponentInParent<WeaponInventory>().currentEquiped;
        adsSpeed = 10/currentWeapon.AdsSpeed * Time.deltaTime;
    }
    private void HandleADS()
    {
        isAiming = Input.GetMouseButton(1);
        if(isAiming)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimpoint, adsSpeed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, adsSpeed);
            //cameraScript.FOVchange(30, adsSpeed);
            adsObserver.Invoke(currentWeapon,true);
            crosshair.SetActive(false);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.zero, adsSpeed);
            //cameraScript.FOVchange(60,adsSpeed);
            adsObserver.Invoke(currentWeapon, false);
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


            if (Physics.Raycast(mainCam.transform.position, direction, out hit, 100, layerMaskIgnore))
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
            bulletRendererObserver.Invoke(currentWeapon, muzleFlashTransform);
            //Quaternion.LookRotation(hit.normal) means where ever i am looking at's rotation
            //hit normal makes it face upwards no matter the angle
            Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
        }
        shootObserver.Invoke(currentWeapon,muzleFlashTransform);
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
        ammoCounter.currentPallet = currentWeapon.palletAmount;
    }

}
