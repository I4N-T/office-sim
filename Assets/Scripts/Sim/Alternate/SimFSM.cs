using UnityEngine;
using System.Collections;

public class SimFSM : MonoBehaviour {

    //SCRIPTS
    SimAI simAIScript;
    SimStats simStatsScript;

	public enum MainFSM
    {
        Idle,
        Task
    }

    //Default state
    public MainFSM mainState = MainFSM.Idle;

    public enum TaskFSM
    {
        None,
        GettingFood,
        MakingWidget,
        Sales
        
    }

    public TaskFSM taskState;

    void Start()
    {
        //GET SCRIPTS
        simAIScript = gameObject.GetComponent<SimAI>();
        simStatsScript = gameObject.GetComponent<SimStats>();

    }


    void FixedUpdate()
    {
        MainFSMMethod();
    }

    public void MainFSMMethod()
    {
        switch (mainState)
        {
            case MainFSM.Idle:
                simAIScript.IdleWander();
                break;

            case MainFSM.Task:
                TaskStatesMethod();
                break;

        }
    }

    public void TaskStatesMethod()
    {
        switch (taskState)
        {

            case TaskFSM.None:
                break;

            case TaskFSM.GettingFood:
                simAIScript.GetTargetPosFridge();
                simAIScript.GoToward(simAIScript.targetPos);
                break;

            case TaskFSM.MakingWidget:
                if (!simAIScript.needToHaul)
                {
                        simAIScript.GetTargetPosWidgetBench();
                        simAIScript.GoToward(simAIScript.targetPos);  
                }
                else if (simAIScript.needToHaul)
                {
                    simAIScript.HaulWidget();

                }
                break;
            case TaskFSM.Sales:
                simAIScript.GetTargetPosSalesBench();
                simAIScript.GoToward(simAIScript.targetPos);
                break;

            

        }
    }


}
