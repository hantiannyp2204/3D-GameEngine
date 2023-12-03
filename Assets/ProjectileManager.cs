using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool
{
    public class ProjectileManager : UnityEngine.MonoBehaviour
    {
        [Tooltip("Prefab to shoot")]
        [SerializeField] private RevisedProjectile projectilePrefab;

        // stack-based ObjectPool available with Unity 2021 and above
        private IObjectPool<RevisedProjectile> objectPool;

        // throw an exception if we try to return an existing item, already in the pool
        [SerializeField] private bool collectionCheck = true;

        // extra options to control the pool capacity and maximum size
        [SerializeField] private int defaultCapacity;
        [SerializeField] private int maxSize;

        private float nextTimeToShoot;

        private void Awake()
        {
            objectPool = new ObjectPool<RevisedProjectile>(CreateProjectile,
                OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                collectionCheck, defaultCapacity, maxSize);
        }

        // invoked when creating an item to populate the object pool
        private RevisedProjectile CreateProjectile()
        {
            RevisedProjectile projectileInstance = Instantiate(projectilePrefab);
            projectileInstance.transform.parent = this.transform;
            projectileInstance.ObjectPool = objectPool;
            return projectileInstance;
        }

        // invoked when returning an item to the object pool
        private void OnReleaseToPool(RevisedProjectile pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        // invoked when retrieving the next item from the object pool
        private void OnGetFromPool(RevisedProjectile pooledObject)
        {
            pooledObject.gameObject.SetActive(true);

        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject(RevisedProjectile pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        public void shootProjectile(BaseWeapon currentWeapon,Transform muzzlePos, Vector3 direction)
        {
            // get a pooled object instead of instantiating
            RevisedProjectile bulletObject = objectPool.Get();

            if (bulletObject == null)
                return;

            // align to gun barrel/muzzle position
            bulletObject.transform.SetPositionAndRotation(muzzlePos.position, Quaternion.LookRotation(-muzzlePos.transform.right));

            // move projectile forward
            bulletObject.GetComponent<Rigidbody>().AddForce(direction * currentWeapon.muzzleVelocity, ForceMode.Acceleration);


            // turn off after a few seconds
            bulletObject.Deactivate();

        }
    }
}
