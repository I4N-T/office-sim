using UnityEngine;
using System.Collections;

public class AltSimAI : MonoBehaviour {

    Rigidbody2D rb2D;
    float moveSpeed = 2f;

    int moveVal;
    float timeLeft;
    float moveTime;

    //PATHFINDING STUFF
    public bool isIdle;

    public Vector2 targetPos;
    public bool isGettingFood;

    //ITEM STUFF
    //GENERAL
    //public bool isHolding;

    //FRIDGE 

    //WIDGET BENCH
    //public bool isUsingWidgetBench;
    public bool isWidgetBenchInProgress;
    public bool needToHaul;

    //ARRAY TO HOLD ALL OTHER SIMS TO KEEP TRACK OF USE INTERFERENCES
    public GameObject[] otherSimArray;

    //SIM STATS SCRIPT INSTANCE
    SimStats simStatsScript;

    SimFSM simFSMScript;



    public int objID;


    void Start()
    {

        isIdle = true;

        simFSMScript = gameObject.GetComponent<SimFSM>();
        simStatsScript = gameObject.GetComponent<SimStats>();
        //simManagerScript = gameObject.GetComponent<SimManager>();

        //GET OTHER SIM OBJECTS ARRAY
        otherSimArray = GameObject.FindGameObjectsWithTag("Sim");



        rb2D = GetComponent<Rigidbody2D>();
        timeLeft = 2f;
        moveVal = Random.Range(1, 9);

    }

    //UPDATE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    void FixedUpdate()
    {

        //UPDATE simPos
        simStatsScript.simPos = gameObject.transform.position;

        //REPLENISH NEEDS IF POSSIBLE

        //ENERGY

        //HUNGER
        if (simStatsScript.hunger < 40)
        {
            //check for food
            if (GameStats.hasFridge == true)
            {
                simFSMScript.mainState = SimFSM.MainFSM.Task;
                simFSMScript.taskState = SimFSM.TaskFSM.GettingFood;
            }
            else if (!GameStats.hasFridge)
            {
                //change this to set off notification that sim is starving
                simFSMScript.mainState = SimFSM.MainFSM.Idle;
            }

        }

        //IF NEEDS ARE MET THEN WORK ON HIGHEST PRIORITY TASK POSSIBLE (refactor this)
        else if (simStatsScript.hunger >= 40 && simStatsScript.energy >= 30)
        {
            if (simStatsScript.canLabor)
            {
                if (GameStats.hasWidgetBench)
                {
                    simFSMScript.mainState = SimFSM.MainFSM.Task;
                    simFSMScript.taskState = SimFSM.TaskFSM.MakingWidget;
                }
                else if (!GameStats.hasWidgetBench)
                {
                    simFSMScript.mainState = SimFSM.MainFSM.Idle;
                }

            }

        }

       

        /*if (simFSMScript.mainState == SimFSM.MainFSM.Task && simFSMScript.taskState != SimFSM.TaskFSM.MakingWidget)
        {

            isWidgetBenchInProgress = false;

            if (!isWidgetBenchInProgress && !needToHaul)
            {
                //print("it ran " + simStatsScript.simName);
                simStatsScript.objectInUse = null;
            }
        }*/



    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


    //IF IN TRANSIT ----------------------------------------------------------------------------------------------------------------
    public void GoToward(Vector2 targetPos)
    {
        //rb2D.MovePosition(targetPos);
        //Vector2 simPos = gameObject.transform.position;
        transform.position = Vector2.MoveTowards(simStatsScript.simPos, targetPos, moveSpeed * Time.deltaTime);
    }

    public void GetTargetPosFridge()
    {
        FridgeScript fridgeScript;
        //Vector2 simPos = gameObject.transform.position;
        Vector2 testPos = new Vector2(0, 0);
        float mag1 = 0;
        float mag2 = 9999;

        //for each fridge in fridgeList, compare positions to determine which is closest
        foreach (GameObject fridge in GameStats.fridgeList)
        {
            //the null check is to prevent a NullReferenceException error upon deleting fridge
            if (fridge != null)
            {
                fridgeScript = fridge.GetComponent<FridgeScript>();
                mag1 = Vector2.Distance(simStatsScript.simPos, fridgeScript.fridgePos);
                if (mag1 < mag2)
                {
                    mag2 = mag1;
                    targetPos = fridgeScript.fridgePos;
                    simStatsScript.objectInUse = fridge;
                }
            }


        }
    }

    public void GetTargetPosWidgetBench()
    {
        AltWidgetBenchScript widgetBenchScript;
        //Vector2 simPos = gameObject.transform.position;
        Vector2 testPos = new Vector2(0, 0);
        float mag1 = 0;
        float mag2 = 9999;

        //for each wb in wbList, compare positions to determine which is closest
        foreach (GameObject widgetBench in GameStats.widgetBenchList)
        {
            //Check if null
            if (widgetBench != null)
            {
                widgetBenchScript = widgetBench.GetComponent<AltWidgetBenchScript>();
                //if this sim is using this bench already, set target so he stays
                if (objID == widgetBenchScript.gameObject.GetInstanceID())
                {
                    //print("1" + simStatsScript.simName + " is going to " + widgetBenchScript.gameObject.GetInstanceID());
                    targetPos = widgetBenchScript.widgetBenchUsePos;
                    return;
                }

                else if (objID != widgetBenchScript.gameObject.GetInstanceID() /*simStatsScript.objectInUse == null || simStatsScript.objectInUse != widgetBenchScript.gameObject*/)
                {

                    //if sim is already at a bench, don't do this next part
                    if (!isWidgetBenchInProgress && !needToHaul && simStatsScript.objectInUse == null)
                    {
                        //For any widget bench that is not being used, find bench at closest distance 
                        if (!widgetBenchScript.inProgress)
                        {
                            mag1 = Vector2.Distance(simStatsScript.simPos, widgetBenchScript.widgetBenchPos);
                            if (mag1 < mag2)
                            {
                                mag2 = mag1;
                                targetPos = widgetBenchScript.widgetBenchUsePos;
                                //print("2" + simStatsScript.simName + " is going to " + widgetBenchScript.gameObject.GetInstanceID());
                            }

                        }
                    }
                }

            }
        }
    }

    public void GetTargetPosHaulDropoff()
    {
        //if there is a zone, go to nearest empty tile in zone

        //else if no zone then drop next to bench
        //THIS IS JUST LIKE WidgetBenchTargetPos EXCEPT THE LINE THAT SETS THE TARGET POSITION
        AltWidgetBenchScript widgetBenchScript;
        Vector2 testPos = new Vector2(0, 0);
        float mag1 = 0;
        float mag2 = 9999;

        //for each wb in wbList, compare positions to determine which is closest
        foreach (GameObject widgetBench in GameStats.widgetBenchList)
        {
            widgetBenchScript = widgetBench.GetComponent<AltWidgetBenchScript>();
            mag1 = Vector2.Distance(simStatsScript.simPos, widgetBenchScript.widgetBenchPos);
            if (mag1 < mag2)
            {
                mag2 = mag1;
                targetPos = widgetBenchScript.widgetBenchPos;
            }
        }

        targetPos = targetPos + new Vector2(2, 0);
    }
    //---------------------------------------------------------------------------------------------------------------------------

    public void HaulWidget()
    {
        //isUsingWidgetBench = false;

        GetTargetPosHaulDropoff();
        GoToward(targetPos);

        //if simPos = targetPos, drop the item
        if (simStatsScript.simPos == targetPos)
        {
            needToHaul = false;
            simStatsScript.itemInPossession = null;
            //isHolding = false;
            //isUsingWidgetBench = true;
        }
    }

    public bool IsWidgetBenchAvailable()
    {
        AltSimAI otherSimAIScript;
        SimFSM otherSimFSMScript;
        int beingUsed = -1;

        if (GameStats.countWidgetBench >= 0)
        {
            foreach (GameObject otherSimObj in otherSimArray)
            {
                if (otherSimObj != gameObject)
                {
                    otherSimAIScript = otherSimObj.GetComponent<AltSimAI>();
                    otherSimFSMScript = otherSimObj.GetComponent<SimFSM>();
                    if ((otherSimFSMScript.mainState == SimFSM.MainFSM.Task && otherSimFSMScript.taskState == SimFSM.TaskFSM.MakingWidget) || otherSimAIScript.needToHaul)
                    {
                        beingUsed++;
                    }
                }
            }
        }
        if (beingUsed < GameStats.countWidgetBench)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //COLLISION LOGIC ---------------------------------------------------------------------
    void OnCollisionStay2D(Collision2D col)
    {
        //FRIDGE ADDS 60 TO HUNGER
        if (col.gameObject.tag == "Fridge")
        {
            if (simFSMScript.mainState == SimFSM.MainFSM.Task && simFSMScript.taskState == SimFSM.TaskFSM.GettingFood)
            {
                int hunger = simStatsScript.hunger;
                hunger += 60;
                //ENSURE MAX HUNGER IS 100
                hunger = Mathf.Clamp(hunger, 0, 100);
                //ACTUALLY SET THE NEW HUNGER VALUE 
                simStatsScript.hunger = hunger;

                //TASK COMPLETE
                simStatsScript.objectInUse = null;
                simFSMScript.taskState = SimFSM.TaskFSM.None;
                simFSMScript.mainState = SimFSM.MainFSM.Idle;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //WIDGET BENCH IS USED 
        if (col.gameObject.tag == "WidgetBenchChild")
        {
            if (simFSMScript.mainState == SimFSM.MainFSM.Task && simFSMScript.taskState == SimFSM.TaskFSM.MakingWidget)
            {
                //set inProgress to true
                isWidgetBenchInProgress = true;

                //set the col.parent to be the object in use for this sim
                simStatsScript.objectInUse = col.transform.parent.gameObject;
                //consider changing the method from using objID to just using the gameobject itself
                objID = simStatsScript.objectInUse.GetInstanceID();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------


    //IF IDLE, THEN WANDER
    public void IdleWander()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            moveTime = Random.Range(0.5f, 2.5f);

            if (timeLeft < -moveTime)
            {
                timeLeft = Random.Range(2f, 6f);
                moveVal = Random.Range(1, 9);

            }
            if (moveVal == 1)
            {
                MoveRight();
                if (timeLeft < -moveTime)
                {
                    timeLeft = Random.Range(2f, 6f);
                    //moveVal = Random.Range(1,5);
                    // moveVal = Random.Range(1, 9);

                }
            }
            if (moveVal == 2)
            {

                MoveLeft();
                if (timeLeft < -moveTime)
                {
                    timeLeft = Random.Range(2f, 6f);
                    //moveVal = Random.Range(1,5);
                    //moveVal = Random.Range(1, 9);
                }

            }
            if (moveVal == 3)
            {

                MoveUp();
                if (timeLeft < -moveTime)
                {
                    timeLeft = Random.Range(2f, 6f);
                    //moveVal = Random.Range(1,5);
                    // moveVal = Random.Range(1, 9);
                }
            }
            if (moveVal == 4)
            {

                MoveDown();
                if (timeLeft < -moveTime)
                {
                    timeLeft = Random.Range(2f, 6f);
                    //moveVal = Random.Range(1,5);
                    // moveVal = Random.Range(1, 9);
                }
            }
            if (moveVal == 5)
            {

                MoveUpRight();
                if (timeLeft < -moveTime)
                {
                    timeLeft = Random.Range(2f, 6f);
                    //moveVal = Random.Range(1,5);
                    // moveVal = Random.Range(1, 9);
                }
            }
            if (moveVal == 6)
            {

                MoveDownRight();
                if (timeLeft < -moveTime)
                {
                    timeLeft = Random.Range(2f, 6f);
                    //moveVal = Random.Range(1,5);
                    //moveVal = Random.Range(1, 9);
                }
            }
            if (moveVal == 7)
            {

                MoveUpLeft();
                if (timeLeft < -moveTime)
                {
                    timeLeft = Random.Range(2f, 6f);
                    //moveVal = Random.Range(1,5);
                    // moveVal = Random.Range(1, 9);
                }
            }
            if (moveVal == 8)
            {

                MoveDownLeft();
                if (timeLeft < -moveTime)
                {
                    timeLeft = Random.Range(2f, 6f);
                    //moveVal = Random.Range(1,5);
                    // moveVal = Random.Range(1, 9);
                }
            }
        }
    }

    public void MoveRight()
    {
        Vector2 mvecE = new Vector2(1, 0);

        rb2D.MovePosition(rb2D.position + mvecE * (moveSpeed * Time.deltaTime));
    }

    public void MoveUp()
    {
        Vector2 mvecE = new Vector2(0, 1);

        rb2D.MovePosition(rb2D.position + mvecE * (moveSpeed * Time.deltaTime));
    }

    public void MoveLeft()
    {
        Vector2 mvecE = new Vector2(-1, 0);

        rb2D.MovePosition(rb2D.position + mvecE * (moveSpeed * Time.deltaTime));
    }

    public void MoveDown()
    {
        Vector2 mvecE = new Vector2(0, -1);

        rb2D.MovePosition(rb2D.position + mvecE * (moveSpeed * Time.deltaTime));
    }

    public void MoveUpRight()
    {
        Vector2 mvecE = new Vector2(1, 1);

        rb2D.MovePosition(rb2D.position + mvecE * (moveSpeed * Time.deltaTime));
    }

    public void MoveDownRight()
    {
        Vector2 mvecE = new Vector2(-1, 1);

        rb2D.MovePosition(rb2D.position + mvecE * (moveSpeed * Time.deltaTime));
    }

    public void MoveUpLeft()
    {
        Vector2 mvecE = new Vector2(-1, 1);

        rb2D.MovePosition(rb2D.position + mvecE * (moveSpeed * Time.deltaTime));
    }

    public void MoveDownLeft()
    {
        Vector2 mvecE = new Vector2(-1, -1);

        rb2D.MovePosition(rb2D.position + mvecE * (moveSpeed * Time.deltaTime));
    }
}
