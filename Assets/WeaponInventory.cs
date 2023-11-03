using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public BaseWeapon testWeapon;
    //0 is primary, 1 is secondary
    public List<BaseWeapon> inventory;

    public Transform hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            addWeapon(testWeapon);
        }
    }
    void addWeapon(BaseWeapon newWeapon)
    {

        // Spawn the prefab at the specified position and rotation, and set its parent
        GameObject spawnedObject = Instantiate(newWeapon.weaponPrefab, hand.position, hand.rotation, hand);

    }
}
