using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public BaseWeapon testSecondary,testPrimary;

    public BaseWeapon currentEquiped;

    public enum inventorySlot
    { 
        Primary = 0,
        Secondary = 1,
    }
    public BaseWeapon.weaponType currentInventorySlot;
    [SerializeField]
    private List<BaseWeapon> inventory;

    public Transform hand;

    GameObject[] spawnedWeaponPrefab = new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        updateCurrentEquipedIndex();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            addWeapon(testSecondary);
        }
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            addWeapon(testPrimary);
        }
        //switch to priamry
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentInventorySlot != BaseWeapon.weaponType.Primary)
        {
            switchWeapon(inventorySlot.Primary);
        }
        //swithc to secondary
        else if(Input.GetKeyDown(KeyCode.Alpha2) && currentInventorySlot != BaseWeapon.weaponType.Secondary)
        {
            switchWeapon(inventorySlot.Secondary);
        }
    }
    void updateCurrentEquipedIndex()
    {
 

        //prevent game from crashing as scrip is trying to get current equip even tho it don't exist
        if (currentEquiped == null)
        {
            currentInventorySlot = BaseWeapon.weaponType.None;
        }
        else
        {
            currentInventorySlot = currentEquiped.inventorySlot;

        }
        //resets the prefab gun render
        switch (currentInventorySlot)
        {
            case BaseWeapon.weaponType.Primary:
                if (spawnedWeaponPrefab[1] != null)
                {
                    spawnedWeaponPrefab[1].SetActive(false);
                }

                break;
            case BaseWeapon.weaponType.Secondary:
                if (spawnedWeaponPrefab[0] != null)
                {
                    spawnedWeaponPrefab[0].SetActive(false);
                }
                break;
            case BaseWeapon.weaponType.None:
                if (spawnedWeaponPrefab[0] != null)
                {
                    spawnedWeaponPrefab[0].SetActive(false);
                }
                if (spawnedWeaponPrefab[1] != null)
                {
                    spawnedWeaponPrefab[1].SetActive(false);
                }
                break;
        }


    }
    void switchWeapon(inventorySlot switchIndex)
    {

        //switch current equiped index to secondary slot
        currentEquiped = inventory[(int)switchIndex];

        //disable current gaemobject's render
        updateCurrentEquipedIndex();

        if (inventory[(int)switchIndex] != null)
        {
            spawnedWeaponPrefab[(int)switchIndex].SetActive(true);
        }
    }

    public void addWeapon(BaseWeapon newWeapon)
    {

        //set the new weapon into the inventory
        switch (newWeapon.inventorySlot)
        {
            case BaseWeapon.weaponType.Primary:

                inventory[0] = newWeapon;
                //replace if weapon already exist
                if (inventory[0] != null)
                {
                    Destroy(spawnedWeaponPrefab[0]);
                    Debug.Log("Weapon replaced");
                    currentEquiped = newWeapon;
                }
                //render the weapon
                spawnedWeaponPrefab[0] = Instantiate(newWeapon.weaponPrefab, hand.position, hand.rotation, hand);
                Debug.Log("New prefab rendered" + newWeapon.weaponPrefab);
                spawnedWeaponPrefab[0].SetActive(true);


                break;
            case BaseWeapon.weaponType.Secondary:
                inventory[1] = newWeapon;
                //replace if weapon already exist
                if (inventory[1] != null || currentInventorySlot != BaseWeapon.weaponType.None)
                {
                    Destroy(spawnedWeaponPrefab[1]);
                    Debug.Log("Weapon replaced");
                    currentEquiped = newWeapon;
                }
                //render the weapon
                spawnedWeaponPrefab[1] = Instantiate(newWeapon.weaponPrefab, hand.position, hand.rotation, hand);
                Debug.Log("New prefab rendered" + newWeapon.weaponPrefab);
                spawnedWeaponPrefab[1].SetActive(true);
                break;
        }

        //equip weapon if no weapon is in inventory/equiped
        if (currentEquiped == null)
        {
            currentEquiped = newWeapon;
      
        }
        updateCurrentEquipedIndex();

    }
    public BaseWeapon GetItem(int index)
    {
        return inventory[index];
    }
}
