using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    // Character Movement Controls
    //----------------------------
    private Vector2 playerMovement;
    private Vector3 moveDirection;
    private float playerSpeed = 8.0f;
    private float playerRotation = 400.0f;
    private CharacterController charCntrl;

    // Animation Controls
    //-------------------
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        charCntrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = playerMovement.x; // Horizontal Axis Controls
        moveDirection.y = 0.0f;
        moveDirection.z = playerMovement.y; // Vertical Axis Controls

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("run", true);

            moveDirection.Normalize();

            float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);

            Vector3 velocity = inputMagnitude * playerSpeed * moveDirection;

            //navMeshAgent.Move(velocity * Time.deltaTime);
            charCntrl.Move(velocity * Time.deltaTime);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotation * Time.deltaTime);
        }
        else
        {
            animator.SetBool("run", false);
        }
    }

    #region *** Player Movement Functions ***

    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log($"OnMove: {context.ReadValue<Vector2>()}");

        playerMovement = context.ReadValue<Vector2>();
    }

    #endregion
}
