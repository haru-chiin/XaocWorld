using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Transform lockOnTransform;
/*    public BoxCollider backStabBoxCollider;*/
    public CriticalDamageCollider backStabCollider;
    public CriticalDamageCollider riposeCollider;
    public bool canBeRiposted;
    public bool canBeParry;
    public bool isParrying;
    public bool isRotatingWithRootMotion;
    public bool canRotate;
    public bool isTwoHandingWeapon;

    public int pendingCriticalDamage;
}
