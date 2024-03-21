using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Encyclopedia : MonoBehaviour
{
    [SerializeField] private List<EncyclopediaEntry> entries;
    [SerializeField] private TMP_Text title, info1, info2, source;
    [SerializeField] private Image img;
    [SerializeField] private GameObject popUp;
    [SerializeField] private TMP_Text popUpText;
    [SerializeField] private float popUpTime;
    private void Awake()
    {
        foreach (var entry in entries)
        {
            entry.button.SetActive(false);
        }
        title.text = string.Empty;
        info1.text = string.Empty;
        info2.text = string.Empty;
        source.text = string.Empty;

    }

    public void SetActiveEntry(int index)
    {
        if (index >= entries.Count) { return; }
        title.text = entries[index].title;
        info1.text = entries[index].info1;
        info2.text = entries[index].info2;
        source.text = entries[index].source;
        if (entries[index].img != null)
        {
            img.sprite = entries[index].img;
        }
    }

    public void SetButtonActive(int index)
    {
        if (index >= entries.Count || index < 0) { return; }
        if (entries[index].button.activeSelf) { return; }

        entries[index].button.SetActive(true);
        StartCoroutine(PopUpCoroutine(index));
    }

    private IEnumerator PopUpCoroutine(int index)
    {
        if (popUp == null) { yield break; }

        popUpText.text = entries[index].title;
        popUp.SetActive(true);
        yield return new WaitForSeconds(popUpTime);
        popUp.SetActive(false);
    }
}

[Serializable]
public class EncyclopediaEntry
{
    public string title, info1, info2, source;
    public Sprite img;
    public GameObject button;
}
