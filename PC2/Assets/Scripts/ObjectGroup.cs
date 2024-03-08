using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGroup : MonoBehaviour
{
    public List<GameObject> objects;

    public void SetActiveAll(bool state)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(state);
        }
    }

    public void ToggleActive()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
