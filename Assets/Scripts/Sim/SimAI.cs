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

    /* //GUI SCRIPT INSTANCE
     MainGUI guiScript = new MainGUI();*/

    //SIM STATS SCRIPT INSTANCE
    SimStats simStatsScript;

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
        //if no task then IdleWander()
        if (isIdle)
        {
            IdleWander();
        }
        

        //REPLENISH NEEDS IF POSSIBLE

        //ENERGY

        //HUNGER
        if (simStatsScript.hunger < 95)
        {
            //check for food
            if (GameStats.hasFridge == true)
            {
                //set targetPos to frigdePos; must compare position to each fridgePos to find closest fridge 
                isIdle = false;
                isGettingFood = true;
          
                GetTargetPos();
                GoToward(targetPos);
            }
            
        }

        //if in transit then GoToward()
        /*if (inTransit)
        {
            
            GoToward(targetPos);
        }*/
        

      

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

    //IF IN TRANSIT
    public void GoToward(Vector2 targetPos)
    {
        //rb2D.MovePosition(targetPos);
        Vector2 simPos = gameObject.transform.position;
        transform.position = Vector2.MoveTowards(simPos, targetPos, moveSpeed * Time.deltaTime);
    }

    public void GetTargetPos()
    {
        FridgeScript fridgeScript;
        Vector2 simPos = gameObject.transform.position;
        Vector2 testPos = new Vector2(0, 0);
        float mag1 = 0;
        float mag2 = 9999;

        //for each fridge in fridgeArray, compare positions to determine which is closest
        foreach (GameObject fridge in GameStats.fridgeList)
        {
            fridgeScript = fridge.GetComponent<FridgeScript>();
            mag1 = Vector2.Distance(simPos, fridgeScript.fridgePos);
            if (mag1 < mag2)
            {
                mag2 = mag1;
                targetPos = fridgeScript.fridgePos;
            }
            
        }
    }

    //COLLISION LOGIC
    void OnCollisionStay2D(Collision2D col)
    {
        //FRIDGE ADDS 60 TO HUNGER
        if (col.gameObject.name == "fridge")
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
    }

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
