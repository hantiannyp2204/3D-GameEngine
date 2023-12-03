using System.Collections;
using UnityEngine;

public class EffectsManager : UnityEngine.MonoBehaviour
{
    [SerializeField] private ParticleSystem _muzzleFlash;

    public void PlayParticle(BaseWeapon currentWeapon, Transform muzzlePos)
    {
        _muzzleFlash.Play();
        _muzzleFlash.transform.position = muzzlePos.position;   
    }
    private void Update()
    {
        
    }
}
