using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractive : MonoBehaviour, IInteractive
{
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private string[] lines;
    public Vector2 position { get => transform.position; }

    public void Highlight()
    {
        image.color = Color.green;
    }

    public void Interact()
    {
        if (lines != null)
        {
            DialogueManager.instance.StartDialogue(lines);
        }
    }

    public void Unhighlight()
    {
        image.color = Color.red;
    }
}
