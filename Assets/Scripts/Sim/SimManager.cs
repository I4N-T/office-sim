using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SimManager : MonoBehaviour {

    //SCRIPT INSTANCES
    public SimStats simStatsScript;
    public SimAI simAIScript;

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

    bool hasRun;
    //public static bool isSimNotSelected = true;

    int hungerHere;

    // Use this for initialization
    void Start () {

        //GET SCRIPT COMPONENTS
        simStatsScript = gameObject.GetComponent<SimStats>();
        simAIScript = gameObject.GetComponent<SimAI>();

        //GET OTHER SIM OBJECTS ARRAY
        otherSimArray = GameObject.FindGameObjectsWithTag("Sim");

        //GET CANVAS OBJECT
        canvasObj = transform.GetChild(0).gameObject;

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
        simItemText = simItemTextObj.GetComponent<Text>();

        //INITIALIZATION OF NEEDS
        simStatsScript.energy = 100;
        simStatsScript.hunger = 100;

        simNameText.enabled = false;
        //simNameText.text = "Name: " + simStatsScript.simName;

        simEnergyText.enabled = false;
        simHungerText.enabled = false;
        //simHungerText.text = "Hunger: " + simStatsScript.hunger + "/" + "100";

        simItemText.enabled = false;      

	}
	
	// Update is called once per frame
	void Update () {

        //if Sim is selected then enable text and disable other sims' text
        if (isSimSelected == true)
        {
            //disable other sims' text
            if (!hasRun)
            {
                DisableOtherSimText();
                hasRun = true;
            }
            

            //update and display this sim's text
            StatsTextUpdate();

            simNameText.enabled = true;
            simEnergyText.enabled = true;
            simHungerText.enabled = true;
            simStatusText.enabled = true;
            simItemText.enabled = true;
        }
        else if (isSimSelected == false)
        {
            simNameText.enabled = false;
            simEnergyText.enabled = false;
            simHungerText.enabled = false;
            simStatusText.enabled = false;
            simItemText.enabled = false;
        }

        

        if (Input.GetMouseButtonUp(1))
        {
            isSimSelected = false;
            
        }

    }

    void StatsTextUpdate()
    {
        //print("hunger from script: " + simStatsScript.hunger);
        simNameText.text = "Name: " + simStatsScript.simName;
        simEnergyText.text = "Energy: " + simStatsScript.energy + "/" + "100";
        simHungerText.text = "Hunger: " + simStatsScript.hunger + "/" + "100";
        

        if (simAIScript.isIdle)
        {
            simStatusText.text = "idle";
        }
        else if (simAIScript.isIdle == false)
        {
            if (simAIScript.isGettingFood)
            {
                simStatusText.text = "getting food";
            }
            if (simAIScript.isUsingWidgetBench)
            {
                simStatusText.text = "using widget bench";
            }
            if (simAIScript.needToHaul)
            {
                if (simStatsScript.itemInPossession != null)
                {
                    simStatusText.text = "hauling " + simStatsScript.itemInPossession.name;
                }              
            }
        }

        if (simAIScript.isHolding)
        {
            simItemText.text = "Item: " + simStatsScript.itemInPossession.name;
        }
        else if (!simAIScript.isHolding)
        {
            simItemText.text = "Item: ";
        }
    }

    void DisableOtherSimText()
    {
        foreach (GameObject otherSimObj in otherSimArray)
        {
            //if the obj refers to any sim other than this one
            if (otherSimObj != gameObject)
            {

                //disable all text
                SimManager otherSimManagerScript = otherSimObj.GetComponent<SimManager>();

                otherSimManagerScript.hasRun = false;
                otherSimManagerScript.isSimSelected = false;
                print(otherSimManagerScript.name);

               /* otherSimManagerScript.simNameText.enabled = false;
                otherSimManagerScript.simEnergyText.enabled = false;
                otherSimManagerScript.simHungerText.enabled = false;
                otherSimManagerScript.simStatusText.enabled = false;
                otherSimManagerScript.simItemText.enabled = false;*/
            }
        }
    }


    void OnMouseDown()
    {
        print("dickeradoogle");
        isSimSelected = true;
    }


}
