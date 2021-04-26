using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TorchManager : MonoBehaviour
{
    float random = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        random = Mathf.Lerp(random, Random.value, 0.1f);

        
        gameObject.GetComponent<Animator>().speed = random;
        gameObject.GetComponentInChildren<Light2D>().intensity = 0.9f + (random / 5);
    }
}
