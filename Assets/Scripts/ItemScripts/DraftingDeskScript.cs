using UnityEngine;
using System.Collections;

public class DraftingDeskScript : MonoBehaviour {

    public Vector2 draftingDeskPos;
    public Vector2 draftingDeskUsePos;

    //PRODUCTION PROGRESS STUFF
    [Range(0, 100)]
    public int progressCount;
    public bool inProgress;
    bool isProgressCoroutineStarted;
    Coroutine progressCoroutine = null;
    Coroutine skillExpCoroutine = null;

    //SIM INTERACTING WITH THIS BENCH
    GameObject sim;
    public SimAI simAIScript;

    //RENDERER (for delete coroutine)
    SpriteRenderer rend;

    //AUDIO
    AudioClip designImprovementSoundClip;
    AudioClip deleteSoundClip;
    public AudioSource sfxSource;

    void Start()
    {
        //Source will stay with sellsoundclip unless deletion happens
        //sfxSource.clip = AudioScript.instance.sellSoundClip;
        deleteSoundClip = AudioScript.instance.deleteSoundClip;

        GameStats.hasDraftingDesk = true;
        draftingDeskPos = gameObject.transform.position;

        //ADD THIS ITEM TO THE WIDGETBENCH ARRAY IN GAMESTATS
        GameStats.countDraftingDesk++;
        GameStats.draftingDeskList.Add(this.gameObject);

        progressCount = 0;
    }

    void Update()
    {
        //Update drafting desk position 
        draftingDeskPos = gameObject.transform.position;

        if (inProgress)
        {
            DraftingInProgress();
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
                StopCoroutine(skillExpCoroutine);
                isProgressCoroutineStarted = false;
                inProgress = false;
            }
            else if (simAIScript.simStatsScript.objectInUse != null)
            {
                if (simAIScript.simStatsScript.objectInUse.tag != "DraftingDesk")
                {
                    StopCoroutine(progressCoroutine);
                    StopCoroutine(skillExpCoroutine);
                    isProgressCoroutineStarted = false;
                    inProgress = false;
                }
            }
        }


    }

    void DraftingInProgress()
    {
        if (!isProgressCoroutineStarted)
        {
            progressCoroutine = StartCoroutine(ProgressIncrement(5f));
            skillExpCoroutine = StartCoroutine(SkillExpIncrement(3f));
        }

        if (progressCount >= 100)
        {
            //stop coroutine and set inProgress to false
            StopCoroutine(progressCoroutine);
            StopCoroutine(skillExpCoroutine);
            inProgress = false;

            //increase widget design level by 1
            GameStats.widgetDesignLevel += 1;
            //print(GameStats.widgetDesignLevel);

            EventLogScript.instance.AddEvent(simAIScript.simStatsScript.simName + " made a design change. The current revision level is " + GameStats.widgetDesignLevel + ".");

            //set progressCount back to 0
            progressCount = 0;
            isProgressCoroutineStarted = false;


        }
    }

    IEnumerator ProgressIncrement(float waitTime)
    {
        isProgressCoroutineStarted = true;

        while (inProgress)
        {
            //simAIScript.simStatsScript.engineeringExp += 2;
            progressCount += 1;
            yield return new WaitForSeconds(waitTime);

        }
    }

    IEnumerator SkillExpIncrement(float waitTime)
    {
        isProgressCoroutineStarted = true;

        while (inProgress)
        {
            simAIScript.simStatsScript.engineeringExp += 1;
            yield return new WaitForSeconds(waitTime);

        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            simAIScript = col.gameObject.GetComponent<SimAI>();
            if (simAIScript.simFSMScript.mainState == SimFSM.MainFSM.Task && simAIScript.simFSMScript.taskState == SimFSM.TaskFSM.Drafting)
            {
                inProgress = true;
            }
        }
    }

    //When sim goes to use bathroom, must set objectinuse to null in order to stop the coroutine
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            //this sets the sim's objectinuse to null once it leaves the desk
            //the reason we declare a new SimStats object here is in case another unrelated sim happens to cross through the trigger
            SimAI simAIHere = col.gameObject.GetComponent<SimAI>();
            simAIHere.simStatsScript.objectInUse = null;
            
        }
    }

    //DELETE OBJECT
    void OnMouseDown()
    {
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
