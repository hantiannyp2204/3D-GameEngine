using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireVFX : UnityEngine.MonoBehaviour
{
    // deactivate after delay
    [SerializeField] private float timeoutDelay = 10f;

    [SerializeField] private ParticleSystem FlameParticle;
    [SerializeField] private ParticleSystem FireEmbers;
    [SerializeField] private ParticleSystem Light;
    private IObjectPool<FireVFX> objectPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<FireVFX> ObjectPool { set => objectPool = value; }

    public void Deactivate()
    {

        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }

    IEnumerator DeactivateRoutine(float delay)
    {

        yield return new WaitForSeconds(timeoutDelay);
        FlameParticle.Stop();
        FireEmbers.Stop();
        Light.Stop();
        yield return new WaitForSeconds(5);
        // release the projectile back to the pool
        objectPool.Release(this);
    }
}
