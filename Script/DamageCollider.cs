using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager characterManager;
    Collider damageCollider;

    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            CharacterManager EnemyCharacterManager = collision.GetComponent<CharacterManager>();
            if (EnemyCharacterManager != null)
            {
                if (EnemyCharacterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }

            if (playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage);
            }
        }

        if (collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            CharacterManager EnemyCharacterManager = collision.GetComponent<CharacterManager>();
            if (EnemyCharacterManager != null)
            {
                if (EnemyCharacterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }
            if (enemyStats != null)
            {
                if (enemyStats.isBoss)
                {
                    enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
                
            }
        }
    }
}
