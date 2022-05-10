using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AF
{
    public class enemyLocomotion : MonoBehaviour
    {


        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;
        public NavMeshAgent navMeshAgent;

        public Rigidbody enemyRigidBody;
        public CharacterStats currentTarget;


        public LayerMask detectionLayer;

        public float distanceFromTarget;
        public float stoppingDistance = 2f;
        public float rotationSpeed = 15;







        public void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyRigidBody = GetComponent<Rigidbody>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        }


        public void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigidBody.isKinematic = false;
        }



        public void handleDetection()
        {
            // create sphere over enemy
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);


            // iteration of all things hit
            for (int i = 0; i < colliders.Length; i++)
            {
                // objects hit with stats script attached
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                // if object has stats
                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    // if within field of view
                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        currentTarget = characterStats;
                    }
                }
            }

        }

        public void HandleMoveToTarget()
        {
            if (enemyManager.isPreformingAction ) return; // checking if preforming an action

            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            //if we are preforming an action, stop our motion
            if (enemyManager.isPreformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                navMeshAgent.enabled = false;
            }
            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                    
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    navMeshAgent.enabled = false;
                    enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    enemyRigidBody.velocity = Vector3.zero;
                    print(stoppingDistance);
                }
            }

            HandleRotationTowardsTarget();
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }


        private void HandleRotationTowardsTarget()
        {
            if (enemyManager.isPreformingAction)
            {
                //rotate manually
                Vector3 direction = currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = navMeshAgent.desiredVelocity;
                // Vector3 targetVelocity = enemyRigidBody.velocity; // VIDEO 31
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentTarget.transform.position);
                enemyRigidBody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }

        }
    }
}


