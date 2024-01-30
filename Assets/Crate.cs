using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, ITarget, IDestroyable
{
    [SerializeField]
    GameObject brokenCrate;
    int health = 80;
    [SerializeField]
    GameObject bulletHole;
    [SerializeField]
    GameObject healthPack;
    Rigidbody rbCrate;
    Vector3 initialVelocity;

    bool isBroken = false;
    private void Start()
    {
        rbCrate = GetComponent<Rigidbody>();
    }
    void BecomeBroken()
    {
        //Instantiate a health pack
        GameObject brokenSFX = Instantiate(healthPack, transform.position, transform.rotation);

        brokenSFX.transform.SetParent(null);

        // Instantiate the broken crate
        brokenSFX = Instantiate(brokenCrate, transform.position, transform.rotation);


        // Detach the brokenSFX from the parent
        brokenSFX.transform.SetParent(null);

        // Get all Rigidbody components in children
        Rigidbody[] rigidbodies = brokenSFX.GetComponentsInChildren<Rigidbody>();
        int number =0;
        // Apply force and set initial velocities to each Rigidbody
        foreach (Rigidbody rb in rigidbodies)
        {
            number++;

            // Add force to the Rigidbody
            rb.AddForce(initialVelocity);

        }


        // Destroy the original crate
        Destroy(gameObject);
    }
    public void setInitialVelocity(Vector3 velocity)
    {
        // Get the initial velocity of the crate
        initialVelocity = velocity;
    }
    public void OnDamage(int damage)
    {

        health -= damage;
        if (health <= 0)
        {
           if(isBroken == false)
            {
                BecomeBroken();
                isBroken = true;
            }
           
        }
    }

    public GameObject generateBulletHole()
    {
        return bulletHole;
    }

    public Transform getParent()
    {
        return transform;
    }
}
