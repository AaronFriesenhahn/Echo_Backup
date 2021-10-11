using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] bool _invertVertical = false;

    public event Action<Vector3> MoveInput = delegate { };
    public event Action<Vector3> RotateInput = delegate { };
    public event Action JumpInput = delegate { };
    public event Action SprintPressed = delegate { };
    public event Action SprintReleased = delegate { };
    public event Action FireInput = delegate { };

    private void Update()
    {
        if (GameManager.GameIsPaused == false)
        {
            DetectMoveInput();
            DetectJumpInput();
            DetectSprintInput();
            DetectFireInput();
        }
    }

    void DetectMoveInput()
    {
        //process input as a 0 or 1 value, if we have it
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        //if we have either horizontal or vertical input
        if (xInput != 0 || yInput != 0)
        {
            //convert to local directions, based on player orientation
            Vector3 _horizontalMovement = transform.right * xInput;
            //Vector3 _forwardMovement = transform.forward * yInput;
            //combine movements into a single vector
            //Vector3 movement = (_horizontalMovement + _forwardMovement).normalized;
            //only using 2d movement
            Vector3 movement = (_horizontalMovement).normalized;

            //notify that we have moved
            MoveInput?.Invoke(movement);
        }
    }

    void DetectJumpInput()
    {
        //Spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput?.Invoke();
        }
    }

    void DetectSprintInput()
    {
        //leftshift pressed
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            SprintPressed?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            SprintReleased?.Invoke();
        }
    }

    void DetectFireInput()
    {
        //left mouse clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireInput?.Invoke();
        }
    }
}
