using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptThatYouNeedToCallMultipleTimesBeforeItDoesSomething : MonoBehaviour
{
    [SerializeField] private int threshold;
    [SerializeField] private UnityEvent OnTrigger;

    private int counter = 0;
    public void Activate()
    {
        counter++;
        if(counter == threshold)
        {
            OnTrigger?.Invoke();
        }
    }
   
}
