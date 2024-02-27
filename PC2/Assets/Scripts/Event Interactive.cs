using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInteractive : MonoBehaviour, IInteractive
{
    public Vector2 position { get => transform.position; }
    public SpriteRenderer sprite;
    public UnityEvent OnInteractive;

    public void Highlight()
    {
        sprite.color = Color.blue;
    }

    public void Interact()
    {
        OnInteractive?.Invoke();
    }

    public void Unhighlight()
    {
        sprite.color = Color.white;
    }
}
