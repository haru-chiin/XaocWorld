using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWall : MonoBehaviour
{
    public AudioSource bossMusicSource;
    private void Awake()
    {
        gameObject.SetActive(true);
    }

    public void ActivateFogWall()
    {
        gameObject.SetActive(true);
        if (bossMusicSource != null)
        {
            bossMusicSource.Play();
        }
    }

    public void DeactiveFogWall()
    {
        gameObject.SetActive(false);
        if (bossMusicSource != null)
        {
            bossMusicSource.Stop();
        }
    }
}
