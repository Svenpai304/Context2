using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerAbilityManager>() != null)
        {
            OnEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerAbilityManager>() != null)
        {
            OnExit?.Invoke();
        }
    }
}
