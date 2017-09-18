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

    //SIM INTERACTING WITH THIS BENCH
    GameObject sim;
    SimAI simAIScript;

    void Start()
    {
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
                isProgressCoroutineStarted = false;
                inProgress = false;
            }
            else if (simAIScript.simStatsScript.objectInUse != null)
            {
                if (simAIScript.simStatsScript.objectInUse.tag != "DraftingDesk")
                {
                    StopCoroutine(progressCoroutine);
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
        }

        if (progressCount >= 100)
        {
            //stop coroutine and set inProgress to false
            StopCoroutine(progressCoroutine);
            inProgress = false;

            //increase widget design level by 1
            GameStats.widgetDesignLevel += 1;
            print(GameStats.widgetDesignLevel);

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
            progressCount += 1;
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

    //DELETE OBJECT
    void OnMouseDown()
    {
        if (DeleteScript.isDelete)
        {
            //destroy object
            Destroy(gameObject);
            DeleteScript.isDelete = false;
        }
    }
}
