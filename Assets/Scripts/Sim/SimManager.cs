using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimManager : MonoBehaviour {

    //SCRIPT INSTANCES
    public SimStats simStatsScript;

    //GUI STUFF
    public Text simNameText;
    public Text simEnergyText;
    public Text simHungerText;
    public bool isSimSelected = false;

    int hungerHere;

    // Use this for initialization
    void Start () {

        simStatsScript = gameObject.GetComponent<SimStats>();

        //INITIALIZATION OF NEEDS
        simStatsScript.energy = 100;
        simStatsScript.hunger = 100;

        simNameText.enabled = false;
        //simNameText.text = "Name: " + simStatsScript.simName;

        simEnergyText.enabled = false;
        simHungerText.enabled = false;
        //simHungerText.text = "Hunger: " + simStatsScript.hunger + "/" + "100";
        

	}
	
	// Update is called once per frame
	void Update () {

        StatsTextUpdate();

        //if Testy is selected then simNameText.text = SimStats(instance of the selected sim).simName
        if (isSimSelected == true)
        {
            simNameText.enabled = true;
            simEnergyText.enabled = true;
            simHungerText.enabled = true;
        }
        else if (isSimSelected == false)
        {
            simNameText.enabled = false;
            simEnergyText.enabled = false;
            simHungerText.enabled = false;
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
    }


    void OnMouseDown()
    {
        print("dickeradoogle");
        isSimSelected = true;
    }


}
