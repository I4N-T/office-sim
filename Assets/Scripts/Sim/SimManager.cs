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

        //INITIALIZATION OF EXPERIENCE
        simStatsScript.engineeringExp = 0;
        simStatsScript.laborExp = 0;
        simStatsScript.salesExp = 0;

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
        //Update sim skill levels
        SkillLevelsMethod();
        print(simStatsScript.laborExp);

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
        }

        if (Input.GetMouseButtonUp(1))
        {
            isSimSelected = false;

        }

    }

    void SkillLevelsMethod()
    {
        //ENGINEERING
        if (simStatsScript.engineeringExp < 100)
        {
            simStatsScript.engineering = 1;
        }
        else if (simStatsScript.engineeringExp >= 100 && simStatsScript.engineeringExp < 300)
        {
            simStatsScript.engineering = 2;
        }
        else if (simStatsScript.engineeringExp >= 300 && simStatsScript.engineeringExp < 800)
        {
            simStatsScript.engineering = 3;
        }
        else if (simStatsScript.engineeringExp >= 800 && simStatsScript.engineeringExp < 1600)
        {
            simStatsScript.engineering = 4;
        }
        else if (simStatsScript.engineeringExp >= 1600 && simStatsScript.engineeringExp < 2800)
        {
            simStatsScript.engineering = 5;
        }
        else if (simStatsScript.engineeringExp >= 2800 && simStatsScript.engineeringExp < 4200)
        {
            simStatsScript.engineering = 6;
        }
        else if (simStatsScript.engineeringExp >= 4200 && simStatsScript.engineeringExp < 6000)
        {
            simStatsScript.engineering = 7;
        }
        else if (simStatsScript.engineeringExp >= 6000 && simStatsScript.engineeringExp < 8000)
        {
            simStatsScript.engineering = 8;
        }
        else if (simStatsScript.engineeringExp >= 8000 && simStatsScript.engineeringExp < 10300)
        {
            simStatsScript.engineering = 9;
        }
        else if (simStatsScript.engineeringExp >= 10300)
        {
            simStatsScript.engineering = 10;
        }

        //PRODUCTION
        if (simStatsScript.laborExp < 100)
        {
            simStatsScript.labor = 1;
        }
        else if (simStatsScript.laborExp >= 100 && simStatsScript.laborExp < 300)
        {
            simStatsScript.labor = 2;
        }
        else if (simStatsScript.laborExp >= 300 && simStatsScript.laborExp < 800)
        {
            simStatsScript.labor = 3;
        }
        else if (simStatsScript.laborExp >= 800 && simStatsScript.laborExp < 1600)
        {
            simStatsScript.labor = 4;
        }
        else if (simStatsScript.laborExp >= 1600 && simStatsScript.laborExp < 2800)
        {
            simStatsScript.labor = 5;
        }
        else if (simStatsScript.laborExp >= 2800 && simStatsScript.laborExp < 4200)
        {
            simStatsScript.labor = 6;
        }
        else if (simStatsScript.laborExp >= 4200 && simStatsScript.laborExp < 6000)
        {
            simStatsScript.labor = 7;
        }
        else if (simStatsScript.laborExp >= 6000 && simStatsScript.laborExp < 8000)
        {
            simStatsScript.labor = 8;
        }
        else if (simStatsScript.laborExp >= 8000 && simStatsScript.laborExp < 10300)
        {
            simStatsScript.labor = 9;
        }
        else if (simStatsScript.laborExp >= 10300)
        {
            simStatsScript.labor = 10;
        }

        //SALES
        if (simStatsScript.salesExp < 100)
        {
            simStatsScript.sales = 1;
        }
        else if (simStatsScript.salesExp >= 100 && simStatsScript.salesExp < 300)
        {
            simStatsScript.sales = 2;
        }
        else if (simStatsScript.salesExp >= 300 && simStatsScript.salesExp < 800)
        {
            simStatsScript.sales = 3;
        }
        else if (simStatsScript.salesExp >= 800 && simStatsScript.salesExp < 1600)
        {
            simStatsScript.sales = 4;
        }
        else if (simStatsScript.salesExp >= 1600 && simStatsScript.salesExp < 2800)
        {
            simStatsScript.sales = 5;
        }
        else if (simStatsScript.salesExp >= 2800 && simStatsScript.salesExp < 4200)
        {
            simStatsScript.sales = 6;
        }
        else if (simStatsScript.salesExp >= 4200 && simStatsScript.salesExp < 6000)
        {
            simStatsScript.sales = 7;
        }
        else if (simStatsScript.salesExp >= 6000 && simStatsScript.salesExp < 8000)
        {
            simStatsScript.sales = 8;
        }
        else if (simStatsScript.salesExp >= 8000 && simStatsScript.salesExp < 10300)
        {
            simStatsScript.sales = 9;
        }
        else if (simStatsScript.salesExp >= 10300)
        {
            simStatsScript.sales = 10;
        }
    }

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
                simStatsScript.canEngineer = false;
                simStatsScript.canLabor = false;
                simStatsScript.canSales = true;

                simStatsScript.simJobString = "Sales";
                break;

        }

    }


    void OnMouseDown()
    {
        isSimSelected = true;
    }


}
