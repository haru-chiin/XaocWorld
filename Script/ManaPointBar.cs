using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaPointBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void SetmaxMana(float maxMana)
    {
        slider.maxValue = maxMana;
        slider.value = maxMana;
    }

    public void SetcurrentMana(float currentMana)
    {
        slider.value = currentMana;
        Debug.Log("Current stamina set to: " + currentMana);
    }
}
