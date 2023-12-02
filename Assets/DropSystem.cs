using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    public BaseWeapon weaponScriptableObject;

    Rigidbody rb;
    BoxCollider gunCollider;
    Camera mainCam;
    ShootSystem shootSystem;
    GameObject gunContainer;

    WeaponInventory playerInv;
    // Start is called before the first frame update
    void Start()
    {
        playerInv = GetComponentInParent<WeaponInventory>();
        shootSystem = GetComponent<ShootSystem>();
        gunCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        mainCam = GetComponentInParent<Camera>();
        gunContainer = GameObject.Find("GunContainer");

        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        gunCollider.enabled = false;
        shootSystem.enabled= true;
    }


    public void DropWeapon()
    {
        transform.SetParent(null);
        playerInv.DropWeapon(); 
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        gunCollider.enabled = true;
        shootSystem.enabled = false;

        rb.AddForce(mainCam.transform.forward * 3, ForceMode.Impulse);
        rb.AddForce(mainCam.transform.up * 6, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 10);
    }
    public void PickUp()
    {
        transform.SetParent(gunContainer.transform);
        playerInv.PickUpWeapon(weaponScriptableObject, shootSystem);

        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        gunCollider.enabled = false;
        shootSystem.enabled = true;

        transform.localPosition= Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }    
}
