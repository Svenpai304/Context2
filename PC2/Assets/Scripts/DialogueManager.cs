using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public bool dialogueActive = false;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject dialogueObj;
    [SerializeField] private float charTime;

    [SerializeField] private GameObject[] textboxes;

    private Line[] lines;
    private int index;

    private void Awake()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = this;
    }

    public void Confirm(InputAction.CallbackContext c)
    {
        if (c.started && dialogueActive)
        {
            if (text.text == lines[index].text)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index].text;
            }
        }
    }

    public void StartDialogue(Line[] _lines)
    {
        PlayerAbilityManager.instance.DisableAll();
        PlayerAbilityManager.instance.inDialogue = true;
        lines = _lines;
        index = 0;
        dialogueActive = true;
        StopAllCoroutines();
        dialogueObj.SetActive(true);
        text.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        dialogueObj.SetActive(false);
        dialogueActive = false;
        PlayerAbilityManager.instance.EnableAll();
        PlayerAbilityManager.instance.inDialogue = false;
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char you in lines[index].text.ToCharArray())
        {
            text.text += you;
            yield return new WaitForSeconds(charTime);
        }
    }

    private void SetTextbox(int index)
    {

    }
}

[Serializable]
public class Line
{
    public string text;
    public int textboxID;
}
