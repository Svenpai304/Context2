using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private bool doesLockCamera;
    [SerializeField] private bool onTriggerEnter;
    [SerializeField] private Transform destination;
    [SerializeField] private Transform camDestination;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!onTriggerEnter) { return; }
        PlayerMovement pm = other.GetComponent<PlayerMovement>();

        if (pm != null)
        {
            Teleport();
        }
    }

    public void Teleport()
    {
        CameraMovement cm = Camera.main.GetComponent<CameraMovement>();
        if (doesLockCamera)
        {
            cm.Lock(new Vector3(camDestination.position.x, camDestination.position.y, cm.transform.position.z));
        }
        else
        {
            cm.Unlock(new Vector3(camDestination.position.x, camDestination.position.y, cm.transform.position.z));
        }
        cm.target.position = destination.position;
    }
}
