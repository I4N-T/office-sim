using UnityEngine;
using System.Collections;

public class SimFSM : MonoBehaviour {

    //SCRIPTS
    SimAI simAIScript;
    SimStats simStatsScript;

    public Coroutine cR;

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
        GettingCoffee,
        UsingBathroom,
        MakingWidget,
        Sales,
        Drafting
        
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
        //print("sim: " + simStatsScript.simName + " mainState: " + mainState + " taskState: " + taskState);
    }

    public void MainFSMMethod()
    {
        switch (mainState)
        {
            case MainFSM.Idle:
                simAIScript.IdleWander();
                simStatsScript.objectInUse = null;
                simAIScript.objID = 0;
                if ( simAIScript.simPathfindingScript != null)
                {
                    simAIScript.simPathfindingScript.isStillPathing = false;
                    simAIScript.simPathfindingScript.isRunned = false;
                    if (cR != null)
                    {
                        StopCoroutine(cR);
                    }
                }  
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
                //simAIScript.simPathfindingScript.isGoing = false;

                if (!simAIScript.simPathfindingScript.isTakePathTime)
                {
                    if (!simAIScript.simPathfindingScript.isStillPathing)
                    {
                        simAIScript.GetTargetPosFridge();
                        simAIScript.simPathfindingScript.AStarPathMethod(simAIScript.targetPos);
                    }
                    else if (simAIScript.simPathfindingScript.isStillPathing)
                    {
                        //do nothing
                        simAIScript.simPathfindingScript.isGoing = false;
                    }
                }
                else if (simAIScript.simPathfindingScript.isTakePathTime)
                {
                    simAIScript.simPathfindingScript.isStillPathing = true;
                    cR = StartCoroutine(simAIScript.simPathfindingScript.TakePath(simAIScript.targetPos, "Fridge"));
                    simAIScript.simPathfindingScript.isTakePathTime = false;
                }
                break;

            case TaskFSM.GettingCoffee:
                if (!simAIScript.simPathfindingScript.isTakePathTime)
                {
                    if (!simAIScript.simPathfindingScript.isStillPathing)
                    {
                        simAIScript.GetTargetPosCoffeeMachine();
                        simAIScript.simPathfindingScript.AStarPathMethod(simAIScript.targetPos);
                    }
                    else if (simAIScript.simPathfindingScript.isStillPathing)
                    {
                        //do nothing
                        simAIScript.simPathfindingScript.isGoing = false;
                    }
                }
                else if (simAIScript.simPathfindingScript.isTakePathTime)
                {
                    simAIScript.simPathfindingScript.isStillPathing = true;
                    cR = StartCoroutine(simAIScript.simPathfindingScript.TakePath(simAIScript.targetPos, "CoffeeMachine"));
                    simAIScript.simPathfindingScript.isTakePathTime = false;
                }
                break;

            case TaskFSM.UsingBathroom:
                simAIScript.GetTargetPosBathroomStall();
                simAIScript.GoToward(simAIScript.targetPos);
                break;

            case TaskFSM.MakingWidget:
                if (!simAIScript.needToHaul)
                {
                    if (!simAIScript.simPathfindingScript.isTakePathTime)
                    {
                        if (simAIScript.simPathfindingScript.isGoing)
                        {
                            simAIScript.GoToward(simAIScript.targetPos);
                        }
                        else if (!simAIScript.simPathfindingScript.isGoing)
                        {
                            if (!simAIScript.simPathfindingScript.isStillPathing)
                            {
                                simAIScript.GetTargetPosWidgetBench();
                                simAIScript.simPathfindingScript.AStarPathMethod(simAIScript.targetPos);
                            }
                            else if (simAIScript.simPathfindingScript.isStillPathing)
                            {
                                //do nothing
                            }
                        }
                    }         
                    else if (simAIScript.simPathfindingScript.isTakePathTime)
                    {
                        simAIScript.simPathfindingScript.isStillPathing = true;
                        cR = StartCoroutine(simAIScript.simPathfindingScript.TakePath(simAIScript.targetPos, "WidgetBench"));
                        simAIScript.simPathfindingScript.isTakePathTime = false;   
                    }
                }
                else if (simAIScript.needToHaul)
                {
                    //simAIScript.GetTargetPosHaulDropOff();
                    //
                    simAIScript.HaulWidget();

                }
                break;

            case TaskFSM.Sales:
                if (!simAIScript.simPathfindingScript.isTakePathTime)
                {
                    if (simAIScript.simPathfindingScript.isGoing)
                    {
                        simAIScript.GoToward(simAIScript.targetPos);
                    }
                    else if (!simAIScript.simPathfindingScript.isGoing)
                    {
                        if (!simAIScript.simPathfindingScript.isStillPathing)
                        {
                            simAIScript.GetTargetPosSalesBench();
                            simAIScript.simPathfindingScript.AStarPathMethod(simAIScript.targetPos);
                        }
                        else if (simAIScript.simPathfindingScript.isStillPathing)
                        {
                            //do nothing
                        }
                    }
                }
                else if (simAIScript.simPathfindingScript.isTakePathTime)
                {
                    simAIScript.simPathfindingScript.isStillPathing = true;
                    cR = StartCoroutine(simAIScript.simPathfindingScript.TakePath(simAIScript.targetPos, "SalesBench"));
                    simAIScript.simPathfindingScript.isTakePathTime = false;
                    //simAIScript.simPathfindingScript.isGoing = false;
                }
                break;

            case TaskFSM.Drafting:
                if (!simAIScript.simPathfindingScript.isTakePathTime)
                {
                    if (simAIScript.simPathfindingScript.isGoing)
                    {
                        simAIScript.GoToward(simAIScript.targetPos);
                    }
                    else if (!simAIScript.simPathfindingScript.isGoing)
                    {
                        if (!simAIScript.simPathfindingScript.isStillPathing)
                        {
                            simAIScript.GetTargetPosDraftingDesk();
                            simAIScript.simPathfindingScript.AStarPathMethod(simAIScript.targetPos);
                        }
                        else if (simAIScript.simPathfindingScript.isStillPathing)
                        {
                            //do nothing
                        }
                    }
                }
                else if (simAIScript.simPathfindingScript.isTakePathTime)
                {
                    simAIScript.simPathfindingScript.isStillPathing = true;
                    cR = StartCoroutine(simAIScript.simPathfindingScript.TakePath(simAIScript.targetPos, "DraftingDesk"));
                    simAIScript.simPathfindingScript.isTakePathTime = false;
                    //simAIScript.simPathfindingScript.isGoing = false;
                }
                break;



        }
    }


}
