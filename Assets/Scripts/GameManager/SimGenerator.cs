using UnityEngine;
using System.Collections;

public class SimGenerator : MonoBehaviour {

    GameObject simPrefab;
    GameObject simObj;
    SimStats simStatsScript;

    Vector3 startPos;
    float randomX;
    float randomY;

    //color stuff
    Renderer rend;
    Color simColor;
    

    // Use this for initialization
    void Start ()
    {
        CreateSims(SimStats.SimJobs.Production, "Geoffrey");
        CreateSims(SimStats.SimJobs.Sales, "Charles Barkley");
        CreateSims(SimStats.SimJobs.Sales, "Sales Guy");
    }
	

    void CreateSims(SimStats.SimJobs job, string name)
    {
        randomX = Random.Range(3f, 15f);
        randomY = Random.Range(3f, 15f);
        startPos = new Vector3(randomX, randomY, -1);

        simPrefab = (GameObject)Resources.Load("Prefabs/sim_MAIN");
        simObj = Instantiate(simPrefab, startPos, Quaternion.identity) as GameObject;

        rend = simObj.GetComponent<Renderer>();


        //set job state
        simStatsScript = simObj.GetComponent<SimStats>();
        simStatsScript.jobState = job;

        //set random name
        simStatsScript.simName = name;

        //set color
        simColor.r = Random.Range(0f, 1f);
        simColor.g = Random.Range(0f, 1f);
        simColor.b = Random.Range(0f, 1f);
        simColor.a = 1f;
        rend.material.color = simColor;
    }
}
