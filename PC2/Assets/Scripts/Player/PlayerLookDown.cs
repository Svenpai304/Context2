using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLookDown : MonoBehaviour, IAbility
{

    [SerializeField] private float holdTime;
    [SerializeField] private float inputThreshold;
    [SerializeField] private float additionalY;

    private bool holdingDown;
    private float heldTime;
    private bool lookingDown;
    private CameraMovement cam;

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    private void FixedUpdate()
    {
        if(holdingDown)
        {
            heldTime += Time.deltaTime;
        }
        else
        {
            heldTime = 0;
        }
        if(heldTime >= holdTime)
        {
            LookDown();
        }
        else
        {
            StopLookDown();
        }
    }

    public void Move(InputAction.CallbackContext c)
    {
        Vector2 move = c.ReadValue<Vector2>();
        if(move.y <= inputThreshold)
        {
            holdingDown = true;
        }
        else
        {
            holdingDown = false;
        }
    }

    private void LookDown()
    {
        if (!lookingDown)
        {
            lookingDown = true;
            Debug.Log("Looking down");
            cam.additionalY += additionalY;
        }
    }

    private void StopLookDown()
    {
        if (lookingDown)
        {
            lookingDown = false;
            cam.additionalY -= additionalY;
        }
    }

    public void Disable()
    {
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Interact(InputAction.CallbackContext c)
    {
    }

    public void Jump(InputAction.CallbackContext c)
    {
    }
}
