using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamespeedManager : MonoBehaviour
{
    GameObject player;
    GameObject walls;
    GameObject plankSpawner;
    Rigidbody2D playerRigidBody;

    [Range(2f, 10f)]
    public float currentGlobalVelocity = 2;
    private float prevGlobalVelocity = 2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        walls = GameObject.Find("Walls");
        plankSpawner = GameObject.Find("PlankSpawner");
        playerRigidBody = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //Take Player up
        float closeToBottomPlayer = player.transform.position.y *-1;
        player.GetComponent<PlayerController>().closeToBottomPlayerModifier = Mathf.Lerp(0,1, (closeToBottomPlayer-0.5f) * 4);
        if (playerRigidBody.gravityScale == 0)
        {
            playerRigidBody.velocity += new Vector2(0, closeToBottomPlayer * 0.1f);
        }
        

        //Change current velocity based on player location
        float closeToBottom = Mathf.Lerp(0, 0.1f, ((player.transform.position.y * -1f) - 0.7f));
        float closeToTop = Mathf.Lerp(0, 0.1f, (player.transform.position.y - 0.6f));
        currentGlobalVelocity += closeToBottom - closeToTop;

        //Change current velocity based on jump
        //currentGlobalVelocity += playerRigidBody.velocity.y *-1 * Mathf.Lerp(1, 0, player.GetComponent<PlayerController>().timeSinceLastJump);

        currentGlobalVelocity = currentGlobalVelocity < 2 ? 2 : currentGlobalVelocity;

        //Speed walls up
        walls.GetComponent<WallScroll>().SidewallsScrollspeed = 1f  * currentGlobalVelocity;
        walls.GetComponent<WallScroll>().BackwallsScrollspeed = 0.5f* currentGlobalVelocity;



        
        //Speed Planks up
        foreach(Transform plank in plankSpawner.transform)
        {
            plank.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, currentGlobalVelocity-prevGlobalVelocity);
        }
        prevGlobalVelocity = currentGlobalVelocity;

    }
}
