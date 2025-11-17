using UnityEngine;
using UnityEngine.InputSystem;

namespace Ashes.PlayerCntrl
{
    public class PlayerCntrl : MonoBehaviour
    {
        // Character Movement Controls
        //----------------------------
        [Header("Character Movement Controls")]
        [SerializeField] private float playerSpeed = 8.0f;
        [SerializeField] private float playerRotation = 400.0f;
        [SerializeField] private float GRAVITY = -9.81f;
        [SerializeField] private float jumpModifier = 2.0f;
        [SerializeField] private float jumpHeight = 2.0f;
        [SerializeField] private float moveSpeed = 6.0f;
        [SerializeField] private bool isGrounded = true;
        [SerializeField] private int maxNumberJumps = 3;
       
        private Vector2 playerMovement;
        private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
        private CharacterController charCntrl;

        private Vector3 turn = new Vector3();

        private int numberOfJumps = 0;

        private float movespeed = 0.0f;

        private float yHeight = 0.0f;

        // Animation Controls
        //-------------------
        private Animator animator;

        public PlayerState currentState = PlayerState.IDLE;

        private PlayerInputCntrl playerInputCntrl = PlayerInputCntrl.IDLE;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            charCntrl = GetComponent<CharacterController>();
        }

        void xxxUpdate()
        {
            isGrounded = charCntrl.isGrounded;

            if (isGrounded && yHeight < 0)
            {
                yHeight = -1.0f;
            } else
            {
                yHeight += GRAVITY * Time.deltaTime;
            }

            if (isGrounded)
            {
                RotatePlayer();
            }

            if ((playerInputCntrl == PlayerInputCntrl.JUMP_REQUEST) && isGrounded)
            {
                yHeight += Mathf.Sqrt(jumpHeight * -jumpModifier * GRAVITY);

                playerInputCntrl = PlayerInputCntrl.MOVEMENT;
            }

            moveDirection.y = yHeight;

            charCntrl.Move(moveSpeed * moveDirection * Time.deltaTime);
        }

        void Update()
        {
            isGrounded = charCntrl.isGrounded;

            ApplyGravity();

            switch (currentState)
            {
                case PlayerState.IDLE:
                    currentState = State_Idle(playerInputCntrl);
                    break;
                case PlayerState.SPRINTING:
                    currentState = State_Sprint();
                    break;
                case PlayerState.START_JUMP:
                    currentState = State_StartJumping();
                    break;
                case PlayerState.JUMPING:
                    currentState = State_Jumping();
                    break;
            }

            switch(playerInputCntrl)
            {
                /*case PlayerInputCntrl.JUMP:
                    if (numberOfJumps < maxNumberJumps)
                    {
                        currentState = PlayerState.START_JUMP;
                        numberOfJumps++;
                    } 

                    playerInputCntrl = PlayerInputCntrl.DO_NOTHING;
                    
                    break;*/

                case PlayerInputCntrl.DASH:
                    Debug.Log("Dash Request ...");
                    break;
            }

            if (playerInputCntrl != PlayerInputCntrl.IDLE)
            {
                RotatePlayer();
                MovePlayer();
            }

            
        }

        #region *** Finite State Machine ***

        private PlayerState State_StartJumping()
        {
            yHeight = Mathf.Sqrt(jumpHeight * -jumpModifier * GRAVITY);

            animator.SetBool("finishjump", false);
            animator.SetBool("startjump", true);

            return (PlayerState.JUMPING);
        }

        private PlayerState State_Jumping()
        {
            PlayerState nextState = PlayerState.JUMPING;

            if (playerInputCntrl == PlayerInputCntrl.JUMP_REQUEST)
            {
                if (numberOfJumps < maxNumberJumps)
                {
                    nextState = PlayerState.START_JUMP;
                    numberOfJumps++;
                }
            } else
            {
                if (isGrounded)
                {
                    nextState = PlayerState.SPRINTING;
                    animator.SetBool("startjump", false);
                    animator.SetBool("finishjump", true);
                    animator.SetBool("isfalling", false);
                    numberOfJumps = 0;
                } 
            }

            playerInputCntrl = PlayerInputCntrl.DO_NOTHING;

            return (nextState);
        }

        private PlayerState State_Idle(PlayerInputCntrl playerInput)
        {
            PlayerState nextState = PlayerState.IDLE;

            if (playerInput == PlayerInputCntrl.MOVEMENT)
            {
                nextState = PlayerState.SPRINTING;
            }
            else
            {
                animator.SetBool("run", false);
            }

            return (nextState);
        }

        private PlayerState State_Sprint()
        {
            PlayerState nextState = PlayerState.SPRINTING;

            switch (playerInputCntrl)
            {
                //case PlayerInputCntrl.MOVEMENT:
                    //nextState = PlayerState.SPRINTING;
                    //break;
                case PlayerInputCntrl.JUMP_REQUEST:
                    if (numberOfJumps < maxNumberJumps)
                    {
                        nextState = PlayerState.START_JUMP;
                        numberOfJumps++;
                    }

                    playerInputCntrl = PlayerInputCntrl.DO_NOTHING;
                    break;
            }

            // If you sprint off a surface, start jumping
            //-------------------------------------------
            if (!isGrounded)
            {
                nextState = PlayerState.JUMPING;
                animator.SetBool("isfalling", true);
            }

            return (nextState);
        }

        #endregion

        #region *** Finite State Machine Support Functions ***

        /**
         * ApplyGravity() - 
         */
        private void ApplyGravity()
        {
            if (isGrounded && yHeight < 0)
            {
                yHeight = -1.0f;
            }
            else
            {
                yHeight += GRAVITY * Time.deltaTime;
            }
        }

        /**
         * RotatePlayer() - 
         */
        private void RotatePlayer()
        {
            turn.x = moveDirection.x;
            turn.y = 0.0f;
            turn.z = moveDirection.z;

            if (turn != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(turn, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotation * Time.deltaTime);
            }

        }

        /**
         * MovePlayer() - 
         */
        private void MovePlayer()
        {
            animator.SetFloat("movespeed", movespeed);

            moveDirection.y = yHeight;

            charCntrl.Move(moveSpeed * moveDirection * Time.deltaTime);
        }

        #endregion

        #region *** Player Movement Functions ***

        public void OnMove(InputAction.CallbackContext context)
        {
            playerMovement = context.ReadValue<Vector2>();

            if (playerMovement != Vector2.zero)
            {
                playerInputCntrl = PlayerInputCntrl.MOVEMENT;

                moveDirection.x = playerMovement.x; // Horizontal Axis Controls
                moveDirection.y = 0.0f;
                moveDirection.z = playerMovement.y; // Vertical Axis Controls

                movespeed = playerMovement.magnitude;
            }
            else
            {
                playerInputCntrl = PlayerInputCntrl.IDLE;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                playerInputCntrl = PlayerInputCntrl.JUMP_REQUEST;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                playerInputCntrl = PlayerInputCntrl.DASH;
            }
        }

        #endregion
    }

    public enum PlayerState
    {
        IDLE,
        SPRINTING,
        JUMPING,
        START_JUMP
    }

    public enum PlayerInputCntrl
    {
        IDLE,
        MOVEMENT,
        JUMP_REQUEST,
        DASH,
        DO_NOTHING
    }
}


