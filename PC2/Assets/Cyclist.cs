using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclist : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public float speed;
    public bool flipped;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed, 0);
        if (speed < 0 && !flipped)
        {
            flipped = true;
            sprite.flipX = !sprite.flipX;
        }
        if (Mathf.Abs(transform.position.x) > 5000)
        {
            Destroy(gameObject);
        }
    }
}
