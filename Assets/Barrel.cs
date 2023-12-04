using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Barrel : MonoBehaviour,ITarget, IDestroyable
{
    int health = 180;
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    ExplosionManager explosionManager;
    [SerializeField]
    FireManager fireManager;
    [SerializeField]
    GameObject bulletHole;
    public float explosionRadius = 2;
    int damage = 60;
    private bool isExploding = false;
    bool exploded= false;
    public GameObject generateBulletHole()
    {
        return bulletHole;
    }

    public Transform getParent()
    {
        return this.transform;
    }

    public void OnDamage(int damage)
    {
        health -= damage;
        if(health<=0)
        {
            BlowUp();
        }
    }

    // Start is called before the first frame update
    void BlowUp()
    {
        if(exploded == false)
        {
            explosionManager.Render(this.transform.position);
            fireManager.Render(this.transform.position);
            exploded = true;
            if (!isExploding)
            {
                CheckForTargetsInImpactArea();
            }

            Destroy(gameObject);
        }
        
    }
    public void CheckForTargetsInImpactArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            // Get the closest point on the collider to the center of the impact area
            Vector3 closestPoint = collider.ClosestPoint(transform.position);

            // Calculate the distance between the rocket and the closest point on the collider
            float distance = Vector3.Distance(transform.position, closestPoint);

            float calculatedDamage = CalculateDamage(damage, distance);
            IDestroyable destroyable = collider.GetComponent<IDestroyable>();
            if (destroyable != null)
            {
                destroyable.OnDamage((int)calculatedDamage);
            }
            ITarget target = collider.GetComponent<ITarget>();
            if (target != null)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 forceDirection = collider.transform.position - transform.position;
                    Debug.Log(forceDirection);
                    forceDirection.Normalize(); // Normalize to get a unit vector
                    rb.AddForce(forceDirection * calculatedDamage * 10);

                    Vector3 pushVelocity = forceDirection * calculatedDamage * 10;
                    //keep velocity if its crate
                    Crate crateBox = collider.GetComponent<Crate>();
                    if (crateBox != null)
                    {
                        crateBox.setInitialVelocity(pushVelocity);
                    }
                }
            }

        }
    }
    private float CalculateDamage(float baseDamage, float distance)
    {
        // Use an inverse-square law for damage falloff
        float falloffFactor = 1f / (distance * distance);
        float calculatedDamage = baseDamage * falloffFactor;

        // Ensure damage is not greater than the base damage
        calculatedDamage = Mathf.Min(calculatedDamage, baseDamage);
        // Ensure damage is not negative
        return Mathf.Max(calculatedDamage, 10);
    }
}
