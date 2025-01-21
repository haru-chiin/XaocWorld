using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("weaponn FX")]
    public ParticleSystem normalWeaponTrial;

    public void PlayWeaponFX()
    {
        normalWeaponTrial.Stop();

        if (normalWeaponTrial.isStopped)
        {
            normalWeaponTrial.Play();
        }
    }
}
