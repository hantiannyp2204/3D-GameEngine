using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, ITarget, IDestroyable
{
    [SerializeField]
    GameObject brokenCrate;
    int health = 80;
    [SerializeField]
    GameObject bulletHole;
    void BecomeBroken()
    {
        GameObject brokenSFX = Instantiate(brokenCrate);
        brokenSFX.transform.position = this.transform.position;
        brokenSFX.transform.SetParent(null);
        Destroy(gameObject);
    }

    public void OnDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            BecomeBroken();
        }
    }
    public GameObject generateBulletHole()
    {
        return bulletHole;
    }
    public Transform getParent()
    {
        return this.transform;
    }
}
