using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScipt : MonoBehaviour
{
    public float shakedDuration;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shake();
        }
    }

    public void Shake()
    {
        StartCoroutine(PerformSimpleShake());
    }
    IEnumerator PerformSimpleShake()
    {
        Quaternion startRotation = transform.localRotation; // Store the original rotation
        float elapsedTime = 0.0f;

        while (elapsedTime < shakedDuration)
        {
            elapsedTime += Time.deltaTime;

    
            transform.localRotation = Quaternion.Euler(new Vector3(
                transform.localEulerAngles.x + UnityEngine.Random.Range(-1, 1),
                transform.localEulerAngles.y + UnityEngine.Random.Range(-1, 1),
                transform.localEulerAngles. z + UnityEngine.Random.Range(-1, 1)
            ));


            yield return null;
        }



    }

}
