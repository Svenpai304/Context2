using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLookDown : MonoBehaviour, IAbility
{

    [SerializeField] private float threshold;
    [SerializeField] private float additionalY;
    [SerializeField] private bool lookingDown;
    private CameraMovement cam;

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    public void Move(InputAction.CallbackContext c)
    {
        Vector2 move = c.ReadValue<Vector2>();
        if(move.y <= threshold)
        {
            LookDown();
        }
        else
        {
            StopLookDown();
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
        throw new System.NotImplementedException();
    }

    public void Enable()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(InputAction.CallbackContext c)
    {
    }

    public void Jump(InputAction.CallbackContext c)
    {
    }
}
