using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetmaxStamina(float maxStamina)
    {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
    }

    public void SetcurrentStamina(float currentStamina)
    {
        slider.value = currentStamina;
        Debug.Log("Current stamina set to: " + currentStamina);
    }
}
