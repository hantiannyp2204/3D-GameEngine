using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Barrel : MonoBehaviour,ITarget, IDestroyable
{
    int health = 20;
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    ExplosionManager explosionManager;
    [SerializeField]
    FireManager fireManager;
    [SerializeField]
    GameObject bulletHole;

    public GameObject generateBulletHole()
    {
        return bulletHole;
    }

    public Transform getParent()
    {
        return this.transform;
    }

    public void OnDamage(int damage)
    {
        health -= damage;
        if(health<=0)
        {
            BlowUp();
        }
    }

    // Start is called before the first frame update
    void BlowUp()
    {
        explosionManager.Render(this.transform.position);
        fireManager.Render(this.transform.position);
        Destroy(gameObject);
    }
}
