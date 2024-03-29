using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclist : MonoBehaviour
{
    public AudioSource sound;
    public CircleCollider2D coll;

    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public float speed;
    public bool flipped;
    public int ringBell;
    // Start is called before the first frame update
    void Start()
    {
        ringBell = Random.Range(1, 5);
        if (coll != null)
        {
            if (ringBell != 1)
            {
                coll.enabled = false;
            }
        }
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
        if (Mathf.Abs(transform.position.x) > 400)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            sound.Play();
            coll.enabled = false;
        }
    }
}
