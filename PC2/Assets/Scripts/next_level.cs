using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class next_level : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == player) Debug.Log("next level");
        Debug.Log("trigger");
    }
}
