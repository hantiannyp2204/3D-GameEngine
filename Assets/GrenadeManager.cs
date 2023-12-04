using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool
{
    public class GrenadeManager : UnityEngine.MonoBehaviour
    {
        [Tooltip("Prefab")]
        [SerializeField] private GrenadeVFX grenadePrefab;

        // stack-based ObjectPool available with Unity 2021 and above
        private IObjectPool<GrenadeVFX> objectPool;

        // throw an exception if we try to return an existing item, already in the pool
        [SerializeField] private bool collectionCheck = true;

        // extra options to control the pool capacity and maximum size
        [SerializeField] private int defaultCapacity;
        [SerializeField] private int maxSize;

        private float nextTimeToShoot;
        GrenadeVFX grenadeVFXObject;
        private void Awake()
        {
            objectPool = new ObjectPool<GrenadeVFX>(CreateProjectile,
                OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                collectionCheck, defaultCapacity, maxSize);
        }

        // invoked when creating an item to populate the object pool
        private GrenadeVFX CreateProjectile()
        {
            GrenadeVFX projectileInstance = Instantiate(grenadePrefab);
            projectileInstance.transform.parent = this.transform;
            projectileInstance.ObjectPool = objectPool;
            return projectileInstance;
        }

        // invoked when returning an item to the object pool
        private void OnReleaseToPool(GrenadeVFX pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        // invoked when retrieving the next item from the object pool
        private void OnGetFromPool(GrenadeVFX pooledObject)
        {
            pooledObject.gameObject.SetActive(true);

        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject(GrenadeVFX pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        public void HoldNade(Transform handPos)
        {
           
            // get a pooled object instead of instantiating
            grenadeVFXObject = objectPool.Get();

            if (grenadeVFXObject == null)
                return;

            // align to gun barrel/muzzle position
            grenadeVFXObject.transform.SetParent(handPos);

         
            // turn off after a few seconds
            grenadeVFXObject.Cook(handPos);

        }
        public void ThrowNade(Vector3 direction)
        {
            grenadeVFXObject.transform.SetParent(null);
            // move projectile forward
            grenadeVFXObject.GetComponent<Rigidbody>().AddForce(direction * 400, ForceMode.Acceleration);
            grenadeVFXObject.Throw();


        }


    }

}
