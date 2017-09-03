using UnityEngine;
using System.Collections;

public class SimAI : MonoBehaviour {

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
    public SimStats simStatsScript;

    public SimFSM simFSMScript;

    public int objID;

    public bool isStarted;


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
    }

    void Update() { 

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
            //LABOR TASKS (Production)
            if (simStatsScript.canLabor)
            {
                if (GameStats.hasWidgetBench)
                {
                    if (IsWidgetBenchAvailable())
                    {
                        simFSMScript.mainState = SimFSM.MainFSM.Task;
                        simFSMScript.taskState = SimFSM.TaskFSM.MakingWidget;
                    }
                    else if (!IsWidgetBenchAvailable())
                    {
                        simFSMScript.mainState = SimFSM.MainFSM.Idle;
                        simFSMScript.taskState = SimFSM.TaskFSM.None;
                    }
                    
                }
                else if (!GameStats.hasWidgetBench)
                {
                    simFSMScript.mainState = SimFSM.MainFSM.Idle;
                    simFSMScript.taskState = SimFSM.TaskFSM.None;
                }
            }

            //SALES TASKS
            if (simStatsScript.canSales)
            {
                if (GameStats.hasSalesBench)
                {
                    //if unnoccupied sales station is available
                    if (IsSalesBenchAvailable())
                    {
                        //You can't sell what you don't have
                        if (GameStats.hasWidgetInStockpile)
                        {
                            simFSMScript.mainState = SimFSM.MainFSM.Task;
                            simFSMScript.taskState = SimFSM.TaskFSM.Sales;
                        }
                        else if (!GameStats.hasWidgetInStockpile) //might not work well once priorities are implemented
                        {
                            simFSMScript.mainState = SimFSM.MainFSM.Idle;
                            simFSMScript.taskState = SimFSM.TaskFSM.None;
                        }
                    }
                    else if (!IsSalesBenchAvailable())
                    {
                        simFSMScript.mainState = SimFSM.MainFSM.Idle;
                        simFSMScript.taskState = SimFSM.TaskFSM.None;
                    }
                    
                }
            }

        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //void MakeDecision(){}


    //IF IN TRANSIT ----------------------------------------------------------------------------------------------------------------
    public void GoToward(Vector2 targetPos)
    {
        //rb2D.MovePosition(targetPos);
        //Vector2 simPos = gameObject.transform.position;
        transform.position = Vector3.MoveTowards(new Vector3(simStatsScript.simPos.x, simStatsScript.simPos.y, -1f), new Vector3(targetPos.x, targetPos.y, -1f), moveSpeed * Time.deltaTime);
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
        WidgetBenchScript widgetBenchScript;
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
                widgetBenchScript = widgetBench.GetComponent<WidgetBenchScript>();
                //if this sim is using this bench already, set target so he stays
                if (objID == widgetBenchScript.gameObject.GetInstanceID())
                {
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
        if (GameStats.hasStockpileZone)
        {
            //get position of nearest stockpile zone
            TileBehavior tileBehaviorScript;
            float mag1 = 0;
            float mag2 = 9999;

            foreach (GameObject tile in GameStats.tileList)
            {
                tileBehaviorScript = tile.GetComponent<TileBehavior>();
                if (tileBehaviorScript.isStockpileZone)
                {
                    if (tileBehaviorScript.itemOnTile == null)
                    {
                        mag1 = Vector2.Distance(simStatsScript.simPos, tile.transform.position);
                        if (mag1 < mag2)
                        {
                            mag2 = mag1;
                            targetPos = tile.transform.position;
                        }
                    }
                    //if there is an object on this tile already
                    else if (tileBehaviorScript.itemOnTile != null)
                    {
                        
                        if (simStatsScript.itemInPossession != null)
                        {
                            //and if that object is just the one being carried right now
                            if (tileBehaviorScript.itemOnTile.GetInstanceID() == simStatsScript.itemInPossession.GetInstanceID())
                            {
                                targetPos = tile.transform.position;
                                return;
                            }
                            /*else if (tileBehaviorScript.itemOnTile.GetInstanceID() != simStatsScript.itemInPossession.GetInstanceID())
                            {
                                return;
                            }*/
                        }
                        
                        
                    }
                    
                }
                
            } 

        }

        //else if no zone then drop next to bench
        else if (!GameStats.hasStockpileZone)
        {
            //THIS IS JUST LIKE WidgetBenchTargetPos EXCEPT THE LINE THAT SETS THE TARGET POSITION
            WidgetBenchScript widgetBenchScript;
            Vector2 testPos = new Vector2(0, 0);
            float mag1 = 0;
            float mag2 = 9999;

            //for each wb in wbList, compare positions to determine which is closest
            foreach (GameObject widgetBench in GameStats.widgetBenchList)
            {
                widgetBenchScript = widgetBench.GetComponent<WidgetBenchScript>();
                mag1 = Vector2.Distance(simStatsScript.simPos, widgetBenchScript.widgetBenchPos);
                if (mag1 < mag2)
                {
                    mag2 = mag1;
                    targetPos = widgetBenchScript.widgetBenchPos;
                }
            }

            targetPos = targetPos + new Vector2(2, 0);
        }
        
    }

    public void GetTargetPosSalesBench()
    {
        SalesBenchScript salesBenchScript;
        //Vector2 simPos = gameObject.transform.position;
        Vector2 testPos = new Vector2(0, 0);
        float mag1 = 0;
        float mag2 = 9999;

        //for each bench in salesBenchList, compare positions to determine which is closest
        foreach (GameObject salesBench in GameStats.salesBenchList)
        {
            //the null check is to prevent a NullReferenceException error upon deleting bench
            if (salesBench != null)
            {
                salesBenchScript = salesBench.GetComponent<SalesBenchScript>();
                //if this sim is using this bench already, set target so he stays
                if (objID == salesBenchScript.gameObject.GetInstanceID())
                {
                    print("runningRRR");
                    targetPos = salesBenchScript.salesBenchPos;

                    return;
                }

                //only get position of benches not being used
                if (!salesBenchScript.inProgress)
                {
                    mag1 = Vector2.Distance(simStatsScript.simPos, salesBenchScript.salesBenchPos);
                    if (mag1 < mag2)
                    {
                        mag2 = mag1;
                        targetPos = salesBenchScript.salesBenchPos;
                        /*simStatsScript.objectInUse = salesBench;
                        objID = salesBench.GetInstanceID();*/
                    }
                }
                
            }


        }
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
            //add widget to list
            GameStats.widgetList.Add(simStatsScript.itemInPossession);

            needToHaul = false;
            simStatsScript.itemInPossession = null;
            //GameStats.countWidgetInStockpile++;
        }
    }

    public bool IsWidgetBenchAvailable()
    {
        SimAI otherSimAIScript;
        SimFSM otherSimFSMScript;
        int beingUsed = -1;

        if (GameStats.countWidgetBench >= 0)
        {
            foreach (GameObject otherSimObj in otherSimArray)
            {
                if (otherSimObj != gameObject)
                {
                    otherSimAIScript = otherSimObj.GetComponent<SimAI>();
                    otherSimFSMScript = otherSimObj.GetComponent<SimFSM>();
                    if ((otherSimFSMScript.mainState == SimFSM.MainFSM.Task && otherSimFSMScript.taskState == SimFSM.TaskFSM.MakingWidget))
                    {
                        
                        if (otherSimAIScript.simStatsScript.objectInUse != null)
                        {
                            beingUsed++;
                        }
                        
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

    public bool IsSalesBenchAvailable()
    {
        SimAI otherSimAIScript;
        SimFSM otherSimFSMScript;
        int beingUsed = -1;

        if (GameStats.countSalesBench >= 0)
        {
            foreach (GameObject otherSimObj in otherSimArray)
            {
                if (otherSimObj != gameObject)
                {
                    otherSimAIScript = otherSimObj.GetComponent<SimAI>();
                    otherSimFSMScript = otherSimObj.GetComponent<SimFSM>();
                    if ((otherSimFSMScript.mainState == SimFSM.MainFSM.Task && otherSimFSMScript.taskState == SimFSM.TaskFSM.Sales))
                    {
                        if (otherSimAIScript.simStatsScript.objectInUse != null)
                        {
                            if (otherSimAIScript.simStatsScript.objectInUse.tag == "SalesBench")
                            {
                                beingUsed++;
                            }
                        }
                    }
                }
            }
        }
        if (beingUsed < GameStats.countSalesBench)
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

        //SALES BENCH
        if (col.gameObject.tag == "SalesBench")
        {
            if (simFSMScript.mainState == SimFSM.MainFSM.Task && simFSMScript.taskState == SimFSM.TaskFSM.Sales)
            {
                //set the col.parent to be the object in use for this sim
                simStatsScript.objectInUse = col.transform.gameObject;
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
