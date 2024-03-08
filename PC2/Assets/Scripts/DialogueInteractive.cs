using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractive : MonoBehaviour
{
    public Line[] lines;

    public void Interact()
    {
        if (lines != null)
        {
            DialogueManager.instance.StartDialogue(lines);
        }
    }
}
