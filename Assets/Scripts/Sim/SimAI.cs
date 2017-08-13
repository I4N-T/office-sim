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

    //SIM STATS SCRIPT INSTANCE
    SimStats simStatsScript;

    //ITEM SCRIPT INSTANCES
    //WidgetBenchScript widgetBenchScript;

    //SimManager simManagerScript;


    void Start () {

        isIdle = true;

        simStatsScript = gameObject.GetComponent<SimStats>();
        //simManagerScript = gameObject.GetComponent<SimManager>();

        rb2D = GetComponent<Rigidbody2D>();
        timeLeft = 2f;
        moveVal = Random.Range(1, 9);

        /*//GUI STUFF
        guiScript.simNameText = */

        //NEED DEPLETIONS
        StartCoroutine(EnergyDeplete(5f));
        StartCoroutine(HungerDeplete(3f));
        

    }


    void FixedUpdate()
    {
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
                isGettingFood = true;
          
                GetTargetPosFridge();
                GoToward(targetPos);
            }
            
        }

        //IF NEEDS ARE MET THEN WORK ON HIGHEST PRIORITY TASK POSSIBLE
        if (simStatsScript.hunger >= 40 && simStatsScript.energy >= 30)
        {
            if (simStatsScript.canLabor)
            {
                if (GameStats.hasWidgetBench && needToHaul)
                {
                    HaulWidget();
                }
                if (GameStats.hasWidgetBench && !needToHaul)
                {
                    isIdle = false;
                    isUsingWidgetBench = true;

                    GetTargetPosWidgetBench();
                    GoToward(targetPos);
                }
            }
        }
        
      

    }


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
            mag1 = Vector2.Distance(simStatsScript.simPos, widgetBenchScript.widgetBenchPos);
            if (mag1 < mag2)
            {
                mag2 = mag1;
                targetPos = widgetBenchScript.widgetBenchUsePos;
            }
        }
    }

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
        }
    }


    //COLLISION LOGIC ---------------------------------------------------------------------
    void OnCollisionStay2D(Collision2D col)
    {
        //FRIDGE ADDS 60 TO HUNGER
        if (col.gameObject.tag == "fridge")
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
                isIdle = true;
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
