using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideUnlocker : MonoBehaviour
{
    public void Activate()
    {
        var pj = FindObjectOfType<PlayerJump>();
        pj.glideEnabled = true;
    }
}
