using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextboxAnimator : MonoBehaviour
{
    private Image image;
    public Sprite[] sprites;
    private int currentIndex = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void NextSprite()
    {
        currentIndex++;
        if(currentIndex >= sprites.Length)
        {
            currentIndex = 0;
        }
        image.sprite = sprites[currentIndex];
    }

    public void SetSprite(int index)
    {
        image.sprite = sprites[index];
    }
}
