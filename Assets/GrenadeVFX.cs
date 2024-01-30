using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool
{
    // projectile revised to use UnityEngine.Pool in Unity 2021
    public class GrenadeVFX : MonoBehaviour,IDestroyable
    {
        // deactivate after delay
        [SerializeField] private float timeoutDelay = 3f;

        private IObjectPool<GrenadeVFX> objectPool;

        // public property to give the projectile a reference to its ObjectPool
        public IObjectPool<GrenadeVFX> ObjectPool { set => objectPool = value; }

        Rigidbody rb;
        Collider collider;
        bool cooking = true;
        bool exploded = false;
        [SerializeField] ParticleSystem explosionVFX;
        [SerializeField] AudioSource explosionSFX;
        [SerializeField] GameObject grenadeRender;
        public float explosionRadius = 5f;
        public float damage = 80;
        int health = 0;
        private bool isExploding = false;
        private void Awake()
        {
            collider = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            if(cooking == true)
            {
               transform.localPosition= Vector3.zero;
            }
        }
        public void Cook(Transform handPos)
        {
            cooking = true;
            collider.isTrigger= true;

            rb.useGravity= false;
            StartCoroutine(DeactivateRoutine(timeoutDelay));
        }
        public void Throw()
        {
            cooking = false;
            collider.isTrigger = false;

            rb.useGravity = true;
        }
        IEnumerator DeactivateRoutine(float delay)
        {
            health = 0;
            isExploding = false;
            exploded = false;
            grenadeRender.SetActive(true);
            explosionVFX.Stop();
            explosionSFX.Stop();
            rb.velocity= Vector3.zero;
            yield return new WaitForSeconds(delay);

            //explode
            if(exploded == false)
            {
                OnExplode();
            }
            
        }
        void OnExplode()
        {
            if (isExploding)
            {
                return;
            }

            isExploding = true;

            exploded = true;
            CheckForTargetsInImpactArea();
            explosionVFX.Play();
            explosionSFX.Play();
            grenadeRender.SetActive(false);
            collider.isTrigger = true;

            // release the projectile back to the pool
            Invoke("releasePool", 4);
        }

        void releasePool()
        {
            objectPool.Release(this);
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

        public void OnDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                OnExplode();
            }
        }
    }
}
