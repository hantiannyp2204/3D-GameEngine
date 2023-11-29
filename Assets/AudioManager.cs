using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    public void PlayShootSound(BaseWeapon currentWeapon,Transform muzzlePos)
    {
        audioSource.PlayOneShot(currentWeapon.weaponSound);
    }
}
