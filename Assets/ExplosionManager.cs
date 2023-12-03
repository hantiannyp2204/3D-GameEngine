using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool
{
    public class ExplosionManager : UnityEngine.MonoBehaviour
    {
        [Tooltip("Prefab")]
        [SerializeField] private ExplosionVFX explosionPrefab;

        // stack-based ObjectPool available with Unity 2021 and above
        private IObjectPool<ExplosionVFX> objectPool;

        // throw an exception if we try to return an existing item, already in the pool
        [SerializeField] private bool collectionCheck = true;

        // extra options to control the pool capacity and maximum size
        [SerializeField] private int defaultCapacity;
        [SerializeField] private int maxSize;

        private float nextTimeToShoot;

        private void Awake()
        {
            objectPool = new ObjectPool<ExplosionVFX>(CreateProjectile,
                OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                collectionCheck, defaultCapacity, maxSize);
        }

        // invoked when creating an item to populate the object pool
        private ExplosionVFX CreateProjectile()
        {
            ExplosionVFX projectileInstance = Instantiate(explosionPrefab);
            projectileInstance.transform.parent = this.transform;
            projectileInstance.ObjectPool = objectPool;
            return projectileInstance;
        }

        // invoked when returning an item to the object pool
        private void OnReleaseToPool(ExplosionVFX pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        // invoked when retrieving the next item from the object pool
        private void OnGetFromPool(ExplosionVFX pooledObject)
        {
            pooledObject.gameObject.SetActive(true);

        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject(ExplosionVFX pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        public void Render(Vector3 position)
        {
            // get a pooled object instead of instantiating
            ExplosionVFX fireVFXObject = objectPool.Get();

            if (fireVFXObject == null)
                return;

            fireVFXObject.transform.position = position;

            // turn off after a few seconds
            fireVFXObject.Deactivate();

        }
    }
}
