using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    private Animator animator;

    private Vector2 leftControl;

    private float speed = 7.0f;
    private Vector3 direction;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        animator.SetFloat("speed", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //JoyStickToMovePlayer(Time.deltaTime);
        ClickToMovePlayer();
    }

    private void Fire1()
    {

    }

    private void JoyStickToMovePlayer(float dt)
    {
        if (leftControl.magnitude > 0.15f)
        {
            Vector3 direction = new Vector3(leftControl.x, 0.0f, leftControl.y).normalized;
            transform.Translate(speed * direction * Time.deltaTime, Space.World);
            Quaternion rotTarget = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotTarget, 500.0f * Time.deltaTime);
            animator.SetFloat("speed", 1.0f);
        } else
        {
            animator.SetFloat("speed", 0.0f);
        }
    }

    private void ClickToMovePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                target = hit.point;
                direction = (target - transform.position).normalized;
                transform.Translate(speed * direction * Time.deltaTime, Space.World);
                Quaternion rotTarget = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotTarget, 500.0f * Time.deltaTime);
                animator.SetFloat("speed", 1.0f);
            }
        } else
        {
            if (Vector3.Distance(target, transform.position) > 0.1f)
            {
                transform.Translate(speed * Time.deltaTime * direction, Space.World);
                Quaternion rotTarget = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotTarget, 500.0f * Time.deltaTime);
            } else
            {
                animator.SetFloat("speed", 0.0f);
            }
        }
    }

    /*************************/
    /*** Controller Inputs ***/
    /*************************/

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            leftControl = context.ReadValue<Vector2>();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log($"Mouse Input: {context.ReadValue<Vector2>()}");
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"OnClick ...");
            //leftMouseButtonClicked = true;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"OnFire Started ...");
        }

        if (context.canceled)
        {
            Debug.Log($"OnFire Ended ...");
        }
    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Fire1();
        }

        if (context.canceled)
        {
            Debug.Log($"OnFire 1 Canceled ...");
        }
    }
}
