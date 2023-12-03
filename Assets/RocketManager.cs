using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool
{
    public class RocketManager : UnityEngine.MonoBehaviour
    {
        [Tooltip("Prefab")]
        [SerializeField] private RocketVFX rocketPrefab;

        // stack-based ObjectPool available with Unity 2021 and above
        private IObjectPool<RocketVFX> objectPool;

        // throw an exception if we try to return an existing item, already in the pool
        [SerializeField] private bool collectionCheck = true;

        // extra options to control the pool capacity and maximum size
        [SerializeField] private int defaultCapacity;
        [SerializeField] private int maxSize;

        private float nextTimeToShoot;

        private void Awake()
        {
            objectPool = new ObjectPool<RocketVFX>(CreateProjectile,
                OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                collectionCheck, defaultCapacity, maxSize);
        }

        // invoked when creating an item to populate the object pool
        private RocketVFX CreateProjectile()
        {
            RocketVFX projectileInstance = Instantiate(rocketPrefab);
            projectileInstance.transform.parent = this.transform;
            projectileInstance.ObjectPool = objectPool;
            return projectileInstance;
        }

        // invoked when returning an item to the object pool
        private void OnReleaseToPool(RocketVFX pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        // invoked when retrieving the next item from the object pool
        private void OnGetFromPool(RocketVFX pooledObject)
        {
            pooledObject.gameObject.SetActive(true);

        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject(RocketVFX pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        public void Render(BaseWeapon currentWeapon,Transform muzzlePos, Vector3 direction)
        {
            // get a pooled object instead of instantiating
            RocketVFX fireVFXObject = objectPool.Get();

            if (fireVFXObject == null)
                return;

            // align to gun barrel/muzzle position
            fireVFXObject.transform.SetPositionAndRotation(muzzlePos.position, Quaternion.LookRotation(-muzzlePos.transform.right));

            // move projectile forward
            fireVFXObject.GetComponent<Rigidbody>().AddForce(direction * currentWeapon.muzzleVelocity, ForceMode.Acceleration);

            // turn off after a few seconds
            fireVFXObject.Deactivate();

        }

    }
}
