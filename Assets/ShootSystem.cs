using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    private CameraScipt cameraScript;

    private void Start()
    {
        cameraScript = GetComponentInChildren<CameraScipt>();
    }
    // Update is called once per frame
    void Update()
    {
        HandleShooting();
    }
    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        cameraScript.Shake();
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100))
        {
            Crate c = hit.transform.GetComponent<Crate>();
            c.OnDamaged(10);
            Debug.Log("hit");
        }
    }
}
