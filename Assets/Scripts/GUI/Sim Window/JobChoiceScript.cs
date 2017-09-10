using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JobChoiceScript : MonoBehaviour {

    SimWindowTextManager simWindowTextScript;
    public Dropdown JobChoiceDropDown;

	// Use this for initialization
	void Start () {
        simWindowTextScript = transform.parent.GetComponent<SimWindowTextManager>();
        JobChoiceDropDown.onValueChanged.AddListener(ValueChangeAction);
	
	}
	
	public void ValueChangeAction(int arg0)
    {
        if (arg0 == 0)
        {
            simWindowTextScript.simStatsScript.jobState = SimStats.SimJobs.Engineer;
            simWindowTextScript.simStatsScript.canEngineer = true;
            simWindowTextScript.simStatsScript.canLabor = false;
            simWindowTextScript.simStatsScript.canSales = false;

            simWindowTextScript.simStatsScript.simJobString = "Engineer";
        }
        if (arg0 == 1)
        {
            simWindowTextScript.simStatsScript.jobState = SimStats.SimJobs.Production;
            simWindowTextScript.simStatsScript.canEngineer = false;
            simWindowTextScript.simStatsScript.canLabor = true;
            simWindowTextScript.simStatsScript.canSales = false;

            simWindowTextScript.simStatsScript.simJobString = "Production";
        }
        if (arg0 == 2)
        {
            simWindowTextScript.simStatsScript.jobState = SimStats.SimJobs.Sales;
            simWindowTextScript.simStatsScript.canEngineer = false;
            simWindowTextScript.simStatsScript.canLabor = false;
            simWindowTextScript.simStatsScript.canSales = true;

            simWindowTextScript.simStatsScript.simJobString = "Sales";
        }
        //set idle here to force a task change
        simWindowTextScript.simFSMScript.mainState = SimFSM.MainFSM.Idle;
        simWindowTextScript.simFSMScript.taskState = SimFSM.TaskFSM.None;
    }
}
