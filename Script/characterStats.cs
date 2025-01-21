using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterStats : MonoBehaviour
{
/*    [SerializeField] protected int healthLevel = 10; // Gunakan 'protected'*/
    [SerializeField] public int maxHealth;
    public int currentHealth;

/*    [SerializeField] protected int staminaLevel = 10; // Gunakan 'protected'*/
    [SerializeField] public float maxStamina;
    [SerializeField] public float currentStamina;
    [SerializeField] protected float staminaRegenAmount = 30;
    public bool isDead;


    public float maxManaPoint;
    public float currentManaPoint;

    public int currentSoul = 0;

    [Header("Chara Level")]
    public int playerLevel = 1;

    [Header("Stat Levels")]
    public int healthLevel = 10;
    public int staminaLevel = 10;
    public int manaLevel = 10;
    public int strengthLevel = 10;
    public int dexterityLevel = 10;
    public int intelligenceLevel = 10;

    /*    // Properti untuk mengakses currentHealth
        public int CurrentHealth
        {
            get { return currentHealth; }
            protected set { currentHealth = value; }
        }*/

    // Properti untuk mengakses maxHealth (opsional, jika diperlukan)
    public int MaxHealth
    {
        get { return maxHealth; }
        protected set { maxHealth = value; }
    }

    // Properti untuk mengakses stamina (opsional, jika diperlukan)
    public float CurrentStamina
    {
        get { return currentStamina; }
        protected set { currentStamina = value; }
    }

    public float MaxStamina
    {
        get { return maxStamina; }
        protected set { maxStamina = value; }
    }

    public int SetMaxHealthFromHealthLevel()
    {
        return healthLevel * 10;
    }

    public float SetMaxStaminaFromStaminaLevel()
    {
        return staminaLevel * 10;
    }

    public float SetMaxManaFromManaLevel()
    {
        maxManaPoint = manaLevel * 10;
        return maxManaPoint;
    }
}
