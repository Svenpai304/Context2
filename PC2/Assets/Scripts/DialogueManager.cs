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
    public GameObject highlightPrefab;
    public GameObject alertPrefab;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject dialogueObj;
    [SerializeField] private GameObject textboxParent;
    [SerializeField] private float charTime;
    [SerializeField] private float animTime;

    [SerializeField] private GameObject[] textboxes;


    private Line[] lines;
    private int index;

    private TextboxAnimator textboxAnimator;

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
                if (textboxAnimator != null)
                {
                    textboxAnimator.SetSprite(0);
                }
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
        root.SetActive(true);
        text.text = string.Empty;
        SetTextbox(lines[index].textboxID);
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        root.SetActive(false);
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
            SetTextbox(lines[index].textboxID);
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        StartCoroutine(AnimateTextbox());
        foreach (char you in lines[index].text.ToCharArray())
        {
            text.text += you;
            yield return new WaitForSeconds(charTime);
        }
        StopAllCoroutines();
    }

    IEnumerator AnimateTextbox()
    {
        Debug.Log("Animating textbox");
        while (textboxAnimator != null)
        {
            textboxAnimator.NextSprite();
            yield return new WaitForSeconds(animTime);
        }
    }

    private void SetTextbox(int index)
    {
        if (index >= lines.Length) { index = 0; }
        if (textboxes[index].activeSelf) { return; }

        foreach (GameObject go in textboxes)
        {
            go.SetActive(false);
        }
        textboxAnimator = textboxes[index].GetComponent<TextboxAnimator>();
        textboxes[index].SetActive(true);
        Debug.Log("Set textbox animator");
    }
}

[Serializable]
public class Line
{
    public string text;
    public int textboxID;
}
