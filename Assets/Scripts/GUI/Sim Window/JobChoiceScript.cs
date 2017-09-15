using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JobChoiceScript : MonoBehaviour {

    SimWindowTextManager simWindowTextScript;
    public Dropdown JobChoiceDropDown;

    bool menuUsed;

	// Use this for initialization
	void Start () {
        simWindowTextScript = transform.parent.GetComponent<SimWindowTextManager>();
        JobChoiceDropDown.onValueChanged.AddListener(ValueChangeAction);
        //menuUsed = false;
	
	}
	
	public void ValueChangeAction(int arg0)
    {
        if (menuUsed)
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
        }

        else if (!menuUsed)
        {
            JobChoiceDropDown.options.RemoveAt(0);
            if (arg0 == 1)
            {
                simWindowTextScript.simStatsScript.jobState = SimStats.SimJobs.Engineer;
                simWindowTextScript.simStatsScript.canEngineer = true;
                simWindowTextScript.simStatsScript.canLabor = false;
                simWindowTextScript.simStatsScript.canSales = false;

                simWindowTextScript.simStatsScript.simJobString = "Engineer";
                menuUsed = true;
                JobChoiceDropDown.value = 0;
                
            }
            if (arg0 == 2)
            {
                
                simWindowTextScript.simStatsScript.jobState = SimStats.SimJobs.Production;
                simWindowTextScript.simStatsScript.canEngineer = false;
                simWindowTextScript.simStatsScript.canLabor = true;
                simWindowTextScript.simStatsScript.canSales = false;

                simWindowTextScript.simStatsScript.simJobString = "Production";
                menuUsed = true;
                JobChoiceDropDown.value = 1;
                
            }
            if (arg0 == 3)
            {
                simWindowTextScript.simStatsScript.jobState = SimStats.SimJobs.Sales;
                simWindowTextScript.simStatsScript.canEngineer = false;
                simWindowTextScript.simStatsScript.canLabor = false;
                simWindowTextScript.simStatsScript.canSales = true;

                simWindowTextScript.simStatsScript.simJobString = "Sales";
                menuUsed = true;
                JobChoiceDropDown.value = 2;
                
            }

            
            //JobChoiceDropDown.value -= 1;
            
        }

        
       
        //set idle here to force a task change
        simWindowTextScript.simFSMScript.mainState = SimFSM.MainFSM.Idle;
        simWindowTextScript.simFSMScript.taskState = SimFSM.TaskFSM.None;
    }
}
