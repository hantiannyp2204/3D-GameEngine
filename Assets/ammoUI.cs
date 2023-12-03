using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ammoUI : UnityEngine.MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private TMP_Text ammoText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponentInChildren<AmmoCounter>() != null)
        {
            if(!player.GetComponentInChildren<ShootSystem>().isReloading)
            {
                ammoText.SetText((player.GetComponentInChildren<AmmoCounter>().currentAmmo + "/" + player.GetComponent<WeaponInventory>().currentEquiped.maxAmmo).ToString());
            }
            else
            {
                ammoText.SetText("Reloading");
            }
            
        }
        else
        {
            ammoText.SetText(' '.ToString());
        }
    }
}
