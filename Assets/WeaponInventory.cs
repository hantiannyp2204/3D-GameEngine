using System.Collections;
using System.Collections.Generic;
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
    public List<BaseWeapon> inventory;

    public Transform hand;

    GameObject spawnedWeaponPrefab;
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
        if(currentEquiped == null)
        {
            currentInventorySlot = BaseWeapon.weaponType.None;
        }
        else
        {
            currentInventorySlot = currentEquiped.inventorySlot;
        }
    }
    void switchWeapon(inventorySlot switchIndex)
    {
        //destroy current equiped weaopn prefab
        if(spawnedWeaponPrefab != null)
        {
            Destroy(spawnedWeaponPrefab);
        }

        //switch current equiped index to secondary slot
        currentEquiped = inventory[(int)switchIndex];
        updateCurrentEquipedIndex();

        //create the gameobject into the game if it dont exist
        if (inventory[(int)switchIndex] != null)
        {
            //change current inv to secondary
            spawnedWeaponPrefab = Instantiate(inventory[(int)currentInventorySlot].weaponPrefab, hand.position, hand.rotation, hand);
            spawnedWeaponPrefab.SetActive(true);

        }
        else
        {
            Debug.Log("no weapon");
        }
    }
    void addWeapon(BaseWeapon newWeapon)
    {

        //set the new weapon into the inventory
        switch (newWeapon.inventorySlot)
        {
            case BaseWeapon.weaponType.Primary:
                //replace if weapon already exist
                if (inventory[0] != null)
                {
                    inventory[0] = null;
                }
                inventory[0] = newWeapon;
                break;
            case BaseWeapon.weaponType.Secondary:
                if (inventory[1] != null)
                {
                    inventory[1] = null;
                }
                inventory[1] = newWeapon;
                break;
        }


    }
}
