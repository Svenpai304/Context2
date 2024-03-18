using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpUnlocker : MonoBehaviour
{
    [SerializeField] private int maxJumps;
    public void Activate()
    {
        var pj = FindObjectOfType<PlayerJump>();
        if(pj.midAirJumps < maxJumps ) { pj.midAirJumps = maxJumps; }
    }
}
