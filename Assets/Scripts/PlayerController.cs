using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource jump;
    public AudioSource walk;
    bool step = false;


    Rigidbody2D rb;
    public float closeToBottomPlayerModifier = 0;
    public float timeSinceLastJump = Mathf.Infinity;
    
    //for Animations
    Animator anim;
    bool isMoving = false;
    bool isOnGround = false;
    bool isFacingRight = false; //Done
    bool isCloseToPlank = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        anim.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Animation
        if (isFacingRight)
        {
            transform.localScale = new Vector2(-0.5f, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(0.5f, transform.localScale.y);
        }

        if(isCloseToPlank || isOnGround)
        {
            anim.Play(0, 0, 0);
            if (isMoving)
            {
                if (Time.realtimeSinceStartup % 0.4f > 0.2f)
                {
                    if (!step)
                    {
                        walk.pitch = 0.8f + 0.4f * Random.value;
                        walk.Play();
                        step = true;
                    }
                }
                else
                {
                    if (step)
                    {
                        walk.pitch = 0.8f + 0.4f * Random.value;
                        walk.Play();
                        step = false;
                    }
                }
                


                if(Time.realtimeSinceStartup % 0.2 > 0.1)
                {
                    anim.Play(0,0,0.5f);
                }

                
            }
            else if (Time.realtimeSinceStartup % 1 > 0.5)
            {
                anim.Play(0,0,1f);
            }
        }
        else
        {
            anim.Play(0,0,1f);
        }




        //Movement
        float desiredVelocityX = Input.GetAxisRaw("Horizontal") * 2f;
        float desiredVelocityY = Input.GetAxisRaw("Vertical") * 1.5f * (1 - closeToBottomPlayerModifier);
        
        
        if(desiredVelocityX !=0 || desiredVelocityY != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }


        Vector2 desiredVelocity = new Vector2(desiredVelocityX, desiredVelocityY);
        if (timeSinceLastJump > 0.5)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, desiredVelocity, Time.deltaTime * 10f);
        }


        //Find Local Direction
        Vector2 localMovementDirection = transform.InverseTransformDirection(desiredVelocity);
        if( localMovementDirection.x > 0)
        {
            isFacingRight = true;
        }
        if( localMovementDirection.x < 0)
        {
            isFacingRight = false;
        }

        //Find and rotate to closest Plank
        GameObject closestPlank = GetClosestPlank();
        float rotationSpeedModifier = 1;

        //If in endstate, find ground (or princess)
        if (rb.gravityScale == 1)
        {
            rb.rotation = 0;
            GameObject princess = GameObject.Find("Princess");
            float distanceToPrincess = Mathf.Infinity;
            if (princess != null)
            {
                distanceToPrincess = princess.transform.position.x - transform.position.x;
            }

            if (distanceToPrincess < 0.25f && distanceToPrincess > 0.1f)
            {
                rb.rotation = 45;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jump.Play();

                    princess.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 3.5f);
                    princess.GetComponent<Rigidbody2D>().angularVelocity = -180f;
                    princess.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    Object[] sprites;
                    sprites = Resources.LoadAll("Sprites/Princess");
                    princess.GetComponent<SpriteRenderer>().sprite = (Sprite)sprites[2];
                    GameObject.FindObjectOfType<PlankSpawner>().endStateSpawnControll = -5;
                    Instantiate(Resources.Load("Prefabs/FadeOut", typeof(GameObject)) as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
            //If close to floor can jump
            if (transform.position.y < -0.6)
            {
                isOnGround = true;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jump.Play();
                    rb.velocity = new Vector2(0, 6);
                }
            }
            else
            {
                isOnGround = false;
            }
            if(transform.position.y < -1)
            {
                transform.position = new Vector2(0, -0.7f);
            }
        }


        if (closestPlank != null && rb.gravityScale == 0)
        {
            Vector2 closestPlankDirection = (closestPlank.transform.position - transform.position).normalized;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, closestPlankDirection);
            RaycastHit2D hitPlank = new RaycastHit2D();
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.transform.parent == GameObject.Find("PlankSpawner").transform)
                {
                    //Debug.DrawRay(hit.point, hit.normal, Color.yellow);
                    hitPlank = hit;
                    break;
                }
            }
            float desiredAngle = Mathf.Atan2(hitPlank.normal.y, hitPlank.normal.x) * Mathf.Rad2Deg;
            float desiredAngularMovement = desiredAngle - rb.rotation - 90;
            if (desiredAngularMovement > 180)
            {
                desiredAngularMovement -= 360;
            }
            if (desiredAngularMovement < -180)
            {
                desiredAngularMovement += 360;
            }
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, desiredAngularMovement * 4 * rotationSpeedModifier, Time.deltaTime * 100f);



            //Jumping
            RaycastHit2D[] hitsLegs = Physics2D.RaycastAll(transform.position, (Vector2)(Quaternion.Euler(0, 0, rb.rotation - 90) * Vector2.right));
            RaycastHit2D hitLegs = new RaycastHit2D();
            foreach (RaycastHit2D hit in hitsLegs) //Find object to jump from
            {
                if (hit.collider.gameObject.transform.parent == GameObject.Find("PlankSpawner").transform)
                {
                    hitLegs = hit;
                    break;
                }
            }
            //Debug.Log(desiredAngularMovement);
            if (hitLegs.distance < 0.3f && Mathf.Abs(desiredAngularMovement) < 15)
            {
                isCloseToPlank = true;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jump.Play();

                    //rb.AddForce((transform.position - hitLegs.transform.gameObject.transform.position)*10);
                    rb.velocity = ((Vector2)transform.position - hitLegs.point) * 75;
                    //hitLegs.rigidbody.velocity = ((Vector2)hitLegs.rigidbody.gameObject.transform.position - hitLegs.point * 4);
                    timeSinceLastJump = 0;
                    //Debug.LogWarning("PUSHED!");
                }
            }
            else
            {
                isCloseToPlank = false;
            }
        }
        timeSinceLastJump += Time.deltaTime;


        if ((transform.position.x > 2 || transform.position.x < -2) && rb.gravityScale == 0)
        {
            transform.position = new Vector2(0, 2);
        }

    }

   GameObject GetClosestPlank()
    {
        PlankSpawner plankSpawner = GameObject.Find("PlankSpawner").GetComponent<PlankSpawner>();
        GameObject closestPlank = null;
        if (plankSpawner.brokenPlanks != null)
        {
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (GameObject brokenPlank in plankSpawner.brokenPlanks)
            {
                Vector2 directionToPlank = brokenPlank.transform.position - currentPosition;
                float distanceSqrToPlank = directionToPlank.sqrMagnitude;
                if (distanceSqrToPlank < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqrToPlank;
                    closestPlank = brokenPlank;
                }
            }
        }
        return closestPlank;
    }
}
