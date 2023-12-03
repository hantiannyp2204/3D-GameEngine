using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSystem : UnityEngine.MonoBehaviour
{
    public BaseWeapon weaponScriptableObject;

    Rigidbody rb;
    BoxCollider gunCollider;
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

        gunContainer = GameObject.Find("GunContainer");

        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        gunCollider.enabled = false;
        shootSystem.enabled= true;
    }


    public void DropWeapon()
    {
        shootSystem.enabled = false;

        Transform playerCam = GetComponentInParent<PlayerCam>().transform;
        playerInv.DropWeapon();

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;

   
        rb.AddForce(playerCam.transform.forward * 3, ForceMode.Impulse);
        rb.AddForce(playerCam.transform.up * 3, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 10);

        gunCollider.enabled = true;
  

    }
    public void PickUp()
    {
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        gunCollider.enabled = false;
        shootSystem.enabled = true;

        transform.SetParent(gunContainer.transform);
        playerInv.PickUpWeapon(weaponScriptableObject, shootSystem);

        transform.localPosition= Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }    
}
