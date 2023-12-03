using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool
{
    public class FireManager : UnityEngine.MonoBehaviour
    {
        [Tooltip("Prefab")]
        [SerializeField] private FireVFX firePrefab;

        // stack-based ObjectPool available with Unity 2021 and above
        private IObjectPool<FireVFX> objectPool;

        // throw an exception if we try to return an existing item, already in the pool
        [SerializeField] private bool collectionCheck = true;

        // extra options to control the pool capacity and maximum size
        [SerializeField] private int defaultCapacity;
        [SerializeField] private int maxSize;

        private float nextTimeToShoot;

        private void Awake()
        {
            objectPool = new ObjectPool<FireVFX>(CreateProjectile,
                OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                collectionCheck, defaultCapacity, maxSize);
        }

        // invoked when creating an item to populate the object pool
        private FireVFX CreateProjectile()
        {
            FireVFX projectileInstance = Instantiate(firePrefab);
            projectileInstance.transform.parent = this.transform;
            projectileInstance.ObjectPool = objectPool;
            return projectileInstance;
        }

        // invoked when returning an item to the object pool
        private void OnReleaseToPool(FireVFX pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        // invoked when retrieving the next item from the object pool
        private void OnGetFromPool(FireVFX pooledObject)
        {
            pooledObject.gameObject.SetActive(true);

        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject(FireVFX pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        public void Render(Vector3 position)
        {
            // get a pooled object instead of instantiating
            FireVFX fireVFXObject = objectPool.Get();

            if (fireVFXObject == null)
                return;

            fireVFXObject.transform.position = position;

            // turn off after a few seconds
            fireVFXObject.Deactivate();

        }
    }
}
