using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    private Vector2 leftControl, rightControl;

    private float speed = 10.0f;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //MovePlayer(Time.deltaTime);
    }

    private void MovePlayer(float dt)
    {
        if (leftControl.magnitude > 0.15f)
        {
            Vector3 direction = new Vector3(leftControl.x, 0.0f, leftControl.y).normalized;
            transform.Translate(speed * direction * Time.deltaTime, Space.World);
            Quaternion rotTarget = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotTarget, 500.0f * Time.deltaTime);
        }
    }

    private void ClickToMovePlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            direction = (hit.point - transform.position).normalized;
            transform.Translate(speed * direction * Time.deltaTime, Space.World);
            Quaternion rotTarget = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotTarget, 500.0f * Time.deltaTime);
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
            ClickToMovePlayer();
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
}
