using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExplosionVFX : UnityEngine.MonoBehaviour
{
    // deactivate after delay
    [SerializeField] private float timeoutDelay = 3f;

    private IObjectPool<ExplosionVFX> objectPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<ExplosionVFX> ObjectPool { set => objectPool = value; }

    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

     

        // release the projectile back to the pool
        objectPool.Release(this);
    }
}
