using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private Vector2 interactBox;
    [SerializeField] private LayerMask interactMask;

    private IInteractive currentInteractive;

    private void Update()
    {
        IInteractive newInteractive = LookForInteractives();
        if (currentInteractive != newInteractive)
        {
            if(currentInteractive != null)
            {
                currentInteractive.Unhighlight();
            }
            currentInteractive = newInteractive;
            currentInteractive.Highlight();
        }
    }

    public void Interact()
    {
        if (currentInteractive == null) { return; }
        currentInteractive.Interact();
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
}
