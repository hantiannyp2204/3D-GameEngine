using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    [SerializeField]
    private AudioSource audioSource;
    private void Start()
    {
        gm = this;
    }
    public void PlayShootSound(BaseWeapon currentWeapon)
    {
        audioSource.PlayOneShot(currentWeapon.weaponSound); 
    }
}
