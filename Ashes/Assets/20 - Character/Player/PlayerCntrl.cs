using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    private Vector2 leftControl, rightControl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer(Time.deltaTime);
    }

    private void MovePlayer(float dt)
    {
        if (leftControl != Vector2.zero)
        {

        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            leftControl = context.ReadValue<Vector2>();
            Debug.Log($"Left Controller (Started): {leftControl}");
        }

        if (context.canceled)
        {
            leftControl = Vector2.zero;
            Debug.Log($"Left Controller (Canceled): {leftControl}");
        }
    }
}
