using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptThatYouNeedToCallMultipleTimesBeforeItDoesSomething : MonoBehaviour
{
    [SerializeField] private int threshold;
    [SerializeField] private UnityEvent OnTrigger;
    private List<int> previousActivators = new();

    private int counter = 0;
    public void Activate(int id)
    {
        if(previousActivators.Contains(id)) { return; }

        counter++;
        previousActivators.Add(id);
        if(counter == threshold)
        {
            OnTrigger?.Invoke();
        }
    }
   
}
