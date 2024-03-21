using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Encyclopedia : MonoBehaviour
{
    [SerializeField] private List<EncyclopediaEntry> entries;
    [SerializeField] private TMP_Text title, info1, info2, source;
    [SerializeField] private Image img;
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
        entries[index].button.SetActive(true);
    }
}

[Serializable]
public class EncyclopediaEntry
{
    public string title, info1, info2, source;
    public Sprite img;
    public GameObject button;
}
