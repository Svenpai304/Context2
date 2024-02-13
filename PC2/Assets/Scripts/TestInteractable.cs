using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInteractable : MonoBehaviour, IInteractive
{
    [SerializeField] private SpriteRenderer image;
    public Vector2 position { get => transform.position; }

    public void Highlight()
    {
        image.color = Color.yellow;
    }

    public void Interact()
    {
        
    }

    public void Unhighlight()
    {
        image.color = Color.red;
    }
}
