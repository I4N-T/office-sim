using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AltSimManager : MonoBehaviour {

    //SCRIPT INSTANCES
    public SimStats simStatsScript;
    public AltSimAI simAIScript;

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

    bool hasRunDisable;

    // Use this for initialization
    void Start()
    {

        //GET SCRIPT COMPONENTS
        simStatsScript = gameObject.GetComponent<SimStats>();
        simAIScript = gameObject.GetComponent<AltSimAI>();

        //ADD THIS SIM TO THE SIMLIST
        GameStats.simList.Add(gameObject);

        //to test the sim list
        foreach (GameObject sim in GameStats.simList)
        {
            SimStats theScript = sim.GetComponent<SimStats>();
            //print("simList: " + theScript.simName);
        }


        //GET OTHER SIM OBJECTS ARRAY
        //otherSimArray = GameObject.FindGameObjectsWithTag("Sim");

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
    void Update()
    {

        //if Sim is selected then enable text and disable other sims' text
        if (isSimSelected == true)
        {
            //disable other sims' text
            if (!hasRunDisable)
            {
                DisableOtherSimText();
                hasRunDisable = true;
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


       /* if (AltsimAIScript.isIdle)
        {
            simStatusText.text = "idle";
        }
        else if (AltsimAIScript.isIdle == false)
        {
            if (AltsimAIScript.isGettingFood)
            {
                simStatusText.text = "getting food";
            }
            if (AltsimAIScript.isUsingWidgetBench)
            {
                simStatusText.text = "using " + simStatsScript.objectInUse;
            }
            if (AltsimAIScript.needToHaul)
            {
                if (simStatsScript.itemInPossession != null)
                {
                    simStatusText.text = "hauling " + simStatsScript.itemInPossession.name;
                }
            }
        }

        if (AltsimAIScript.isHolding)
        {
            simItemText.text = "Item: " + simStatsScript.itemInPossession.name;
        }
        else if (!AltsimAIScript.isHolding)
        {
            simItemText.text = "Item: ";
        }*/
    }

    void DisableOtherSimText()
    {
        foreach (GameObject otherSimObj in GameStats.simList)
        {
            //if the obj refers to any sim other than this one
            if (otherSimObj != gameObject)
            {

                //disable all text
                AltSimManager otherSimManagerScript = otherSimObj.GetComponent<AltSimManager>();

                otherSimManagerScript.hasRunDisable = false;
                otherSimManagerScript.isSimSelected = false;


            }
        }
    }


    void OnMouseDown()
    {
        isSimSelected = true;
    }
}
