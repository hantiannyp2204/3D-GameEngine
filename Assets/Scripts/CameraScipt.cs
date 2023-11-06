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

    }

    public void Shake(float inStrength)
    {
        StartCoroutine(PerformSimpleShake(inStrength));
    }
    IEnumerator PerformSimpleShake(float inStrength)
    {
        Quaternion startRotation = transform.localRotation; // Store the original rotation
        float elapsedTime = 0.0f;

        while (elapsedTime < shakedDuration)
        {
            elapsedTime += Time.deltaTime;

            var strength = inStrength * Mathf.Lerp(1, 0, elapsedTime / shakedDuration);
            transform.localRotation = Quaternion.Euler(new Vector3(
                transform.localEulerAngles.x + UnityEngine.Random.Range(-1, 1) * strength,
                transform.localEulerAngles.y + UnityEngine.Random.Range(-1, 1) * strength,
                transform.localEulerAngles. z + UnityEngine.Random.Range(-1, 1) * strength
            ));


            yield return null;
        }



    }

}
