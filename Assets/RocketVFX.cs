using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class RocketVFX : UnityEngine.MonoBehaviour
{
    // deactivate after delay
    [SerializeField] private float timeoutDelay = 10f;



    [SerializeField] private GameObject rocketRenderer;
    [SerializeField] private List<ParticleSystem> particleSystems;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private ParticleSystem explosionVFX;


    [SerializeField] private float impactRadius = 5f;
    [SerializeField] private float rocketDamage;

    private IObjectPool<RocketVFX> objectPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<RocketVFX> ObjectPool { set => objectPool = value; }

    bool exploded;
    private bool isExploding = false;
    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }
    private void OnTriggerEnter(Collider collison)
    {
        if (!collison.gameObject.CompareTag("Bullet") && exploded == false)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            if (!isExploding)
            {
                CheckForTargetsInImpactArea();
            }
            explosionVFX.Play();
            explosionSound.Play();
            for(int x =0; x< particleSystems.Count;x++)
            {
                particleSystems[x].Stop();
            }
            exploded = true;
            rocketRenderer.SetActive(false);
            StartCoroutine(RemoveRocketOnCollision());
        }
    }

    // Call this method to check for objects within the impact area during runtime
    public void CheckForTargetsInImpactArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);

        foreach (Collider collider in colliders)
        {
            // Get the closest point on the collider to the center of the impact area
            Vector3 closestPoint = collider.ClosestPoint(transform.position);

            // Calculate the distance between the rocket and the closest point on the collider
            float distance = Vector3.Distance(transform.position, closestPoint);

            float calculatedDamage = CalculateDamage(rocketDamage, distance);
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

                Debug.Log((int)calculatedDamage);


            }
            IDestroyable destroyable = collider.GetComponent<IDestroyable>();
            if (destroyable != null)
            {
                destroyable.OnDamage((int)calculatedDamage);
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

    IEnumerator RemoveRocketOnCollision()
    {
        yield return new WaitForSeconds(4);

        explosionVFX.Stop();
        explosionSound.Stop();

        // release the projectile back to the pool
        objectPool.Release(this);
    }
    IEnumerator DeactivateRoutine(float delay)
    {
        exploded = false;
        explosionVFX.Stop();
        explosionSound.Stop();
        yield return new WaitForSeconds(delay);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        for (int x = 0; x < particleSystems.Count; x++)
        {
            particleSystems[x].Stop();
        }

        rocketRenderer.SetActive(false);
        yield return new WaitForSeconds(4);
        // release the projectile back to the pool
        objectPool.Release(this);
    }
}
