using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInteractive : MonoBehaviour, IInteractive
{
    public Vector2 position { get => transform.position; }
    public Vector2 highlightOffset;
    public SpriteRenderer sprite;
    public UnityEvent OnInteractive;

    private GameObject currentHighlight;

    public void Highlight()
    {
        currentHighlight = Instantiate(DialogueManager.instance.highlightPrefab, transform);
        currentHighlight.transform.Translate(highlightOffset);
    }

    public void Interact()
    {
        if (currentHighlight != null)
        {
            Destroy(currentHighlight);
        }
        OnInteractive?.Invoke();
    }

    public void Unhighlight()
    {
        if (currentHighlight != null)
        {
            Destroy(currentHighlight);
        }
    }
}
