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
    WidgetScript thisWidgetScript;
    int widgetSellPrice;


    //SIM INTERACTING WITH THIS BENCH
    GameObject sim;
    SimAI simAIScript;

    
    void Start () {
        GameStats.hasSalesBench = true;
        salesBenchPos = gameObject.transform.position;

        //ADD THIS ITEM TO THE WIDGETBENCH ARRAY IN GAMESTATS
        GameStats.countSalesBench++;
        GameStats.salesBenchList.Add(this.gameObject);

        progressCount = 0;

    }
	
	
	void Update () {
        //Update sales bench position 
        salesBenchPos = gameObject.transform.position;

        if (inProgress)
        {
            SalesInProgress();
        }
        else if (!inProgress)
        {
            simAIScript = null;
        }

        //this stuff stops bench from running when sim isn't working on it anymore
        if (simAIScript != null)
        {
            if (simAIScript.simStatsScript.objectInUse == null)
            {
                StopCoroutine(progressCoroutine);
                isProgressCoroutineStarted = false;
                inProgress = false;
            }
            else if (simAIScript.simStatsScript.objectInUse != null)
            {
                if (simAIScript.simStatsScript.objectInUse.tag == "Fridge")
                {
                    StopCoroutine(progressCoroutine);
                    isProgressCoroutineStarted = false;
                    inProgress = false;
                }
            }
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

            thisWidgetScript = GameStats.widgetList[0].GetComponent<WidgetScript>();

            //have chance to sell a widget
            //price based on quality 
            if (thisWidgetScript.widgetQuality == "Bad")
            {
                widgetSellPrice += 50;
            }
            else if (thisWidgetScript.widgetQuality == "Average")
            {
                widgetSellPrice += 75;
            }
            if (thisWidgetScript.widgetQuality == "Good")
            {
                widgetSellPrice += 100;
            }

            //bonus or detriment based on sales skill
            if (simAIScript.simStatsScript.sales <= 3)
            {
                widgetSellPrice = Mathf.RoundToInt(widgetSellPrice * .75f);
                print("low");
            }
            else if (simAIScript.simStatsScript.sales > 3 && simAIScript.simStatsScript.sales < 7)
            {
                print("medium");
            }
            else if (simAIScript.simStatsScript.sales >= 7)
            {
                widgetSellPrice = Mathf.RoundToInt(widgetSellPrice * 1.25f);
                print("high");
            }

            GameStats.dollars += widgetSellPrice;
            print("widget sold for $" + widgetSellPrice);
            Destroy(GameStats.widgetList[0]);

            //set progressCount back to 0
            progressCount = 0;
            widgetSellPrice = 0;

            //if more widgets yet to sell, keep going; else if no widgets then go idle
            if (GameStats.countWidgetInStockpile >= 0)
            {
                isProgressCoroutineStarted = false;
            }
            
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

    /*void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            inProgress = false;
        }
    }*/

    //DELETE OBJECT
    void OnMouseDown()
    {
        //print("click happened");
        if (DeleteScript.isDelete)
        {
            //destroy object
            Destroy(gameObject);
            DeleteScript.isDelete = false;
        }
    }
}
