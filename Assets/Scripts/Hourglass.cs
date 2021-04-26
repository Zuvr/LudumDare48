using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hourglass : MonoBehaviour
{
    public int currentStage = 0;
    public int currentSprite = 0;
    float animationJumpFrequency = 1;

    GameObject player;
    Rigidbody2D playerRigidBody;
    WallScroll wallScroll;
    GamespeedManager gamespeedManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        wallScroll = GameObject.FindObjectOfType<WallScroll>();
        gamespeedManager = GameObject.FindObjectOfType<GamespeedManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.timeSinceLevelLoad;
        //Debug.Log(currentTime);
        currentStage = (int)(Mathf.InverseLerp(0, 120, currentTime) * 9);
        animationJumpFrequency = 1f / (currentStage + 1f) * 5;


        //Jump between jump and not jump :D
        if (Time.timeSinceLevelLoad % animationJumpFrequency > animationJumpFrequency / 4 * 3)
        {
            currentSprite = currentStage + 10;
        }
        else
        {
            currentSprite = currentStage;
        }


        Object[] sprites;
        sprites = Resources.LoadAll("Sprites/Hourglass");
        gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)sprites[currentSprite + 1];



        //ENDSTATE
        if (currentTime >= 120 && currentTime <= 125) //CHANGE TO 120 & 125
        {
            GameObject.FindObjectOfType<PlankSpawner>().endStateSpawnControll = 0;
        }

        if (currentTime >= 125 && wallScroll.endState == false) //CHANGE TO 125
        {
            wallScroll.endState = true;
            playerRigidBody.velocity = new Vector2(0, gamespeedManager.currentGlobalVelocity * -1);
            playerRigidBody.gravityScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
