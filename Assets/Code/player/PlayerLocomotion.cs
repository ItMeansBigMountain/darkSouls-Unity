using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{

    public class PlayerLocomotion : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimateHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection RayCast")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;

        LayerMask ignoreForGroundCheck;
        public float inAirTimer;


        [Header("Movment Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallSpeed = 45;
        [SerializeField]
        float walkingSpeed = 3;
        [SerializeField]
        float jumpHeight = 300.0f;
        float gravity_pressure = -98.5f;


        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }



        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimateHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11); //ignores layers 8 and 11
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            if (inputHandler.lockOnFlag)
            {
                if (inputHandler.SprintFlag || inputHandler.RollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                    targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;

                }

            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                float moveOverride = inputHandler.moveAmount;

                targetDir = cameraObject.forward * inputHandler.vertical;
                targetDir += cameraObject.right * inputHandler.horizontal;

                targetDir.Normalize();
                targetDir.y = 0;

                if (targetDir == Vector3.zero)

                    targetDir = myTransform.forward;

                float rs = rotationSpeed;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                myTransform.rotation = targetRotation;
            }


        }

        public void HandleMovement(float delta)
        {

            if (inputHandler.RollFlag)
                return;

            if (playerManager.isInteracting)
                return;



            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.SprintFlag && inputHandler.moveAmount > 0.5f) //checking for sprinting
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (inputHandler.moveAmount < 0.5)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }

            }


            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && inputHandler.SprintFlag == false)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);

            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }


            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.anim.GetBool("IsInteracting")) return;

            // ROLL ACTIVATED
            if (inputHandler.RollFlag)
            {
                // Debug.Log("Move Direction Vector: " + moveDirection);
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;


                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    rigidbody.AddForce(moveDirection * 3000);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Backstep", true);
                    rigidbody.AddRelativeForce(Vector3.back * 3000);
                }

            }

        }

        public void HandleFalling(float delta, Vector3 moveDirection) //bugged function, commented out in PlayerManager.Update()
        {

            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;



            //IF SOMETHING IS DIRECTLY IN FRONT OF YOU, YOURE NOT MOVING
            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }



            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallSpeed);
                rigidbody.AddForce(moveDirection * fallSpeed / 5f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;
            targetPosition = myTransform.position;

            //DEBUGGER
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceToBeginFall, Color.red, 0.1f, false);

            //grounded
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceToBeginFall, ignoreForGroundCheck))
            {

                normalVector = hit.normal;
                Vector3 tp = hit.point;
                targetPosition.y = tp.y;
                playerManager.isGrounded = true;

                if (inAirTimer > 0.05f)
                {
                    animatorHandler.PlayTargetAnimation("Land", false);

                    inAirTimer = 0;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;


            }
            else
            {
                // Debug.Log("falling");
                playerManager.isInAir = true;

                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == true)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }
                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);

                    // if commented from the top and uncommented here, then rolls work but fall doesnt *******************
                    //    playerManager.isInAir = true; 

                    //When player is touching ground no matter what is commented out from above, PLAYER CANNOT ROLL WHILE TOUCHING GROUND <---- fixed on line 218 else block
                }

                //Video11
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    myTransform.position = targetPosition;
                }


            }

            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }

        }

        public void HandleJumping(float delta, Vector3 moveDirection)
        {
            if (playerManager.isInteracting) return;

            if (inputHandler.jump_input)
            {
                if (inputHandler.moveAmount > 0)
                // if (playerManager.playerGravity.velocity.y == 0f )
                {
                    if (playerManager.isGrounded)
                    {
                        moveDirection = cameraObject.forward * inputHandler.vertical;
                        moveDirection += cameraObject.right * inputHandler.horizontal;
                        animatorHandler.PlayTargetAnimation("Jumping", true);
                        moveDirection.y = 0f;
                        Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = jumpRotation;

                        playerManager.playerGravity.AddForce(0, Mathf.Sqrt(jumpHeight * -1.0f * gravity_pressure), 0, ForceMode.Impulse);
                    }
                }
                else
                {
                    print("Jump Unavailable");
                }

            }
        }

        #endregion

    }

}

