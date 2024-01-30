using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Health : MonoBehaviour,IDestroyable
{
    int health = 100;

    [SerializeField]
    TMP_Text healthTxt;

    public void OnDamage(int damage)
    {
        health -= damage;
        UpdateHealthTxt();
        if (health <=0)
        {
            Debug.Log("death");
        }
    }
    private void Start()
    {
        UpdateHealthTxt();
    }
    // Update is called once per frame
    void UpdateHealthTxt()
    {
        healthTxt.text = "Health: "+ health.ToString();
    }
}
