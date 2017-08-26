using UnityEngine;
using System.Collections;

public class SimFSM : MonoBehaviour {

    //SCRIPTS
    AltSimAI simAIScript;
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
        MakingWidget
        
    }

    public TaskFSM taskState;

    void Start()
    {
        //GET SCRIPTS
        simAIScript = gameObject.GetComponent<AltSimAI>();
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
                    if (simAIScript.IsWidgetBenchAvailable())
                    {
                        //simAIScript.isUsingWidgetBench = true;

                        simAIScript.GetTargetPosWidgetBench();
                        simAIScript.GoToward(simAIScript.targetPos);
                    }
                    else if (!simAIScript.IsWidgetBenchAvailable())
                    {
                        taskState = TaskFSM.None;
                        mainState = MainFSM.Idle;
                    }
                }
                else if (simAIScript.needToHaul)
                {
                    simAIScript.HaulWidget();
                    //this next line is needed to prevent another sim from going isUsingWidgetBench = true while this sim changes it's needtohaul bool
                    //simAIScript.isUsingWidgetBench = true;
                }

                break;

            

        }
    }


}
