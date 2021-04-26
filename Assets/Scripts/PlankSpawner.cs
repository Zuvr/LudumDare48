using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankSpawner : MonoBehaviour
{

    public float secondsPerSpawn = 2f;
    
    public List<GameObject> brokenPlanks = new List<GameObject>();
    
    private float prevSpawnTime;

    public float endStateSpawnControll = 1;

    public int planksKilled = 0;


    // Start is called before the first frame update
    void Start()
    {
        prevSpawnTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        float currentGlobalVelocity = GameObject.Find("GamespeedManager").GetComponent<GamespeedManager>().currentGlobalVelocity;
        secondsPerSpawn= 1 / currentGlobalVelocity * 3f / Mathf.Abs(endStateSpawnControll);

        if(Time.timeSinceLevelLoad - prevSpawnTime > secondsPerSpawn)
        {
            brokenPlanks.Add(Instantiate(Resources.Load(string.Format("Prefabs/BrokenPlanks/BrokenPlanks_ALL_TEMP_{0}", Random.Range(0, 3)), typeof(GameObject))) as GameObject);
            brokenPlanks[brokenPlanks.Count-1].transform.parent = gameObject.transform;
            brokenPlanks[brokenPlanks.Count-1].transform.localScale = new Vector3(Random.value * 0.5f + 0.75f, Random.value * 0.5f + 0.75f);
            brokenPlanks[brokenPlanks.Count-1].transform.position = new Vector2(Random.value * 2 - 1, -2 *(endStateSpawnControll));
            brokenPlanks[brokenPlanks.Count-1].transform.rotation = Quaternion.Euler(0, 0, Random.value*360);
            brokenPlanks[brokenPlanks.Count-1].GetComponent<Rigidbody2D>().velocity = new Vector2(Random.value*0.1f-0.05f, Random.value*0.1f+0.4f * currentGlobalVelocity * endStateSpawnControll);
            brokenPlanks[brokenPlanks.Count-1].GetComponent<Rigidbody2D>().angularVelocity = Random.value * 20 - 10;
            brokenPlanks[brokenPlanks.Count-1].GetComponent<Rigidbody2D>().gravityScale = (endStateSpawnControll - 1) / -5;
            prevSpawnTime = Time.timeSinceLevelLoad;
        }

        List<GameObject> brokenPlanksForLoop = brokenPlanks;
        for(int i = 0; i < brokenPlanks.Count; i++)
        {
            if (brokenPlanks[i].transform.position.y > 10)
            {
                Destroy(brokenPlanks[i]);
                planksKilled++;
                brokenPlanks.Remove(brokenPlanks[i]);
                i -= 1;
            }
        }
    }
}
