using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour, IAbility
{

    [SerializeField] private Vector2 interactBox;
    [SerializeField] private LayerMask interactMask;

    private IInteractive currentInteractive;

    private void Update()
    {
        IInteractive newInteractive = LookForInteractives();
        if (currentInteractive != newInteractive)
        {
            if (currentInteractive != null)
            {
                currentInteractive.Unhighlight();
            }
            currentInteractive = newInteractive;
            if (currentInteractive != null)
            {
                currentInteractive.Highlight();
            }
        }
    }

    public void Interact(InputAction.CallbackContext c)
    {
        if (c.started)
        {
            if (currentInteractive == null) { return; }
            currentInteractive.Interact();
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

    private IInteractive LookForInteractives()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, interactBox, interactMask);
        IInteractive closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in colliders)
        {
            IInteractive interactive = collider.gameObject.GetComponent<IInteractive>();
            if (interactive != null)
            {
                float sqrDistance = (interactive.position - (Vector2)transform.position).sqrMagnitude;
                if (sqrDistance < closestDistance)
                {
                    closest = interactive;
                }
            }
        }
        return closest;
    }

    #region unused interface bits
    public void Move(InputAction.CallbackContext c)
    {
        //not used
    }

    public void Jump(InputAction.CallbackContext c)
    {
        //not used
    }
    #endregion
}
