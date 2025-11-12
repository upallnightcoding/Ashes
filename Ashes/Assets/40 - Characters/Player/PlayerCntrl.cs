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
        [SerializeField] private float gravityMultiplier = 3.0f;
        private Vector2 playerMovement;
        private Vector3 moveDirection;
        private CharacterController charCntrl;
        private float jumpHeight = 2.0f;

        // Animation Controls
        //-------------------
        private Animator animator;

        private PlayerState currentState = PlayerState.IDLE;

        private PlayerInputCntrl playerInputCntrl = PlayerInputCntrl.DO_NOTHING;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            charCntrl = GetComponent<CharacterController>();
        }

        void Update()
        {
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
                    currentState = State_Jumping(moveDirection);
                    break;
            }
        }

        #region *** Finite State Machine ***

        private PlayerState State_Jump()
        {
            jumpHeight = 3.0f;

            moveDirection.y += jumpHeight;

            MovePlayer(moveDirection);

            return (PlayerState.JUMPING);
        }

        private PlayerState State_Jumping(Vector3 moveDirection)
        {
            MovePlayer(moveDirection);

            moveDirection.y += GRAVITY * Time.deltaTime;

            Debug.Log($"moveDirection.y: {moveDirection.y}");

            return (moveDirection.y > 0.0f ? PlayerState.JUMPING : PlayerState.RUNNING);
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
                    RotatePlayer(moveDirection);
                    MovePlayer(moveDirection);
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
        private void RotatePlayer(Vector3 moveDirection)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotation * Time.deltaTime);
        }

        /**
         * MovePlayer() - 
         */
        private void MovePlayer(Vector3 moveDirection)
        {
            animator.SetBool("run", true);

            charCntrl.Move(playerSpeed * moveDirection * Time.deltaTime);
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
                playerInputCntrl = PlayerInputCntrl.DO_NOTHING;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log($"OnJump ...");

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
        DO_NOTHING,
        MOVEMENT,
        JUMP
    }
}


