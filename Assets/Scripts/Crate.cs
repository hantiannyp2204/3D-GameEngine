using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private int health = 20;

    public void OnDamaged(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy();
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
