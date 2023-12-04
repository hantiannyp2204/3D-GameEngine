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
        if(health <=0)
        {
            Debug.Log("death");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthTxt.text = "Health: "+ health.ToString();
    }
}
