using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    // NOTES
    // The PlayerManager script refers to this script as this input handler's tickInput() is called on the playerManager's update()
    // any new inputs will need to first be added to the player input system ui
    // then refered to using function that will be called in the tickInput()
    // inputActions.PlayerActions.ACTION.performed += i => ACTION_BOOL = true;

    // Now that the boolean for action is true, handle action truth deligation in PlayerLocomotion.cs



    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        public float rollTimer_Threashold = 0.25f;


        // input variables
        public bool a_Input;
        public bool b_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool lb_Input;
        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;
        public bool inventory_input;
        public bool jump_input;
        public bool lockOn_input;
        public bool rightStick_right_input;
        public bool rightStick_left_input;

        private Vector3 _playerVelocity;
        public float RollInputTimer;
        public bool RollFlag;
        public bool lockOnFlag;
        public bool comboFlag;
        public bool SprintFlag;
        public bool inventoryFlag;
        public bool twoHandFlag;




        PlayerControls inputActions;
        PlayerLocomotion playerLocomotion;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        UI_manager ui_manager;
        CameraHandler cameraHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerStats playerStats;

        Vector2 movementInput;
        Vector2 CameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerStats = GetComponent<PlayerStats>();
            playerManager = GetComponent<PlayerManager>();
            ui_manager = FindObjectOfType<UI_manager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovment.Movment.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovment.Camera.performed += i => CameraInput = i.ReadValue<Vector2>();

                // // rolling - sprinting
                // inputActions.PlayerActions.Roll.started += i => b_Input = true;
                // inputActions.PlayerActions.Roll.canceled += i => b_Input = false;

                // player combos
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;

                // interactions
                inputActions.PlayerActions.NORTH.performed += i => a_Input = true;

                // jumping
                inputActions.PlayerActions.SOUTH.performed += i => jump_input = true;

                // item pick up
                inputActions.PlayerActions.Inventory.performed += i => inventory_input = true;

                // quick slot menu
                inputActions.PlayerActions.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerActions.DPadLeft.performed += i => d_Pad_Left = true;

                //lock on input
                inputActions.PlayerActions.LockOn.performed += i => lockOn_input = true;
                inputActions.PlayerMovment.LockOnTargetLeft.performed += i => rightStick_left_input = true;
                inputActions.PlayerMovment.LockOnTargetRight.performed += i => rightStick_right_input = true;




                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleMoveInput(delta);
            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleLockOnInput();
            handleTwoHandinput();
        }

        public void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = CameraInput.x;
            mouseY = CameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            // rollings
            inputActions.PlayerActions.Roll.started += i => b_Input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_Input = false;

            if (b_Input)
            {
                RollInputTimer += delta;
                SprintFlag = true;
                playerStats.TakeStaminaDamage(0.25f);
            }
            else
            {
                if (RollInputTimer > 0 && RollInputTimer < rollTimer_Threashold)
                {
                    SprintFlag = false;
                    RollFlag = true;
                    playerStats.TakeStaminaDamage(10);
                }
                RollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            // playerManager.cs ,  playerInventory.cs , PlayerAttacker.cs
            //what to do if you press RB / RT (LIGHT AND HEAVY ATK)
            if (rb_Input) //light atk
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;
                    if (playerManager.canDoCombo)
                        return;
                    playerAttacker.handleLightAttack(playerInventory.rightWeapon);
                }
            }
            if (rt_Input) //heavy atk
            {
                playerAttacker.handleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleQuickSlotInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.changeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventory.changeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    ui_manager.OpenSelectedWindow();
                    ui_manager.updateUI();
                    ui_manager.HUD_window.SetActive(false);
                }
                else
                {
                    ui_manager.CloseSelectedWindow();
                    ui_manager.CloseAllInventoryWindows();
                    ui_manager.HUD_window.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {

            // if input and unflagged
            if (lockOn_input && lockOnFlag == false)
            {
                lockOn_input = false;
                lockOnFlag = true;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_input && lockOnFlag)
            {
                // //clear array of targets
                lockOn_input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && rightStick_left_input)
            {
                rightStick_left_input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && rightStick_right_input)
            {
                rightStick_right_input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            // raise camera when locked on
            cameraHandler.SetCameraHeight();
        }

        private void handleTwoHandinput()
        {
            if (lb_Input)
            {
                lb_Input = false;
                twoHandFlag = !twoHandFlag;


                if (twoHandFlag)
                {
                    //enable
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    //disable
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

                }

            }

        }


    }
}
