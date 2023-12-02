using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public float pickupRange;
    [SerializeField]
    private LayerMask pickupLayer, itemDrop;

    Camera cam;

    private WeaponInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        playerInventory = GetComponent<WeaponInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange, pickupLayer))
            {
                BaseWeapon newItem = hit.transform.GetComponent<Item>().item as BaseWeapon;
                //add weapon
                playerInventory.addWeapon(newItem);
            }
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, itemDrop, itemDrop))
            {
                //set weapon back to parent
                DropSystem weaponPicupSystem = hit.transform.GetComponent<DropSystem>();
                weaponPicupSystem.PickUp();

                //pickup weapon
                BaseWeapon newItem = hit.transform.GetComponent<DropSystem>().weaponScriptableObject as BaseWeapon;

            }
        }
    }
}
