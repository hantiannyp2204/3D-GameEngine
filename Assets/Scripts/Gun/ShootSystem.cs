using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShootSystem : UnityEngine.MonoBehaviour
{
    private Camera mainCam;

    private BaseWeapon currentWeapon;

    private AmmoCounter ammoCounter;

    public bool isShooting;
    private bool readyToShoot, isAiming;
    
    public bool isReloading;

    private Image crosshair;
    public Vector3 aimpoint;
    public Quaternion aimRotation;
    //makes sure bullets only -1 if it is shotgun
    private bool shellEjected;
    public GameObject bulletHole;

    float adsSpeed;
    public System.Action<BaseWeapon,Transform> shootObserver;
    public System.Action<BaseWeapon, Transform, Vector3> bulletRendererObserver;
    public System.Action<BaseWeapon, Transform, Vector3> rocketRendererObserver;
    public System.Action<BaseWeapon, Transform, Vector3> missleRendererObserver;
    public System.Action<BaseWeapon, bool> adsObserver;
    public System.Action<BaseWeapon, Transform, bool> adsShootObserver;

    private LayerMask layerMaskIgnore;
    
    ProjectileManager projectileManager;
    public Transform muzleFlashTransform;

    DropSystem dropSystem;

    public bool getAim()
    {
        return isAiming;
    }
    private void Start()
    {
        dropSystem = GetComponent<DropSystem>();
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        mainCam = GetComponentInParent<Camera>();
        ammoCounter = GetComponentInChildren<AmmoCounter>();
        projectileManager = GetComponent<ProjectileManager>();

        readyToShoot = true;
        isReloading = false;
        layerMaskIgnore.value = 255;
    }
    // Update is called once per frame
    void Update()
    {

        HandleButtonPresses();
        HandleADS();
        if (GetComponentInParent<WeaponInventory>() != null)
        {
            currentWeapon = GetComponentInParent<WeaponInventory>().currentEquiped;
            adsSpeed = 10 / currentWeapon.AdsSpeed * Time.deltaTime;
        }
    }
    private void HandleADS()
    {
        isAiming = Input.GetMouseButton(1);
        if(isAiming)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimpoint, adsSpeed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, adsSpeed);
            adsObserver.Invoke(currentWeapon,true);
            crosshair.enabled = false;
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.zero, adsSpeed);
            adsObserver.Invoke(currentWeapon, false);
            crosshair.enabled = true;
        }
    }
    private void HandleButtonPresses()
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            isAiming = false;
           
            dropSystem.DropWeapon();
        }
    }



    private void Shoot()
    {

        readyToShoot = false;

        shellEjected = false;

        int damage=0;
        IDestroyable breakableTarget = null;
        ITarget targetPrefab = null;
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

            //hit target logic
            switch (currentWeapon.BulletType)
            {
                case BaseWeapon.bulletType.Raycast:
                    if (Physics.Raycast(mainCam.transform.position, direction, out hit, 100, layerMaskIgnore))
                    {
                        breakableTarget = hit.transform.GetComponent<IDestroyable>();
                        targetPrefab = hit.transform.GetComponent<ITarget>();
                        if (breakableTarget != null)
                        {
                            damage += currentWeapon.damage;
                            if (targetPrefab != null && targetPrefab.generateBulletHole() != null)
                            {
                                GameObject bullethole = Instantiate(targetPrefab.generateBulletHole(), hit.point, Quaternion.LookRotation(hit.normal));
                                bullethole.transform.SetParent(targetPrefab.getParent());
                            }

                        }
                        else
                        {
                            //Quaternion.LookRotation(hit.normal) means where ever i am looking at's rotation
                            //hit normal makes it face upwards no matter the angle  
                            if (bulletHole != null)
                            {
                                Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                            }

                        }

                        //check rigidbody
                        if (hit.rigidbody != null)
                        {
                            Vector3 dir = hit.point - mainCam.transform.position;
                            hit.rigidbody.AddForce(dir * currentWeapon.weaponKnockback, ForceMode.Impulse);
                        }

                    }
                    bulletRendererObserver.Invoke(currentWeapon, muzleFlashTransform, direction);
                    break;
                case BaseWeapon.bulletType.Rocket:
                    rocketRendererObserver.Invoke(currentWeapon, muzleFlashTransform, mainCam.transform.forward);
                    break;
                case BaseWeapon.bulletType.HomingMissle:
                    missleRendererObserver.Invoke(currentWeapon, muzleFlashTransform, mainCam.transform.forward);
                    break;
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

        }

        //do damage(prevent mutiple instances of onDeath on breakable objects
        if(breakableTarget != null)
        {
            breakableTarget.OnDamage(damage);
        }

        if (isAiming == true)
        {
            adsShootObserver.Invoke(currentWeapon, muzleFlashTransform, true);
        }
        else
        {
            adsShootObserver.Invoke(currentWeapon, muzleFlashTransform, false);
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
