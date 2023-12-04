using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeInventory : MonoBehaviour
{
    [SerializeField]
    GrenadeManager grenadeManager;
    [SerializeField]
    Transform hand;
    [SerializeField]
    Camera mainCamera;

    bool nadeOut = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q) && nadeOut == false)
        {
            nadeOut = true;
            grenadeManager.HoldNade(hand);

        }
        if (Input.GetKeyUp(KeyCode.Q) && nadeOut == true)
        {
            nadeOut = false;
            grenadeManager.ThrowNade(mainCamera.transform.forward);
        }
    }
}
