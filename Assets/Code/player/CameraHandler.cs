using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{

    public class CameraHandler : MonoBehaviour
    {
        public static CameraHandler singleton;

        public Transform targetTranfsorm;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public LayerMask ignoreLayers;
        public LayerMask enviromentLayer;

        public float lookspeed = 0.1f;
        public float followspeed = 0.1f;
        public float pivotspeed = 0.03f;

        private float targetPosition;
        private float defaultPosistionl;
        private float LookAngle;
        private float PivotAngle;

        public float minimumPivot = -35;
        public float maxPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOfffset = 0.2f;
        public float minCollisionOfffset = 0.2f;


        // LOCK ON 
        public Transform nearestLockOnTarget;
        List<CharacterManager> availableTargets;
        InputHandler inputHandler;
        PlayerManager playerManager;
        public Transform currentLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;
        PlayerStats playerStats;



        private void Awake()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            targetTranfsorm = FindObjectOfType<PlayerManager>().transform;
            playerStats = FindObjectOfType<PlayerStats>();

            availableTargets = new List<CharacterManager>();

            singleton = this;
            myTransform = transform;
            defaultPosistionl = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);


        }

        private void Start()
        {
            enviromentLayer = LayerMask.NameToLayer("Enviroment");
        }



        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTranfsorm.position, ref cameraFollowVelocity, delta / followspeed);
            myTransform.position = targetPosition;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mousXInput, float mouseYInput)
        {
            // not locked on
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                LookAngle += (mousXInput * lookspeed) / delta;
                PivotAngle -= (mouseYInput * pivotspeed) / delta;
                PivotAngle = Mathf.Clamp(PivotAngle, minimumPivot, maxPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = LookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = PivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            // locked on Running after handleLockOn()
            {
                // float velocity = 0;

                // currentLockOnTarget error!!!
                Vector3 dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;

            }

        }

        private void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosistionl;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOfffset);

            }

            if (Mathf.Abs(targetPosition) < minCollisionOfffset)
            {
                targetPosition = -minCollisionOfffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;


        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            // create sphere of collision to understand whos nearby
            Collider[] colliders = Physics.OverlapSphere(targetTranfsorm.position, 26);

            // loop through items collided with and check if any have characterManager scripts
            for (var i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTranfsorm.position;
                    float distanceFromTarget = Vector3.Distance(targetTranfsorm.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if (
                        character.transform.root != targetTranfsorm.transform.root
                    && viewableAngle > (-1 * playerStats.angleThreashhold) && viewableAngle < playerStats.angleThreashhold
                    && distanceFromTarget <= playerStats.maximumLockOnDistance
                    )
                    {
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                            if (hit.transform.gameObject.layer == enviromentLayer)
                            {
                                //LAYER MASK CANT GO THRU WALLZ
                            }
                            else
                            {
                                availableTargets.Add(character);
                            }
                        }

                    }

                }
            }

            if (availableTargets.Count == 0)
            {
                inputHandler.lockOnFlag = false;
            }




            // NEAREST ENEMY BY FIRST ITEM IN ARRAY
            // if (availableTargets.Count > 0)
            // {
            //     nearestLockOnTarget = availableTargets[0].lockOnTransform;
            // }
            // else
            // {
            //     inputHandler.lockOnFlag = false;
            //     currentLockOnTarget = null;
            // }



            // // NEAREST ENEMY BY SHORTEST DISTANCE
            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTranfsorm.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k].lockOnTransform;
                }

                if (inputHandler.lockOnFlag && currentLockOnTarget)
                {
                    Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                    var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;
                    if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[k].lockOnTransform;
                    }
                    if (relativeEnemyPosition.x < 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[k].lockOnTransform;
                    }
                }

            }


        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            currentLockOnTarget = null;
            nearestLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUNLockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUNLockedPosition, ref velocity, Time.deltaTime);
            }
        }


    }

}

