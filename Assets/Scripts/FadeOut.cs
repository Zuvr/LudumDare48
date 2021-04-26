using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = spriteRenderer.color;
        //Debug.Log(color);
        color = new Color(color.r, color.g, color.b, color.a + Time.deltaTime/10);
        spriteRenderer.color = color;
        if (color.a >= 1)
        {
            GameObject.FindObjectOfType<PlankSpawner>().endStateSpawnControll = 0;
        }
    }
}
