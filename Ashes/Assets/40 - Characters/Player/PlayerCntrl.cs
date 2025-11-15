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
       
        private Vector2 playerMovement;
        private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
        private CharacterController charCntrl;

        private Vector3 turn = new Vector3();

        private bool isGrounded = true;

        private float ySpeed = 0.0f;

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

            if (isGrounded && ySpeed < 0)
            {
                ySpeed = -1.0f;
            } else
            {
                ySpeed += GRAVITY * Time.deltaTime;
            }

            if (isGrounded)
            {
                RotatePlayer();
            }

            if ((playerInputCntrl == PlayerInputCntrl.JUMP) && isGrounded)
            {
                ySpeed += Mathf.Sqrt(jumpHeight * -jumpModifier * GRAVITY);

                playerInputCntrl = PlayerInputCntrl.MOVEMENT;
            }

            moveDirection.y = ySpeed;

            charCntrl.Move(moveSpeed * moveDirection * Time.deltaTime);
        }

        void Update()
        {
            isGrounded = charCntrl.isGrounded;

            if (isGrounded && ySpeed < 0)
            {
                ySpeed = -1.0f;
            }
            else
            {
                ySpeed += GRAVITY * Time.deltaTime;
            }

            switch (currentState)
            {
                case PlayerState.IDLE:
                    currentState = State_Idle(playerInputCntrl);
                    break;
                case PlayerState.RUNNING:
                    currentState = State_Running(playerInputCntrl);
                    break;
                case PlayerState.JUMP:
                    currentState = State_Jump();
                    break;
                case PlayerState.JUMPING:
                    currentState = State_Jumping();
                    break;
            }

            switch(playerInputCntrl)
            {
                case PlayerInputCntrl.JUMP:
                    currentState = PlayerState.JUMP;
                    playerInputCntrl = PlayerInputCntrl.DO_NOTHING;
                    break;
            }

            if (playerInputCntrl != PlayerInputCntrl.IDLE)
            {
                RotatePlayer();
                MovePlayer();
            }
        }

        #region *** Finite State Machine ***

        private PlayerState State_Jump()
        {
            ySpeed += Mathf.Sqrt(jumpHeight * -jumpModifier * GRAVITY);

            return (PlayerState.JUMPING);
        }

        private PlayerState State_Jumping()
        {
            return (isGrounded ? PlayerState.RUNNING: PlayerState.JUMPING);
        }

        private PlayerState State_Idle(PlayerInputCntrl playerInput)
        {
            PlayerState nextState = PlayerState.IDLE;

            if (playerInput == PlayerInputCntrl.MOVEMENT)
            {
                nextState = PlayerState.RUNNING;
            }
            else
            {
                animator.SetBool("run", false);
            }

            return (nextState);
        }

        private PlayerState State_Running(PlayerInputCntrl playerInputCntrl)
        {
            PlayerState nextState = PlayerState.RUNNING;

            switch (playerInputCntrl)
            {
                case PlayerInputCntrl.MOVEMENT:
                    nextState = PlayerState.RUNNING;
                    break;
                case PlayerInputCntrl.JUMP:
                    nextState = PlayerState.JUMP;
                    break;
                default:
                    nextState = PlayerState.IDLE;
                    break;
            }

            return (nextState);
        }

        #endregion

        #region *** Finite State Machine Support Functions ***

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
            animator.SetBool("run", true);

            moveDirection.y = ySpeed;

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
                playerInputCntrl = PlayerInputCntrl.JUMP;
            }
        }

        #endregion
    }

    public enum PlayerState
    {
        IDLE,
        RUNNING,
        JUMPING,
        JUMP
    }

    public enum PlayerInputCntrl
    {
        IDLE,
        MOVEMENT,
        JUMP,
        DO_NOTHING
    }
}


