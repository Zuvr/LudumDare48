using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankSound : MonoBehaviour
{
    public AudioSource hit;
    public AudioSource hitBig;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.magnitude > 2)
        {
            hitBig.volume = 0.25f + 0.5f * Random.value;
            hitBig.pitch  = 0.75f + 0.5f * Random.value;
            hitBig.Play();
        }
        if(rb.velocity.magnitude > 1)
        {
            hit.volume = 0.1f + 0.4f * Random.value;
            hit.pitch  = 0.75f + 0.5f * Random.value;
            hit.Play();
        }

    }
}
