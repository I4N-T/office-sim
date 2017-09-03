using UnityEngine;
using System.Collections;

public class WidgetBenchScript : MonoBehaviour {

    public Vector2 widgetBenchPos;
    public Vector2 widgetBenchUsePos;

    BoxCollider2D widgetBenchCollider;

    //WIDGET PREFABS
    public GameObject widgetPrefab;
    GameObject widgetHere;

    //SIMAI SCRIPT
    public SimAI simAIScript;

    //PRODUCTION PROGRESS STUFF
    [Range(0, 100)]
    public int progressCount;
    public bool inProgress;
    bool isProgressCoroutineStarted;
    Coroutine progressCoroutine = null;

    //USE TO DETERMINE IF needToHaul A WIDGET
    public bool hasWidgetOnIt;

    //USE TO DETERMINE IF BENCH IS ALREADY IN USE
    //public bool isOccupied;


    void Start()
    {
        widgetBenchCollider = gameObject.GetComponent<BoxCollider2D>();
        GameStats.hasWidgetBench = true;
        widgetBenchPos = gameObject.transform.position;

        //ADD THIS ITEM TO THE WIDGETBENCH ARRAY IN GAMESTATS
        GameStats.countWidgetBench++;
        GameStats.widgetBenchList.Add(this.gameObject);

        progressCount = 0;

    }


    void Update()
    {
        //Update widget bench position 
        widgetBenchPos = gameObject.transform.position;
        widgetBenchUsePos = gameObject.transform.GetChild(0).transform.position;

        //set inProgress bool from SimAI script
        if (simAIScript != null)
        {
            if (simAIScript.isWidgetBenchInProgress)
            {
                inProgress = true;
            }
            else if (!simAIScript.isWidgetBenchInProgress)
            {
                inProgress = false;
            }

            if (hasWidgetOnIt)
            {
                simAIScript.needToHaul = true;
            }

        }

        //if inProgress, then run the widget production method
        if (inProgress)
        {
            WidgetInProduction();
        }
        else if (!inProgress)
        {
            isProgressCoroutineStarted = false;
            if (progressCoroutine != null)
            {
                StopCoroutine(progressCoroutine);
            }
        }

        //determine if widget bench has a widget set on it that needs to be moved before the bench can be used again
        if (widgetHere != null)
        {
            Vector3 wPos = new Vector3(widgetHere.transform.position.x, widgetHere.transform.position.y, widgetHere.transform.position.z);

            if (widgetBenchCollider.bounds.Contains(wPos))
            {
                hasWidgetOnIt = true;
            }
            else if (!widgetBenchCollider.bounds.Contains(wPos))
            {
                hasWidgetOnIt = false;
            }
        }

    }

    //TO DO: at some point need to get reference to which sim is working on it
    void WidgetInProduction()
    {
        if (!isProgressCoroutineStarted)
        {
            progressCoroutine = StartCoroutine(ProgressIncrement(.25f));
        }

        if (progressCount >= 100)
        {
            //stop coroutine and set inProgress to false
            StopCoroutine(progressCoroutine);
            inProgress = false;
            simAIScript.isWidgetBenchInProgress = false;

            //instantiate widget on top of bench
            widgetPrefab = (GameObject)Resources.Load("Prefabs/widgetSmall");
            widgetHere = Instantiate(widgetPrefab, new Vector3(widgetBenchPos.x, widgetBenchPos.y - .2f, -1), Quaternion.identity) as GameObject;
            

            //set widget quality based on assembler skill
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


    //DELETE OBJECT
    void OnMouseDown()
    {
        //print("click happened");
        if (DeleteScript.isDelete)
        {
            if (simAIScript != null)
            {
                simAIScript.isWidgetBenchInProgress = false;
            }

            //destroy object
            Destroy(gameObject);
            DeleteScript.isDelete = false;
        }
    }

}
