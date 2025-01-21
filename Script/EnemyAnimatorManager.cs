using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;
    EnemyEffectManager enemyEffectManager;
    EnemyStats enemyStats;
    EnemyBossManager enemyBossManager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyStats = GetComponentInParent<EnemyStats>();
        enemyBossManager = GetComponentInParent<EnemyBossManager>();
        enemyEffectManager = GetComponentInParent<EnemyEffectManager>();
    }

    public void PlayWeaponTrailFX()
    {
        enemyEffectManager.PlayWeaponFX(false);
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
        enemyManager.pendingCriticalDamage = 0;
    }

    public void CanRotate()
    {
        anim.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
        anim.SetBool("canRotate", false);
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }
    public void AwardSoulsOnDeath()
    {
        PlayerStats playerStats = FindAnyObjectByType<PlayerStats>();
        SoulCountBar soulCountBar = FindAnyObjectByType<SoulCountBar>();

        if (playerStats != null)
        {
            playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

            if (soulCountBar != null)
            {
                soulCountBar.SetSoulCountText(playerStats.currentSoul);
            }
        }


    }
    public void EnableIsParrying()
    {
        anim.SetBool("isParrying", true);
    }

    public void DisableIsParryng()
    {
        anim.SetBool("isParrying", false);
    }

    public void EnableCanBeRipoted()
    {
        enemyManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        enemyManager.canBeRiposted = false;
    }

    public void InstatiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFXTransform.transform);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity;

        if (enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= anim.deltaRotation;
        }
    }
}
