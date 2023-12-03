using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCrate : MonoBehaviour, ITarget, IDestroyable
{
    [SerializeField] 
    private GameObject bullethole;
    int health = 40;
    public GameObject generateBulletHole()
    {
        return bullethole;
    }

    public Transform getParent()
    {
        return this.transform;
    }

    public void OnDamage(int damage)
    {
        health -= damage;
        if(health <=0)
        {
            BreakPlank();
        }
    }

    void BreakPlank()
    {
        Destroy(gameObject);
    }
}
