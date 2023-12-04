using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sniperScope : MonoBehaviour
{
    [SerializeField] 
    private ShootSystem shootSystem;
    [SerializeField]
    private GameObject scopePNG;
    [SerializeField]
    private GameObject sniperRender;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(shootSystem.getAim() == true)
        {
            scopePNG.gameObject.SetActive(true);
            sniperRender.gameObject.SetActive(false);
        }
        else
        {
            scopePNG.gameObject.SetActive(false);
            sniperRender.gameObject.SetActive(true);
        }
    }
}
