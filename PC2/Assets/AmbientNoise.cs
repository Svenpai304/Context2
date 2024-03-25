using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoise : MonoBehaviour
{
    public AudioSource sound;
    public Transform parentTransform;

    public float startHeight;
    public float endHeight;

    public float percentage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (parentTransform.position.y > startHeight)
        {
            percentage = .5f / endHeight * parentTransform.position.y;
            //bar.fillAmount = Mathf.Clamp(1f - percentage, 0, 1);
            sound.volume = Mathf.Clamp(.5f - percentage, 0, 1);
        }
        else if (parentTransform.position.y < -30)
        {
            sound.volume = 0f;
        }
        else
        {
            sound.volume = .5f;
        }
    }
}
//Make audio fade out when going up higher. 0 at 40. Start at 15