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

        //switch current equiped index to secondary slot
        currentEquiped = inventory[(int)switchIndex];
        updateCurrentEquipedIndex();

        //create the gameobject into the game if it dont exist
        renderWeapon((int)switchIndex);

    }
    void renderWeapon(int weaponIndex)
    {
        //destroy current equiped weaopn prefab
        Debug.Log("Weapon prefab destroyed");
        Destroy(spawnedWeaponPrefab);

        if (inventory[(int)weaponIndex] != null)
        {
            //change current inv to secondary
            spawnedWeaponPrefab = Instantiate(inventory[(int)currentInventorySlot].weaponPrefab, hand.position, hand.rotation, hand);
            Debug.Log("New prefab rendered" + inventory[(int)currentInventorySlot].weaponPrefab);
            spawnedWeaponPrefab.SetActive(true);

        }
        else
        {
            Debug.Log("no weapon");
        }
    }
    public void addWeapon(BaseWeapon newWeapon)
    {

        //set the new weapon into the inventory
        switch (newWeapon.inventorySlot)
        {
            case BaseWeapon.weaponType.Primary:
                //replace if weapon already exist
                inventory[0] = newWeapon;
                if (currentInventorySlot == newWeapon.inventorySlot)
                {
                    Debug.Log("Weapon replaced");
                    currentEquiped = newWeapon;
                    renderWeapon((int)newWeapon.inventorySlot);
               
                }
                
                
                break;
            case BaseWeapon.weaponType.Secondary:
                inventory[1] = newWeapon;
                if (currentInventorySlot == newWeapon.inventorySlot)
                {
                    currentEquiped = newWeapon;
                    renderWeapon((int)newWeapon.inventorySlot);
                }

                break;
        }
        updateCurrentEquipedIndex();
    }
    public BaseWeapon GetItem(int index)
    {
        return inventory[index];
    }
}
