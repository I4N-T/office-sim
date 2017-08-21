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
    public bool isHolding;

    //FRIDGE 

    //WIDGET BENCH
    public bool isUsingWidgetBench;
    public bool isWidgetBenchInProgress;
    public bool needToHaul;

    /* //GUI SCRIPT INSTANCE
     MainGUI guiScript = new MainGUI();*/

    //ARRAY TO HOLD ALL OTHER SIMS TO KEEP TRACK OF USE INTERFERENCES
    public GameObject[] otherSimArray;

    //SIM STATS SCRIPT INSTANCE
    SimStats simStatsScript;

    //ITEM SCRIPT INSTANCES
    //WidgetBenchScript widgetBenchScript;

    //SimManager simManagerScript;

    public int objID;


    void Start () {

        isIdle = true;

        simStatsScript = gameObject.GetComponent<SimStats>();
        //simManagerScript = gameObject.GetComponent<SimManager>();

        //GET OTHER SIM OBJECTS ARRAY
        otherSimArray = GameObject.FindGameObjectsWithTag("Sim");
        
        

        rb2D = GetComponent<Rigidbody2D>();
        timeLeft = 2f;
        moveVal = Random.Range(1, 9);

        /*//GUI STUFF
        guiScript.simNameText = */

        //NEED DEPLETIONS
        StartCoroutine(EnergyDeplete(5f));
        StartCoroutine(HungerDeplete(3f));
        

    }

    //UPDATE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    void FixedUpdate()
    {

        print(simStatsScript.simName + " is using widget bench: " + isUsingWidgetBench);
        //UPDATE simPos
        simStatsScript.simPos = gameObject.transform.position;

        //SET isHolding BOOL
        if (simStatsScript.itemInPossession != null)
        {
            isHolding = true;
        }
        else if (simStatsScript.itemInPossession == null)
        {
            isHolding = false;
        }
        

        //if no task then IdleWander()
        if (isIdle)
        {
            IdleWander();
        }
        

        //REPLENISH NEEDS IF POSSIBLE

        //ENERGY

        //HUNGER
        if (simStatsScript.hunger < 40)
        {
            //check for food
            if (GameStats.hasFridge == true)
            {
                //set targetPos to frigdePos; must compare position to each fridgePos to find closest fridge 
                isIdle = false;
                isUsingWidgetBench = false;
                needToHaul = false;
                isGettingFood = true;
    
                GetTargetPosFridge();
                GoToward(targetPos);
            }
            
        }

        //IF NEEDS ARE MET THEN WORK ON HIGHEST PRIORITY TASK POSSIBLE (refactor this)
        else if (simStatsScript.hunger >= 40 && simStatsScript.energy >= 30)

            //WidgetBenchAvailable();
        {
            if (simStatsScript.canLabor)
            {
                if (GameStats.hasWidgetBench && needToHaul)
                {
                    HaulWidget();
                    //this next line is needed to prevent another sim from going isUsingWidgetBench = true while this sim changes it's needtohaul bool
                    isUsingWidgetBench = true;
                }
                else if (GameStats.hasWidgetBench && !needToHaul)
                {
                    //if widget bench is available
                    if (IsWidgetBenchAvailable())
                    {
                        isIdle = false;
                        isUsingWidgetBench = true;

                        GetTargetPosWidgetBench();
                        GoToward(targetPos);
                    }
                    else if (!IsWidgetBenchAvailable())
                    {
                        isIdle = true;
                    }
                    
                }
                else if (!GameStats.hasWidgetBench)
                {
                    isIdle = true;
                }
            }
           
        }

        else 
        {
            isIdle = true;
        }

        if (!isUsingWidgetBench)
        {

            isWidgetBenchInProgress = false;

            if (!isWidgetBenchInProgress && !needToHaul)
            {
                //print("it ran " + simStatsScript.simName);
                simStatsScript.objectInUse = null;
            }
        }

       

    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


    //NEED METERS DEPLETE PERIODICALLY
    IEnumerator EnergyDeplete(float waitTime)
    {
        for (;;)
        {
            simStatsScript.energy -= 1;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator HungerDeplete(float waitTime)
    {
        for (;;)
        {
            simStatsScript.hunger -= 1;

            simStatsScript.hunger = Mathf.Clamp(simStatsScript.hunger, 0, 100);

            //simManagerScript.simStatsScript.hunger -= 1;
            yield return new WaitForSeconds(waitTime);
        }
    }

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
            fridgeScript = fridge.GetComponent<FridgeScript>();
            mag1 = Vector2.Distance(simStatsScript.simPos, fridgeScript.fridgePos);
            if (mag1 < mag2)
            {
                mag2 = mag1;
                targetPos = fridgeScript.fridgePos;
            }
            
        }
    }
    
    /*public void GetTargetPosWidgetBench()
    {
        GameObject[] widgetBenches = GameObject.FindGameObjectsWithTag("WidgetBench");
        WidgetBenchScript widgetBenchScript;
        //Vector2 simPos = gameObject.transform.position;
        Vector2 testPos = new Vector2(0, 0);
        float mag1 = 0;
        float mag2 = 9999;

        //for each wb in wbarray, compare positions to determine which is closest
        foreach (GameObject widgetBench in widgetBenches)
        {
            widgetBenchScript = widgetBench.GetComponent<WidgetBenchScript>();

            foreach (GameObject sim in GameStats.simList)
            {
                SimStats localScript = sim.GetComponent<SimStats>();
                if (localScript.objectInUse.GetInstanceID() == widgetBench.GetInstanceID())
                {
                    //remove bench from list
                }
            }

            mag1 = Vector2.Distance(simStatsScript.simPos, widgetBenchScript.widgetBenchPos);
            if (mag1 < mag2)
            {
                mag2 = mag1;
                targetPos = widgetBenchScript.widgetBenchUsePos;
            }

        }
    }

    public void */

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
            widgetBenchScript = widgetBench.GetComponent<WidgetBenchScript>();

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

    /*public void GetObjectInUse()
    {
        WidgetBenchScript widgetBenchScript;

        foreach (GameObject widgetBench in GameStats.widgetBenchList)
        {
            widgetBenchScript = widgetBench.GetComponent<WidgetBenchScript>();
            if (targetPos == widgetBenchScript.widgetBenchPos)
            {
                simStatsScript.objectInUse = widgetBench;
            }
        }
    }*/

    public void GetTargetPosHaulDropoff()
    {
        //if there is a zone, go to nearest empty tile in zone

        //else if no zone then drop next to bench
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
        //GetTargetPosWidgetBench();
        targetPos = targetPos + new Vector2(2, 0);
    }
    //---------------------------------------------------------------------------------------------------------------------------

        public void HaulWidget()
    {
        isUsingWidgetBench = false;

        GetTargetPosHaulDropoff();
        GoToward(targetPos);

        //if simPos = targetPos, drop the item
        if (simStatsScript.simPos == targetPos)
        {
            needToHaul = false;
            simStatsScript.itemInPossession = null;
            isHolding = false;
            //isUsingWidgetBench = true;
        }
    }

    public bool IsWidgetBenchAvailable()
    {
        SimAI otherSimAIScript;
        int beingUsed = -1;

        if (GameStats.countWidgetBench >= 0)
        {
            foreach(GameObject otherSimObj in otherSimArray)
            {
                if (otherSimObj != gameObject)
                {
                    otherSimAIScript = otherSimObj.GetComponent<SimAI>();
                    if (otherSimAIScript.isUsingWidgetBench || otherSimAIScript.needToHaul)
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
            if (isGettingFood)
            {
                int hunger = simStatsScript.hunger;
                hunger += 60;
                //ENSURE MAX HUNGER IS 100
                hunger = Mathf.Clamp(hunger, 0, 100);
                //ACTUALLY SET THE NEW HUNGER VALUE 
                simStatsScript.hunger = hunger;

                //TASK COMPLETE
                isGettingFood = false;
                //isIdle = true;
            }
        }

        //WIDGET BENCH IS USED 
       /* if (col.gameObject.tag == "WidgetBench")
        {
            if (isUsingWidgetBench)
            {
                //set inProgress to true
                isWidgetBenchInProgress = true;
            }
        }*/
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //WIDGET BENCH IS USED 
        if (col.gameObject.tag == "WidgetBenchChild")
        {
            if (isUsingWidgetBench)
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
