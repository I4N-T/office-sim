using UnityEngine;
using System.Collections;

public class BathroomStallScript : MonoBehaviour {

    public Vector2 bathroomStallPos;

    //SIM INTERACTING WITH THIS OBJECT
    GameObject sim;
    public SimAI simAIScript;
    SimStats simStatsScript;

    //PROGRESS STUFF
    [Range(0, 100)]
    public int progressCount;
    public bool inProgress;
    public bool isProgressCoroutineStarted;
    Coroutine progressCoroutine = null;

    //EXITING AFTER USING FACILITIES
    public bool isFinishedBathroom;

    void Start()
    {
        GameStats.hasBathroomStall = true;
        bathroomStallPos = gameObject.transform.position;

        //ADD THIS ITEM TO THE FRIDGE ARRAY IN GAMESTATS
        GameStats.countBathroomStall++;
        GameStats.bathroomStallList.Add(this.gameObject);

    }


    void Update()
    {

        bathroomStallPos = gameObject.transform.position;

        if (inProgress)
        {
            BathroomInProgress();
        }
        else if (!inProgress)
        {
            simAIScript = null;
        }

        

        //this stuff stops progress from running when sim isn't working on it anymore
        if (simAIScript != null)
        {
            //if this sim has reached the target position outside the stall, finish the bathroom sequence
            if (simAIScript.simStatsScript.simPos == (Vector2)gameObject.transform.GetChild(0).position)
            {
                simAIScript.isFinishedBathroom = false;
                inProgress = false;
                //set state back to idle
                simAIScript.simFSMScript.mainState = SimFSM.MainFSM.Idle;
                simAIScript.simFSMScript.taskState = SimFSM.TaskFSM.None;
            }

            if (simAIScript.simStatsScript.objectInUse == null)
            {
                StopCoroutine(progressCoroutine);
                isProgressCoroutineStarted = false;
                inProgress = false;
            }
            else if (simAIScript.simStatsScript.objectInUse != null)
            {
                if (simAIScript.simStatsScript.objectInUse.tag != "BathroomStall")
                {
                    StopCoroutine(progressCoroutine);
                    isProgressCoroutineStarted = false;
                    inProgress = false;
                }
            }
        }

    }

    void BathroomInProgress()
    {
        if (!isProgressCoroutineStarted)
        {
            progressCoroutine = StartCoroutine(ProgressIncrement(.125f));
        }

        if (simAIScript.simStatsScript.bladder >= 100)
        {
            //stop coroutine and set inProgress to false
            StopCoroutine(progressCoroutine);
            simAIScript.isFinishedBathroom = true;

            //inProgress = false;
            

            //set state back to idle
            //simAIScript.simFSMScript.mainState = SimFSM.MainFSM.Idle;
            //simAIScript.simFSMScript.taskState = SimFSM.TaskFSM.None;

            //move sim out of stall?

            isProgressCoroutineStarted = false;         
        }
    }

    IEnumerator ProgressIncrement(float waitTime)
    {
        isProgressCoroutineStarted = true;

        while (inProgress)
        {
            simAIScript.simStatsScript.bladder += 1;
            simAIScript.simStatsScript.bladder = Mathf.Clamp(simAIScript.simStatsScript.bladder, 0, 100);

            yield return new WaitForSeconds(waitTime);
        }
    }

    //GET SIM INFORMATION
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            simAIScript = col.gameObject.GetComponent<SimAI>();

            if (simAIScript.simFSMScript.mainState == SimFSM.MainFSM.Task && simAIScript.simFSMScript.taskState == SimFSM.TaskFSM.UsingBathroom)
            {
                inProgress = true;
            }
        }
    }

    //DELETE OBJECT
    void OnMouseDown()
    {
        print("click happened");
        if (DeleteScript.isDelete)
        {
            Destroy(gameObject);
            DeleteScript.isDelete = false;
        }
    }
}
