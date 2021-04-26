using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScroll : MonoBehaviour
{
    //ENDSTATE
    public bool endState = false;
    public bool floorSpawned = false;
    public bool stopScrolling = false;



    //Sidewalls
    private GameObject SidewallsOld;
    private GameObject SidewallsCurrent;

    public float SidewallsScrollspeed = 1f;


    //Sidecolliders

    private GameObject SidecollidersOld;
    private GameObject SidecollidersCurrent;

    private Rigidbody2D SidecollidersCurrentRB;
    private Rigidbody2D SidecollidersOldRB;


    //Backwalls
    private GameObject BackwallsOld;
    private GameObject BackwallsCurrent;

    public float BackwallsScrollspeed = 0.5f;



    // Start is called before the first frame update
    void Start()
    {

        //Sidewalls
        SidewallsCurrent = gameObject.transform.Find("Sidewalls").gameObject;
        SidewallsOld = SidewallsCurrent;
        SidewallsCurrent = Instantiate(SidewallsOld, new Vector3(0, -2, 0), Quaternion.identity);
        SidewallsCurrent.name = SidewallsOld.name;
        SidewallsCurrent.transform.parent = gameObject.transform;




        //Wall Colliders
        SidecollidersCurrent = gameObject.transform.Find("Sidecolliders").gameObject;
        SidecollidersOld = SidecollidersCurrent;
        SidecollidersCurrent = Instantiate(SidecollidersOld, new Vector3(0, -2, 0), Quaternion.identity);
        SidecollidersCurrent.name = SidecollidersOld.name;
        SidecollidersCurrent.transform.parent = gameObject.transform;
        SidecollidersCurrentRB = SidecollidersCurrent.GetComponent<Rigidbody2D>();
        SidecollidersOldRB = SidecollidersOld.GetComponent<Rigidbody2D>();



        //Backwalls
        BackwallsCurrent = gameObject.transform.Find("Backwalls").gameObject;
        BackwallsOld = BackwallsCurrent;
        BackwallsCurrent = Instantiate(BackwallsOld, new Vector3(0, -2, 1), Quaternion.identity);
        BackwallsCurrent.name = BackwallsOld.name;
        BackwallsCurrent.transform.parent = gameObject.transform;

    }

    // Update is called once per frame
    void Update()
    {
        //Sidewalls
        if (!(stopScrolling)) //Stop scrolling if at bottom
        {
            SidewallsCurrent.transform.position = SidewallsCurrent.transform.position + new Vector3(0, SidewallsScrollspeed * Time.deltaTime);
            SidewallsOld.transform.position = SidewallsOld.transform.position + new Vector3(0, SidewallsScrollspeed * Time.deltaTime);
        }
        if (SidewallsOld.transform.position.y >= 2)
        {
            if (floorSpawned)
            {
                stopScrolling = true;
                return;
            }
            if (endState) //Spawn bottom
            {
                Destroy(SidewallsOld);
                SidewallsOld = SidewallsCurrent;
                SidewallsCurrent = Instantiate(Resources.Load("Prefabs/Bottom", typeof(GameObject)), SidewallsOld.transform.position + new Vector3(0, -4,0), Quaternion.identity) as GameObject;
                floorSpawned = true;
            }
            else
            {
                Destroy(SidewallsOld);
                SidewallsOld = SidewallsCurrent;
                SidewallsCurrent = Instantiate(SidewallsOld, SidewallsOld.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
                SidewallsCurrent.name = SidewallsOld.name;
                SidewallsCurrent.transform.parent = gameObject.transform;
            }
        }



        //Backwalls
        if (!(stopScrolling)) //Stop scrolling if at bottom
        {
            BackwallsCurrent.transform.position = BackwallsCurrent.transform.position + new Vector3(0, BackwallsScrollspeed * Time.deltaTime);
            BackwallsOld.transform.position = BackwallsOld.transform.position + new Vector3(0, BackwallsScrollspeed * Time.deltaTime);
        }
        if (BackwallsOld.transform.position.y >= 2)
        {
            Destroy(BackwallsOld);
            BackwallsOld = BackwallsCurrent;
            BackwallsCurrent = Instantiate(BackwallsOld, BackwallsOld.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
            BackwallsCurrent.name = BackwallsOld.name;
            BackwallsCurrent.transform.parent = gameObject.transform;
        }
    }
    private void FixedUpdate()
    {
        //Sidewalls
        SidecollidersCurrentRB.MovePosition(SidecollidersCurrent.transform.position + new Vector3(0, SidewallsScrollspeed * Time.fixedDeltaTime));
        SidecollidersOldRB.MovePosition(SidecollidersOld.transform.position + new Vector3(0, SidewallsScrollspeed * Time.fixedDeltaTime));

        if (SidecollidersOld.transform.position.y >= 2)
        {
            if (!stopScrolling)
            {
                Destroy(SidecollidersOld);
                SidecollidersOld = SidecollidersCurrent;
            
                SidecollidersCurrent = Instantiate(SidecollidersOld, SidecollidersOld.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
                SidecollidersCurrent.name = SidecollidersOld.name;
                SidecollidersCurrent.transform.parent = gameObject.transform;

                SidecollidersCurrentRB = SidecollidersCurrent.GetComponent<Rigidbody2D>();
                SidecollidersOldRB = SidecollidersOld.GetComponent<Rigidbody2D>();
            }
        }
    }
}
