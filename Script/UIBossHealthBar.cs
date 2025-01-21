using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealthBar : MonoBehaviour
{
    public Text bossName;
    public Slider slider;

    public bool isDead;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        bossName = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        slider.gameObject.SetActive(false);
    }

    public void SetBossName(string name)
    {
        bossName.text = name;
    }

    public void SetUIHealthBarToArctive()
    {
        slider.gameObject.SetActive(true);
    }

    public void SetHealthBarToInactive()
    {
        slider.gameObject.SetActive(false);
    }

    public void SetBossMaxHEalth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetBossCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;

        if (slider.value <= 0)
        {
            isDead = true;
            Destroy(slider.gameObject);
        }
    }
}
