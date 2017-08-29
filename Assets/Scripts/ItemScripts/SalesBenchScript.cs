using UnityEngine;
using System.Collections;

public class SalesBenchScript : MonoBehaviour {

    public Vector2 salesBenchPos;
    public Vector2 salesBenchUsePos;

    //PRODUCTION PROGRESS STUFF
    [Range(0, 100)]
    public int progressCount;
    public bool inProgress;
    bool isProgressCoroutineStarted;
    Coroutine progressCoroutine = null;

    //WIDGETS INTERACTION
    

    //SIM INTERACTING WITH THIS BENCH
    GameObject sim;
    SimAI simAIScript;

    // Use this for initialization
    void Start () {
        GameStats.hasSalesBench = true;
        salesBenchPos = gameObject.transform.position;

        //ADD THIS ITEM TO THE WIDGETBENCH ARRAY IN GAMESTATS
        GameStats.countSalesBench++;
        GameStats.salesBenchList.Add(this.gameObject);

        progressCount = 0;

    }
	
	// Update is called once per frame
	void Update () {
        //Update sales bench position 
        salesBenchPos = gameObject.transform.position;

        if (inProgress)
        {
            SalesInProgress();
        }

    }

    void SalesInProgress()
    {
        if (!isProgressCoroutineStarted)
        {
            progressCoroutine = StartCoroutine(ProgressIncrement(.5f));
        }

        if (progressCount >= 100)
        {
            //stop coroutine and set inProgress to false
            StopCoroutine(progressCoroutine);
            inProgress = false;
            //simAIScript.isWidgetBenchInProgress = false;

            //have chance to sell a widget

            //set progressCount back to 0
            progressCount = 0;
        }
    }

    IEnumerator ProgressIncrement(float waitTime)
    {
        isProgressCoroutineStarted = true;

        while (inProgress)
        {
            progressCount += 1;
            yield return new WaitForSeconds(waitTime);

        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            simAIScript = col.gameObject.GetComponent<SimAI>();
            if (simAIScript.simFSMScript.mainState == SimFSM.MainFSM.Task && simAIScript.simFSMScript.taskState == SimFSM.TaskFSM.Sales)
            {
                inProgress = true;
            }
        }
    }
}
