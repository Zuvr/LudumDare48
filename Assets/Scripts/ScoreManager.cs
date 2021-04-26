using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    PlankSpawner plankSpawner;

    // Start is called before the first frame update
    void Start()
    {
        plankSpawner = GameObject.FindObjectOfType<PlankSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        int hundreds = plankSpawner.planksKilled / 100;
        int tens = (plankSpawner.planksKilled % 100) / 10;
        int ones = (plankSpawner.planksKilled % 10);
        gameObject.GetComponent<Text>().text = "Score: " + hundreds + tens + ones;
    }
}
