using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScipt : UnityEngine.MonoBehaviour
{
    public float shakedDuration;

    private Camera mainCam;

    public float shakeStrength ;

    public bool playerAiming = false;
    private void Start()
    {
        mainCam= GetComponent<Camera>();
    }
    // Update is called once per frame
    public void FOVchange(BaseWeapon currentWeapon, bool isAiming)
    {
        if (isAiming == true)
        {
            playerAiming = true;
            FOVchange(currentWeapon.AdsFOV, currentWeapon.AdsSpeed);
        }
        else
        {
            playerAiming = false;
            if (mainCam.fieldOfView != 60)
            {
                FOVchange(60, 0.3f);
            }

        }
    }
    public void FOVchange(float newFOV, float timeToChange)
    {
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, newFOV, 10 / timeToChange * Time.deltaTime);
    }
    public void FOVSprinting(bool isSprinting)
    {
        if(playerAiming != true)
        {
            if (isSprinting == true)
            {
                FOVchange(75, 0.3f);
            }
            else
            {
                FOVchange(60, 0.3f);
            }
        }

    }
    public void Shake(BaseWeapon currentWeapon, Transform muzzleFlash)
    {
        Shake(currentWeapon.weaponKick);
    }
    void Shake(float strength)
    {
        StartCoroutine(PerformSimpleShake(strength));
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
