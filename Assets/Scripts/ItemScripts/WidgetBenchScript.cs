using UnityEngine;
using System.Collections;

public class WidgetBenchScript : MonoBehaviour {

    public Vector2 benchPos;

    //WIDGET PREFABS
    public GameObject widgetSmall;

    //PRODUCTION PROGRESS STUFF
    [Range(0, 100)]
    public int progressCount;
    public bool inProgress;
    bool isProgressCoroutineStarted;
    Coroutine progressCoroutine = null;

    bool instantiateSwitch;

    void Start()
    {

        GameStats.hasWidgetBench = true;
        benchPos = gameObject.transform.position;

        //ADD THIS ITEM TO THE FRIDGE ARRAY IN GAMESTATS
        GameStats.countWidgetBench++;
        GameStats.widgetBenchList.Add(this.gameObject);

        progressCount = 0;

    }


    void Update()
    {

        benchPos = gameObject.transform.position;

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

        print("isprogresscoroutinestarted = " + isProgressCoroutineStarted);
        //widget instantiation
        //InstantiateWidget();

    }

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

            //instantiate widget on top of bench
            //instantiateSwitch = true;
            Instantiate(widgetSmall, new Vector3(benchPos.x, benchPos.y, 0), Quaternion.identity);

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
            print("isrunning");
            yield return new WaitForSeconds(waitTime);
        }
    }

    void InstantiateWidget()
    {
        if (instantiateSwitch)
        {
            Instantiate(widgetSmall, new Vector3(benchPos.x, benchPos.y, 0), Quaternion.identity);
        }

        instantiateSwitch = false;
    }
}
