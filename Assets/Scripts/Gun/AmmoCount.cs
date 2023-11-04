using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    private TMP_Text ammoStatus;
    public ShootSystem player;
    [SerializeField]
    private WeaponInventory weaponInv;

    private BaseWeapon currentWeapon;
    // Start is called before the first frame update
    void Start()
    {
        ammoStatus = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = weaponInv.currentEquiped;
        if (currentWeapon != null)
        {
            updateAmmoCount();
        }
        else
        {
            ammoStatus.SetText(' '.ToString());
        }
    }
    void updateAmmoCount()
    {
        if(player.isReloading == true)
        {
            ammoStatus.SetText("Reloading".ToString());
        }   
        else
        {
            ammoStatus.SetText((currentWeapon.currentAmmo.ToString() + '/' + currentWeapon.maxAmmo.ToString()));
        }
   
    }
}
