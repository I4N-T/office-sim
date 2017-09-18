using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimWindowTextManager : MonoBehaviour {

    //SCRIPT INSTANCES
    public SimStats simStatsScript;
    public SimAI simAIScript;
    public SimFSM simFSMScript;
    SimManager simManagerScript;

    //ARRAY TO HOLD ALL OTHER SIMS SO THEIR UI CAN BE DISABLED WHEN THIS ONE IS ENABLED
    public GameObject[] otherSimArray;
    //SIM INSTANCE
    public GameObject simObj;

    //GUI STUFF
    GameObject canvasObj;
    public Canvas canvas;

    GameObject simNameTextObj;
    Text simNameText;

    GameObject simJobTextObj;
    Text simJobText;

    GameObject simStatusTextObj;
    Text simStatusText;

    GameObject simEnergyTextObj;
    Text simEnergyText;

    GameObject simHungerTextObj;
    Text simHungerText;

    GameObject simBladderTextObj;
    Text simBladderText;

    GameObject simItemTextObj;
    Text simItemText;

    public Dropdown jobChoiceDropDown;

    public bool isSimSelected;

    bool hasRunDisable;


    // Use this for initialization
    void Start()
    {      

        //GET SCRIPT COMPONENTS
        simStatsScript = simObj.GetComponent<SimStats>();
        simAIScript = simObj.GetComponent<SimAI>();
        simFSMScript = simObj.GetComponent<SimFSM>();    
        
        //GET CANVAS OBJECT
        //canvasObj = transform.GetChild(0).gameObject;

        //GET TEXT OBJECT COMPONENTS
        simNameTextObj = transform.GetChild(1).gameObject;
        //simNameTextObj = GameObject.Find("SimText");

        simJobTextObj = transform.GetChild(2).gameObject;

        simStatusTextObj = transform.GetChild(3).gameObject;
        //simStatusTextObj = GameObject.Find("SimStatusText");

        simItemTextObj = transform.GetChild(4).gameObject;

        simEnergyTextObj = transform.GetChild(5).gameObject;
        //simEnergyTextObj = GameObject.Find("EnergyText");

        simHungerTextObj = transform.GetChild(6).gameObject;
        //simHungerTextObj = GameObject.Find("HungerText");

        simBladderTextObj = transform.GetChild(7).gameObject;
        //simHungerTextObj = GameObject.Find("HungerText");


        //GET ACTUAL TEXT COMPONENTS FROM OBJECTS
        simNameText = simNameTextObj.GetComponent<Text>();
        simStatusText = simStatusTextObj.GetComponent<Text>();
        simJobText = simJobTextObj.GetComponent<Text>();
        simEnergyText = simEnergyTextObj.GetComponent<Text>();
        simHungerText = simHungerTextObj.GetComponent<Text>();
        simBladderText = simBladderTextObj.GetComponent<Text>();
        simItemText = simItemTextObj.GetComponent<Text>();


    }

    // Update is called once per frame
    void Update()
    {
        //update and display this sim's text
        StatsTextUpdate();

       /* if (Input.GetMouseButtonUp(1))
        {
            isSimSelected = false;

        }*/

    }   

    void StatsTextUpdate()
    {
        //print("hunger from script: " + simStatsScript.hunger);
        simNameText.text = "Name: " + simStatsScript.simName;
        simJobText.text = "Job: " + simStatsScript.simJobString;

        simEnergyText.text = "Energy: " + simStatsScript.energy + "/" + "100";
        simHungerText.text = "Hunger: " + simStatsScript.hunger + "/" + "100";
        simBladderText.text = "Bladder: " + simStatsScript.bladder + "/" + "100";


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

            if (simFSMScript.taskState == SimFSM.TaskFSM.GettingCoffee)
            {
                simStatusText.text = "getting coffee";
            }

            if (simFSMScript.taskState == SimFSM.TaskFSM.UsingBathroom)
            {
                simStatusText.text = "leaking";
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

                //otherSimManagerScript.hasRunDisable = false;
                //otherSimManagerScript.isSimSelected = false;


            }
        }
    }
}
