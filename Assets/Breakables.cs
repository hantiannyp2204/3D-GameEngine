using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Break : UnityEngine.MonoBehaviour
{
    public int health;
    public GameObject bulletHole;
    public System.Action onDestroy;

    public virtual void OnDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            NotifyDestroy();
        }
    }

    protected virtual void NotifyDestroy()
    {
        onDestroy?.Invoke();
        Destroy(this.gameObject);
    }

}
