using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHandler : AnimatorManager
{

    InputHandler inputHandler;
    PlayerStats playerStats;
    PlayerLocomotion2 playerLocomotion;
    PlayerManager playerManager;
    int vertical;
    int horizontal;

    public void Initialized()
    {
        anim = GetComponent<Animator>();
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion2>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void UpdateAnimatorValue(float verticalMove, float horizontalMove, bool isSprinting)
    {
        #region VErtical
        float v = 0;

        if(verticalMove > 0 && verticalMove < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMove > 0.55f)
        {
            v = 1;
        }
        else if (verticalMove < 0 && verticalMove > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMove < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion
        float h = 0;
        #region Horizontal
        if (horizontalMove > 0 && horizontalMove < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMove > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMove < 0 && horizontalMove > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMove < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }

        #endregion

        if (isSprinting && verticalMove > 0)
        {
            v = 2;
            h = horizontalMove;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }



    public void CanRotate()
    {
        anim.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
        anim.SetBool("canRotate",false);
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo",true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }
    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false)
            return;

        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        playerStats.TakeDamageNoAnimation(playerManager.pendingCriticalDamage);
        playerManager.pendingCriticalDamage = 0;
    }

    public void DisableCollision()
    {
        playerLocomotion.characterCollider.enabled = false;
        playerLocomotion.characterCollisionBlockerCollider.enabled = false;
    }

    public void EnableCollison()
    {
        playerLocomotion.characterCollider.enabled = true;
        playerLocomotion.characterCollisionBlockerCollider.enabled = true;
    }

    public void EnableisInvulnerabel()
    {
        anim.SetBool("isInvulnerabel", true);
    }

    public void DisableisInvulnerabel()
    {
        anim.SetBool("isInvulnerabel", false);
    }

    public void EnableIsParrying()
    {
        playerManager.isParrying = true;
    }

    public void DisableIsParryng()
    {
        playerManager.isParrying = false;
    }

    public void EnableCanBeRipoted()
    {
        playerManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        playerManager.canBeRiposted = false;
    }

}
