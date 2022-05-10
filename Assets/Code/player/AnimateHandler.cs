using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace AF
{
    

    public class AnimateHandler : AnimationManager
    {
        PlayerManager playerManager;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;

        int vertical;
        int horizontal;
        public bool canRotate;
        

        public void Initialize()
        {

            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues( float verticalMovment , float horizontalMovment , bool isSprinting)
        {

            #region Vertical
            float v = 0;

                if (verticalMovment > 0 && verticalMovment< 0.55f)
                {
                    v = 0.5f;
                }
                else if (verticalMovment > 0.55f)
                {
                    v = 1;
                }
                else if (verticalMovment < 0 && verticalMovment> -0.55f)
                {
                    v = -0.5f;
                }
                else if (verticalMovment < -0.55f)
                {
                    v = -1;
                }
                else
                {
                    v = 0;
                }
            #endregion

            #region Horizontal
            float h = 0;

                if (horizontalMovment > 0 && horizontalMovment< 0.55f)
                {
                    h = 0.5f;
                }
                else if (horizontalMovment > 0.55f)
                {
                    h = 1;
                }
                else if (horizontalMovment < 0 && horizontalMovment> -0.55f)
                {
                    h = -0.5f;
                }
                else if (horizontalMovment < -0.55f)
                {
                    h = -1;
                }
                else
                {
                    h = 0;
                }
            #endregion

            //SPRINT ANIMATION WILL RUN EVEN IF THE PLAYER IS NOT MOVING SO I ADDED THE horizontalMovment CHECK
            // if (isSprinting && horizontalMovment > 0)
            if (isSprinting)
            {
                v = 2;
                h = horizontalMovment;
            }

            anim.SetFloat(vertical , v , 0.1f , Time.deltaTime);
            anim.SetFloat(horizontal , h , 0.1f , Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }
        
        public void StopRotation()
        {
            canRotate = false;
        }

        
        public void EnableCombo()
        {
            anim.SetBool("canDoCombo" , true);
        }
        
        public void DisableCombo()
        {
            anim.SetBool("canDoCombo" , false);
        }


        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
            return;

            float delta = Time.deltaTime;

            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta ;
            playerLocomotion.rigidbody.velocity = velocity;


        }



    }
}