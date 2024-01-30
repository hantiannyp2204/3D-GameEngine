using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPackLogic : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves an object with the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the collided object has a script implementing IDestroyable
            IDestroyable destroyableScript = collision.gameObject.GetComponentInChildren<IDestroyable>();

            if (destroyableScript != null)
            {
                // Call a method or perform an action on the destroyable script
                destroyableScript.OnDamage(-50);
                Destroy(gameObject);
            }
        }
    }
}
