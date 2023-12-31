using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool
{
    // projectile revised to use UnityEngine.Pool in Unity 2021
    public class RevisedProjectile : UnityEngine.MonoBehaviour
    {
        [SerializeField]
        MeshRenderer meshRenderer;
        // deactivate after delay
        [SerializeField] private float timeoutDelay = 3f;

        private IObjectPool<RevisedProjectile> objectPool;

        // public property to give the projectile a reference to its ObjectPool
        public IObjectPool<RevisedProjectile> ObjectPool { set => objectPool = value; }

        public void Deactivate()
        {
            StartCoroutine(DeactivateRoutine(timeoutDelay));
        }

        IEnumerator DeactivateRoutine(float delay)
        {
            bulletRenderDelay();
            yield return new WaitForSeconds(delay);

            // reset the moving Rigidbody
            Rigidbody rBody = GetComponent<Rigidbody>();
            rBody.velocity = new Vector3(0f, 0f, 0f);
            rBody.angularVelocity = new Vector3(0f, 0f, 0f);

            // release the projectile back to the pool
            UnrenderBullet();
            objectPool.Release(this);
        }
        private void OnTriggerEnter(Collider collison)
        {
            if(!collison.gameObject.CompareTag("Bullet"))
            {
                //do damage to player if its player
                if(collison.gameObject.CompareTag("Player"))
                {
                    IDestroyable destroyable = collison.gameObject.GetComponent<IDestroyable>();
                    if(destroyable != null)
                    {
                        destroyable.OnDamage(20);
                    }
                }
                // reset the moving Rigidbody
                Rigidbody rBody = GetComponent<Rigidbody>();
                rBody.velocity = new Vector3(0f, 0f, 0f);
                rBody.angularVelocity = new Vector3(0f, 0f, 0f);

                UnrenderBullet();
                objectPool.Release(this);
            }

        }
        public void bulletRenderDelay()
        {
            UnrenderBullet();
            Invoke("renderBullet", 0.5f);
        }
        void renderBullet()
        {
            meshRenderer.enabled = true;
        }
        public void UnrenderBullet()
        {
            meshRenderer.enabled = false;
        }
    }
}
