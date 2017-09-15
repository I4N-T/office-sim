using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SimManager : MonoBehaviour {

    //SCRIPT INSTANCES
    public SimStats simStatsScript;
    public SimAI simAIScript;
    public SimFSM simFSMScript;

    //ARRAY TO HOLD ALL OTHER SIMS SO THEIR UI CAN BE DISABLED WHEN THIS ONE IS ENABLED
    public GameObject[] otherSimArray;

    //GUI STUFF
    GameObject canvasObj;
    public Canvas canvas;

    GameObject simNameTextObj;
    public Text simNameText;

    GameObject simStatusTextObj;
    public Text simStatusText;

    GameObject simEnergyTextObj;
    public Text simEnergyText;

    GameObject simHungerTextObj;
    public Text simHungerText;

    GameObject simItemTextObj;
    public Text simItemText;

    public bool isSimSelected;

    public bool hasRunDisable;

    //SIM WINDOW STUFF
    public GameObject simWindowCanvasPrefab;
    public GameObject simWindowCanvasObj;
    GameObject simWindowObj;
    public SimWindowTextManager simWindowTextManagerScript;


    void Start()
    {

        simWindowCanvasPrefab = (GameObject)Resources.Load("Prefabs/SimWindowCanvas");

        //GET SCRIPT COMPONENTS
        simStatsScript = gameObject.GetComponent<SimStats>();
        simAIScript = gameObject.GetComponent<SimAI>();
        simFSMScript = gameObject.GetComponent<SimFSM>();

        //ADD THIS SIM TO THE SIMLIST
        GameStats.simList.Add(gameObject);



        //GET OTHER SIM OBJECTS ARRAY
        //otherSimArray = GameObject.FindGameObjectsWithTag("Sim");

        //GET CANVAS OBJECT
        /*canvasObj = transform.GetChild(0).gameObject;

        //GET TEXT OBJECT COMPONENTS
        simNameTextObj = transform.GetChild(0).GetChild(0).gameObject;
        //simNameTextObj = GameObject.Find("SimText");

        simStatusTextObj = transform.GetChild(0).GetChild(1).gameObject;
        //simStatusTextObj = GameObject.Find("SimStatusText");

        simEnergyTextObj = transform.GetChild(0).GetChild(2).gameObject;
        //simEnergyTextObj = GameObject.Find("EnergyText");

        simHungerTextObj = transform.GetChild(0).GetChild(3).gameObject;
        //simHungerTextObj = GameObject.Find("HungerText");

        simItemTextObj = transform.GetChild(0).GetChild(4).gameObject;
        

        //GET ACTUAL CANVAS FROM CANVAS OBJECT
        canvas = canvasObj.GetComponent<Canvas>();

        //SET CANVAS RENDER CAMERA
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;

        //GET ACTUAL TEXT COMPONENTS FROM OBJECTS
        simNameText = simNameTextObj.GetComponent<Text>();
        simStatusText = simStatusTextObj.GetComponent<Text>();
        simEnergyText = simEnergyTextObj.GetComponent<Text>();
        simHungerText = simHungerTextObj.GetComponent<Text>();
        simItemText = simItemTextObj.GetComponent<Text>();*/

        //INITIALIZATION OF NEEDS
        simStatsScript.energy = 100;
        simStatsScript.hunger = 100;
        simStatsScript.bladder = 100;

        /*simNameText.enabled = false;
        //simNameText.text = "Name: " + simStatsScript.simName;

        simEnergyText.enabled = false;
        simHungerText.enabled = false;
        //simHungerText.text = "Hunger: " + simStatsScript.hunger + "/" + "100";

        simItemText.enabled = false;*/

        //Set the job of this sim 
        SetSimJob();



    }

    void Update()
    {
        

        //if Sim is selected then enable text and disable other sims' text
        if (isSimSelected == true)
        {
            //disable other sims' text
            if (!hasRunDisable)
            {
                //only instantiate if window does not already exist
                if (simWindowCanvasObj == null)
                {
                    //instantiate window
                    simWindowCanvasObj = Instantiate(simWindowCanvasPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                    //set simwindowtextmanager's sim object to this sim
                    simWindowTextManagerScript = simWindowCanvasObj.GetComponentInChildren<SimWindowTextManager>();
                    simWindowTextManagerScript.simObj = gameObject;                
                }
                DisableOtherSimText();
                hasRunDisable = true;

            }

            

            //update and display this sim's text
            //StatsTextUpdate();

            /*simNameText.enabled = true;
            simEnergyText.enabled = true;
            simHungerText.enabled = true;
            simStatusText.enabled = true;
            simItemText.enabled = true;*/
        }
       /* else if (isSimSelected == false)
        {
            simNameText.enabled = false;
            simEnergyText.enabled = false;
            simHungerText.enabled = false;
            simStatusText.enabled = false;
            simItemText.enabled = false;
        }*/



        if (Input.GetMouseButtonUp(1))
        {
            isSimSelected = false;

        }

    }

    /*void StatsTextUpdate()
    {
        //print("hunger from script: " + simStatsScript.hunger);
        simNameText.text = "Name: " + simStatsScript.simName;
        simEnergyText.text = "Energy: " + simStatsScript.energy + "/" + "100";
        simHungerText.text = "Hunger: " + simStatsScript.hunger + "/" + "100";


         if (simFSMScript.mainState == SimFSM.MainFSM.Idle)
         {
             simStatusText.text = "idle";
         }
         else if (simFSMScript.mainState == SimFSM.MainFSM.Task)
         {
             if (simFSMScript.taskState == SimFSM.TaskFSM.GettingFood)
             {
                 simStatusText.text = "getting food";
             }
             if (simFSMScript.taskState == SimFSM.TaskFSM.MakingWidget && simAIScript.needToHaul == false)
             {
                 simStatusText.text = "Making Widget";
                if (simStatsScript.objectInUse != null)
                {
                    simStatusText.text = "Making Widget at " + simStatsScript.objectInUse;
                }
             }
             if (simFSMScript.taskState == SimFSM.TaskFSM.MakingWidget && simAIScript.needToHaul == true)
             {
                 if (simStatsScript.itemInPossession != null)
                 {
                     simStatusText.text = "hauling " + simStatsScript.itemInPossession.name;
                 }
             }
            if (simFSMScript.taskState == SimFSM.TaskFSM.Sales)
            {
                simStatusText.text = "Making cold calls";
                if (simStatsScript.objectInUse != null)
                {
                    simStatusText.text = "Making cold calls at " + simStatsScript.objectInUse;
                }
            }
        }

         if (simStatsScript.itemInPossession != null)
         {
             simItemText.text = "Item: " + simStatsScript.itemInPossession.name;
         }
         else if (simStatsScript.itemInPossession == null)
         {
             simItemText.text = "Item: ";
         }
    }*/

    void DisableOtherSimText()
    {
        foreach (GameObject otherSimObj in GameStats.simList)
        {
            //if the obj refers to any sim other than this one
            if (otherSimObj != gameObject)
            {

                //disable all text
                SimManager otherSimManagerScript = otherSimObj.GetComponent<SimManager>();

                otherSimManagerScript.hasRunDisable = false;
                otherSimManagerScript.isSimSelected = false;


            }
        }
    }

    //eventually make job titles based on skill level - junior engineer, senior sales proffessional, etc.
    void SetSimJob()
    {
        switch (simStatsScript.jobState)
        {
            case SimStats.SimJobs.Engineer:
                simStatsScript.canEngineer = true;
                simStatsScript.canLabor = false;
                simStatsScript.canSales = false;

                simStatsScript.simJobString = "Engineer";
                break;

            case SimStats.SimJobs.Production:
                simStatsScript.canEngineer = false;
                simStatsScript.canLabor = true;
                simStatsScript.canSales = false;

                simStatsScript.simJobString = "Production";
                break;

            case SimStats.SimJobs.Sales:
                print("sales then");
                simStatsScript.canEngineer = false;
                simStatsScript.canLabor = false;
                simStatsScript.canSales = true;

                simStatsScript.simJobString = "Sales";
                break;

        }


        /*if (simStatsScript.canEngineer)
        {
            //simStatsScript.canLabor = false;
            //simStatsScript.canSales = false;
            simStatsScript.simJob = "Engineer";
        }
        else if (simStatsScript.canLabor)
        {
            //simStatsScript.canEngineer = false;
            //simStatsScript.canSales = false;
            simStatsScript.simJob = "Production";
        }
        else if (simStatsScript.canSales)
        {
            //simStatsScript.canEngineer = false;
            //simStatsScript.canLabor = false;
            simStatsScript.simJob = "Sales";
        }*/
    }


    void OnMouseDown()
    {
        isSimSelected = true;
    }


}
