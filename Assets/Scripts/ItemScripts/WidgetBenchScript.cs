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
    Coroutine skillExpCoroutine = null;

    //USE TO DETERMINE IF needToHaul A WIDGET
    public bool hasWidgetOnIt;

    //RENDERER (for delete coroutine)
    SpriteRenderer rend;

    //AUDIO
    AudioClip widgetProduceSoundClip;
    AudioClip deleteSoundClip;
    public AudioSource sfxSource;


    void Start()
    {
        //Source will stay with widgetProduceSoundClip unless deletion happens
        sfxSource.clip = AudioScript.instance.widgetProduceSoundClip;
        deleteSoundClip = AudioScript.instance.deleteSoundClip;

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
                StopCoroutine(skillExpCoroutine);
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
            skillExpCoroutine = StartCoroutine(SkillExpIncrement(3f));
        }

        if (progressCount >= 100)
        {
            //stop coroutine and set inProgress to false
            StopCoroutine(progressCoroutine);
            StopCoroutine(skillExpCoroutine);
            inProgress = false;
            simAIScript.isWidgetBenchInProgress = false;
            WidgetScript thisWidgetScript;
            string widgetQuality = "average";

            //instantiate widget on top of bench
            widgetPrefab = (GameObject)Resources.Load("Prefabs/widgetSmall");
            //make sure widget instantiates with the proper offset in order to allow the collider to overlap with sim collider
            if (gameObject.transform.eulerAngles == new Vector3(0,0, 0))
            {
                widgetHere = Instantiate(widgetPrefab, new Vector3(widgetBenchPos.x, widgetBenchPos.y - .2f, -1), gameObject.transform.rotation) as GameObject;
            }
            else if (gameObject.transform.eulerAngles == new Vector3(0, 0, 90))
            {
                widgetHere = Instantiate(widgetPrefab, new Vector3(widgetBenchPos.x + .2f, widgetBenchPos.y, -1), gameObject.transform.rotation) as GameObject;
            }
            else if (gameObject.transform.eulerAngles == new Vector3(0, 0, 180))
            {
                widgetHere = Instantiate(widgetPrefab, new Vector3(widgetBenchPos.x, widgetBenchPos.y + .2f, -1), gameObject.transform.rotation) as GameObject;
            }
            else if (gameObject.transform.eulerAngles == new Vector3(0, 0, 270))
            {
                widgetHere = Instantiate(widgetPrefab, new Vector3(widgetBenchPos.x - .2f, widgetBenchPos.y, -1), gameObject.transform.rotation) as GameObject;
            }

            thisWidgetScript = widgetHere.GetComponent<WidgetScript>();

            //set widget quality based on assembler skill
            if (simAIScript.simStatsScript.labor <= 3)
            {
                thisWidgetScript.widgetQuality = "Bad";
            }
            else if (simAIScript.simStatsScript.labor > 3 && simAIScript.simStatsScript.labor <= 7)
            {
                thisWidgetScript.widgetQuality = "Average";
            }
            else if (simAIScript.simStatsScript.labor > 7)
            {
                thisWidgetScript.widgetQuality = "Good";
            }

            //print to event log
            EventLogScript.instance.AddEvent(simAIScript.simStatsScript.simName + " built a widget of " + thisWidgetScript.widgetQuality + " quality.");

            //play sound
            sfxSource.Play();

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

    IEnumerator SkillExpIncrement(float waitTime)
    {
        isProgressCoroutineStarted = true;

        while (inProgress)
        {
            simAIScript.simStatsScript.laborExp += 1;
            yield return new WaitForSeconds(waitTime);

        }
    }


    //DELETE OBJECT
    void OnMouseDown()
    {
        //print("click happened");
        if (DeleteScript.isDelete)
        {
            StartCoroutine(Delete());
        }
    }

    IEnumerator Delete()
    {
        //play sound
        sfxSource.clip = deleteSoundClip;
        sfxSource.Play();
        //disable sprite
        rend = gameObject.GetComponent<SpriteRenderer>();
        rend.enabled = false;

        DeleteScript.isDelete = false;

        yield return new WaitForSecondsRealtime(.75f);

        //destroy object
        Destroy(gameObject);
    }

}
