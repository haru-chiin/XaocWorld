using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHandler : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerManager playerManager;

    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform mytransform;
    private Vector3 cameraTransformPost;
    public LayerMask ignoreLayer;
    public LayerMask environmentLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static cameraHandler singleton;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPost;
    private float lookAngle;
    private float pivotAngle;
    public float minimumPivot = -35;
    public float maximumPivot = 35;
    public float maximumPivotOnLock = 15;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    public float lockedPivotPosition = 2.25f;
    public float unlockPivotPosition = 1.65f;

    public CharacterManager currentLockOnTarget;

    List<CharacterManager> availableTarget = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockTarget;
    public CharacterManager rightLockTarget;
    public float maximumLockOnDistance = 30;

    private void Awake()
    {
        singleton = this;
        mytransform = transform;
        defaultPost = cameraTransform.localPosition.z;
        ignoreLayer = ~(1 << 8 | 1 << 9 | 1 << 10 | 1 << 11 | 1 << 13 | 1 << 14);
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputHandler = FindObjectOfType<InputHandler>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        environmentLayers = LayerMask.NameToLayer("Environment");
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosisition = Vector3.SmoothDamp
            (mytransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        mytransform.position = targetPosisition;

        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if(inputHandler.lockOnFlag == false && currentLockOnTarget == null)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            mytransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            /*float velocity = 0;

            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;*/
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();

            // Rotasi horizontal (yaw) diatur pada root
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            // Rotasi vertikal (pitch) diatur pada pivot kamera
            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;

            // Mengunci rotasi vertikal pada batas yang diinginkan
            eulerAngle.x = Mathf.Clamp(eulerAngle.x, minimumPivot, maximumPivotOnLock);
            eulerAngle.y = 0; // Tetap mengatur rotasi horizontal ke 0

            cameraPivotTransform.localEulerAngles = eulerAngle;
        }

    }

    private void HandleCameraCollision(float delta)
    {
        targetPosition = defaultPost;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivotTransform.transform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayer))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }
        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraTransformPost.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta/ 0.2f);
        cameraTransform.localPosition = cameraTransformPost;
    }

    public void HandleLock()
    {
        float shortestDistance = Mathf.Infinity;
        float shortDistanceOfLeftTarget = -Mathf.Infinity;
        float shortDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 26);

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if(character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;

                if(character.transform.root != targetTransform.transform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                {
                    if(Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                        if(hit.transform.gameObject.layer == environmentLayers)
                        {
                            inputHandler.lockOnFlag = false;
                        }
                        else
                        {
                            availableTarget.Add(character);
                        }
                    }
                    /*availableTarget.Add(character);*/
                }
            }
        }

        for(int k = 0; k < availableTarget.Count; k++)
        {
            float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTarget[k].transform.position);

            if(distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTarget[k];
            }

            if (inputHandler.lockOnFlag)
            {
                //Vector3 relativeEnemyPos = currentLockOnTarget.transform.InverseTransformPoint(availableTarget[k].transform.position);
                //var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTarget[k].transform.position.x;
                //var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTarget[k].transform.position.x;
                Vector3 relativeEnemyPos = inputHandler.transform.InverseTransformPoint(availableTarget[k].transform.position);
                var distanceFromLeftTarget = relativeEnemyPos.x;
                var distanceFromRightTarget = relativeEnemyPos.x;

                if (relativeEnemyPos.x <= 0.00 && distanceFromLeftTarget > shortDistanceOfLeftTarget && availableTarget[k] != currentLockOnTarget)
                {
                    shortDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTarget[k];
                }

                else if( relativeEnemyPos.x >= 0.00 && distanceFromRightTarget < shortDistanceOfRightTarget && availableTarget[k] != currentLockOnTarget)
                {
                    shortDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTarget = availableTarget[k];
                }
            }
        }
    }

    public void ClearLockOnTarget()
    {
        availableTarget.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
    }

    public void setCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnLockedPosition = new Vector3(0, unlockPivotPosition);

        if(currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnLockedPosition, ref velocity, Time.deltaTime);
        }
    }
}

