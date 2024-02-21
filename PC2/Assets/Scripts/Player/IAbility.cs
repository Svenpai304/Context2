using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IAbility
{
    public void Move(InputAction.CallbackContext c);
    public void Jump(InputAction.CallbackContext c);
    public void Interact(InputAction.CallbackContext c);

    public void Disable();
    public void Enable();
}
