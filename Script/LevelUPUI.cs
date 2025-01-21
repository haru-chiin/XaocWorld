using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUPUI : MonoBehaviour
{
    public PlayerManager playerManager;
    /*public playerManager.playerStats playerManager.playerStats;*/

    public Button confrimPlayerLevelUpButton;

    [Header("Player Level")]
    public int currentPlayerLevel;
    public int projectedPlayerLevel;
    public Text currentPlayerLevelText;
    public Text projectedPlayerLevelText;

    [Header("Souls")]
    public Text currentSoulsText;
    public Text soulsRequiredTolevelUpText;
    private int SoulRequiredTolevelUP;
    private int baseLevelUpCost = 5;

    [Header("Health")]
    public Slider healthSlider;
    public Text currentHealthLevelText;
    public Text projectedHealthLevelText;

    [Header("Stamina")]
    public Slider staminaSlider;
    public Text currentStaminaLevelText;
    public Text projectedStaminaLevel;

    [Header("Mana")]
    public Slider manaslider;
    public Text currentManaLevelText;
    public Text projectedManaLevel;

    [Header("STR")]
    public Slider strSlider;
    public Text currentSTRLevel;
    public Text projectedSTRLevel;

    [Header("DEX")]
    public Slider dexSlider;
    public Text currentDEXLevel;
    public Text projectedDEXLevel;

    [Header("INT")]
    public Slider intSlider;
    public Text currentINTLevel;
    public Text projectedINTLevel;

    private void OnEnable()
    {
        currentPlayerLevel = playerManager.playerStats.playerLevel;
        currentPlayerLevelText.text = currentPlayerLevel.ToString();

        projectedPlayerLevel = playerManager.playerStats.playerLevel;
        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

        healthSlider.value = playerManager.playerStats.healthLevel;
        healthSlider.minValue = playerManager.playerStats.healthLevel;
        healthSlider.maxValue = 99;
        currentHealthLevelText.text = playerManager.playerStats.healthLevel.ToString();
        projectedHealthLevelText.text = playerManager.playerStats.healthLevel.ToString();

        staminaSlider.value = playerManager.playerStats.staminaLevel;
        staminaSlider.minValue = playerManager.playerStats.staminaLevel;
        staminaSlider.maxValue = 99;
        currentStaminaLevelText.text = playerManager.playerStats.staminaLevel.ToString();
        projectedStaminaLevel.text = playerManager.playerStats.staminaLevel.ToString();

        manaslider.value = playerManager.playerStats.manaLevel;
        manaslider.minValue = playerManager.playerStats.manaLevel;
        manaslider.maxValue = 99;
        currentManaLevelText.text = playerManager.playerStats.manaLevel.ToString();
        projectedManaLevel.text = playerManager.playerStats.manaLevel.ToString();

        strSlider.value = playerManager.playerStats.strengthLevel;
        strSlider.minValue = playerManager.playerStats.strengthLevel;
        strSlider.maxValue = 99;
        currentSTRLevel.text = playerManager.playerStats.strengthLevel.ToString();
        projectedSTRLevel.text = playerManager.playerStats.strengthLevel.ToString();

        dexSlider.value = playerManager.playerStats.dexterityLevel;
        dexSlider.minValue = playerManager.playerStats.dexterityLevel;
        dexSlider.maxValue = 99;
        currentDEXLevel.text = playerManager.playerStats.dexterityLevel.ToString();
        projectedDEXLevel.text = playerManager.playerStats.dexterityLevel.ToString();

        intSlider.value = playerManager.playerStats.intelligenceLevel;
        intSlider.minValue = playerManager.playerStats.intelligenceLevel;
        intSlider.maxValue = 99;
        currentINTLevel.text = playerManager.playerStats.intelligenceLevel.ToString();
        projectedINTLevel.text = playerManager.playerStats.intelligenceLevel.ToString();

        currentSoulsText.text = playerManager.playerStats.currentSoul.ToString();

        UpdateProjectedPlayerLevel();
    }

    public void ConfrimPlayerLevelUP()
    {
        playerManager.playerStats.playerLevel = projectedPlayerLevel;
        playerManager.playerStats.healthLevel = Mathf.RoundToInt(healthSlider.value);
        playerManager.playerStats.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
        playerManager.playerStats.manaLevel = Mathf.RoundToInt(manaslider.value);
        playerManager.playerStats.strengthLevel = Mathf.RoundToInt(strSlider.value);
        playerManager.playerStats.dexterityLevel = Mathf.RoundToInt(dexSlider.value);
        playerManager.playerStats.intelligenceLevel = Mathf.RoundToInt(intSlider.value);

        playerManager.playerStats.maxHealth = playerManager.playerStats.SetMaxHealthFromHealthLevel();
        playerManager.playerStats.maxStamina = playerManager.playerStats.SetMaxStaminaFromStaminaLevel();
        playerManager.playerStats.maxManaPoint = playerManager.playerStats.SetMaxManaFromManaLevel();

        playerManager.playerStats.currentSoul = playerManager.playerStats.currentSoul - SoulRequiredTolevelUP;
        playerManager.uIManager.soulCount.text = playerManager.playerStats.currentSoul.ToString();

        gameObject.SetActive(false);
    }

    private void CalculateSoulCostToLevelUP()
    {
        for(int i = 0; i < projectedPlayerLevel; i++)
        {
            SoulRequiredTolevelUP = SoulRequiredTolevelUP + Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) * 1.5f);
        }
    }

    private void UpdateProjectedPlayerLevel()
    {
        SoulRequiredTolevelUP = 0;

        
        projectedPlayerLevel = currentPlayerLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStats.healthLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStats.staminaLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(manaslider.value) - playerManager.playerStats.manaLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strSlider.value) - playerManager.playerStats.strengthLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexSlider.value) - playerManager.playerStats.dexterityLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intSlider.value) - playerManager.playerStats.intelligenceLevel;

        CalculateSoulCostToLevelUP();
        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();
        soulsRequiredTolevelUpText.text = SoulRequiredTolevelUP.ToString();



        if (playerManager.playerStats.currentSoul < SoulRequiredTolevelUP)
        {
            confrimPlayerLevelUpButton.interactable = false;
        }
        else
        {
            confrimPlayerLevelUpButton.interactable = true;
        }
    }

    public void UpdateHealthLevelSlider()
    {
        projectedHealthLevelText.text = healthSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateStaminaLevelSlider()
    {
        projectedStaminaLevel.text = staminaSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateManaLevelSlider()
    {
        projectedManaLevel.text = manaslider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateSTRLevelSlider()
    {
        projectedSTRLevel.text = strSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateDEXLevelSlider()
    {
        projectedDEXLevel.text = dexSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }
    public void UpdateINTLevelSlider()
    {
        projectedINTLevel.text = intSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }
}
