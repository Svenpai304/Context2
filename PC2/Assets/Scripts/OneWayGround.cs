using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayGround : MonoBehaviour
{
    [SerializeField] private Collider2D ground;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerAbilityManager>() != null)
        {
            ground.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerAbilityManager>() != null)
        {
            ground.enabled = true;
        }
    }
}
