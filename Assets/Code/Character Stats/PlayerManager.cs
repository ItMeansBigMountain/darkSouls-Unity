using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public class PlayerManager : CharacterManager
    {


        // SCRIPTS
        InputHandler inputhandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;
        InteractableUI interactableUI;





        public GameObject interactableUIGameObject;
        public GameObject itemInteractable_GameObject;




        // VARIABLES
        public Rigidbody playerGravity;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;



        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        void Start()
        {
            inputhandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerGravity = GetComponentInChildren<Rigidbody>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();

        }


        void Update()
        {
            float delta = Time.deltaTime;

            // animation detection
            isInteracting = anim.GetBool("IsInteracting");

            // combo detection
            canDoCombo = anim.GetBool("canDoCombo");

            //input detection
            inputhandler.TickInput(delta);

            // interaction
            CheckForInteractableObject();


            // animation and movement
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        private void FixedUpdate()
        {
            // // animation and movement
            // float delta = Time.fixedDeltaTime;
            // playerLocomotion.HandleMovement(delta);
            // playerLocomotion.HandleRollingAndSprinting(delta);
            // playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            // playerLocomotion.HandleJumping(delta, playerLocomotion.moveDirection);
        }

        private void LateUpdate()
        {
            // reset input variables after frame ends
            inputhandler.RollFlag = false;
            inputhandler.SprintFlag = false;
            inputhandler.rb_Input = false;
            inputhandler.rt_Input = false;
            inputhandler.d_Pad_Up = false;
            inputhandler.d_Pad_Down = false;
            inputhandler.d_Pad_Left = false;
            inputhandler.d_Pad_Right = false;
            inputhandler.jump_input = false;
            inputhandler.a_Input = false;
            inputhandler.inventory_input = false;



            // CAMERA FOLLOW 
            float delta = Time.deltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputhandler.mouseX, inputhandler.mouseY);
            }



            // count how long in air... judge in locomotion algo
            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }

        }


        public void CheckForInteractableObject()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactables interactableObject = hit.collider.GetComponent<Interactables>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputhandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactables>().interact(this);
                        }



                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractable_GameObject != null && inputhandler.a_Input == true)
                {
                    itemInteractable_GameObject.SetActive(false);
                }
            }
        }

    }

}



