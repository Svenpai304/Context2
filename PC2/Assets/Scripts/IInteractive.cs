using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
    public void Interact();
    public void Highlight();
    public void Unhighlight();
    public Vector2 position { get; }
}
