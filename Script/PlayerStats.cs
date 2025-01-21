using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerStats : characterStats
{
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    ManaPointBar manaPointBar;
    private PlayerManager playerManager;
    private PlayerAnimatorHandler playerAnimatorHandler;

    public float staminaRegenTimer = 0;

    private void Awake()
    {
        /*healthBar = FindAnyObjectByType<HealthBar>();*/
        staminaBar = FindAnyObjectByType<StaminaBar>();
        playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
        playerManager = GetComponent<PlayerManager>();
        manaPointBar = FindObjectOfType<ManaPointBar>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;

        maxManaPoint = SetMaxManaFromManaLevel();
        currentManaPoint = maxManaPoint;
        manaPointBar.SetmaxMana(maxManaPoint);
        manaPointBar.SetcurrentMana(currentManaPoint);
    }

/*    private int SetMaxHealthFromHealthLevel()
    {
        return healthLevel * 10;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        return staminaLevel * 10;
    }

    private float SetMaxManaFromManaLevel()
    {
        maxManaPoint = manaLevel * 10;
        return maxManaPoint;
    }*/

    public void TakeDamage(int damage)
    {
        if (playerManager.isInvulnerabel)
            return;

        if (isDead)
            return;

        currentHealth = currentHealth - damage;
        healthBar.SetCurrentHealth(currentHealth);
        playerAnimatorHandler.PlayTargetAnimation("Hit", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimatorHandler.PlayTargetAnimation("Die", true);
            isDead = true;
            RestartScene();
        }
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;
        if (currentHealth <= 0)
        {
            if (isDead)
                return;

            currentHealth = 0;
            isDead = true;
            RestartScene();
        }
    }

    public void TakeStamina(int stamina)
    {
        currentStamina = currentStamina - stamina;
        staminaBar.SetcurrentStamina(currentStamina);

/*        if (currentStamina <= 0)
        {
            currentStamina = 0;
            animatorHandler.PlayTargetAnimation("Hit", true);
        }*/
    }

    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            staminaRegenTimer = 0;
        }
        else
        {
            staminaRegenTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenTimer > 1f)
            {
                currentStamina += staminaRegenAmount * Time.deltaTime;
                staminaBar.SetcurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
    }

    public void healPlayer(int healAmount)
    {
        currentHealth = currentHealth + healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void DeductManaPoint(int manaPoints)
    {
        currentManaPoint = currentManaPoint - manaPoints;

        if(currentManaPoint < 0)
        {
            currentManaPoint = 0;
        }
        manaPointBar.SetcurrentMana(currentManaPoint);
    }

    public void AddSouls(int souls)
    {
        currentSoul = currentSoul + souls;
    }

    private void RestartScene()
    {
        StartCoroutine(RestartSceneCoroutine());
    }

    private IEnumerator RestartSceneCoroutine()
    {
        // Optionally wait for some time or show a UI
        yield return new WaitForSeconds(2f); // Adjust delay as needed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
