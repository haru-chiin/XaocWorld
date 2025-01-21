using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : characterStats
{
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyBossManager bossManager;
    public UIEnemyHealthBar EnemyHealthBar;

    public int soulsAwardedOnDeath = 50;
    public bool isBoss;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        bossManager = GetComponent<EnemyBossManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (!isBoss)
        {
            EnemyHealthBar.setMaxHealth(maxHealth);
        }
        
    }

    private int SetMaxHealthFromHealthLevel()
    {
        return healthLevel * 10;
    }

    public void TakeDamageNoAnimation(int damage)
    {
        if (!isBoss)
        {
            currentHealth = currentHealth - damage;
            EnemyHealthBar.SetHealth(currentHealth);
        }
        else if(isBoss && bossManager != null)
        {
            currentHealth = currentHealth - damage;
            bossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

/*        currentHealth = currentHealth - damage;
        EnemyHealthBar.SetHealth(currentHealth);*/
        if (currentHealth <= 0)
        {
            if (isDead)
                return;

            currentHealth = 0;
            /*HandleDeath();*/
            isDead = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isBoss)
        {
            currentHealth = currentHealth - damage;
            EnemyHealthBar.SetHealth(currentHealth);
        }
        else if(isBoss && bossManager != null)
        {
            currentHealth = currentHealth - damage;
            bossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        enemyAnimatorManager.PlayTargetAnimation("Hit", true);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        if (isDead)
            return;

        currentHealth = 0;

        /*animator.Play("Die");*/
        enemyAnimatorManager.PlayTargetAnimation("Die", true);
        isDead = true;
        

        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }
    }
}
