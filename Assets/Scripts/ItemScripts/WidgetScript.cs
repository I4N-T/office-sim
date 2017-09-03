using UnityEngine;
using System.Collections;

public class WidgetScript : MonoBehaviour {

    //WIDGET BOXCOLLIDER
    BoxCollider2D bc2d;

    //POSITION OF WIDGET
    public Vector2 widgetPos;

    //POSITION OF SIM POSSESSING WIDGET
    public Vector2 simPos;
    public GameObject sim;

    //SIMAI SCRIPT
    public SimAI simAIScript;
    public SimStats simStatsScript;

    //QUALITY
    public string widgetQuality;

    void Start()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        widgetPos = gameObject.transform.position;

        //IF ITEM IS POSSESSED, THEN UPDATE widgetPos TO THE POSITION OF THE POSSESSOR SIM
        if (simStatsScript != null)
        {
            if (simStatsScript.itemInPossession != null && simStatsScript.itemInPossession.tag == "Widget")
            {
                CarriedPosition();
            }

        }
        //CarriedPosition();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            sim = col.gameObject;
            simAIScript = col.gameObject.GetComponent<SimAI>();
            simStatsScript = col.gameObject.GetComponent<SimStats>();
        }
    }

    //is this one necessary???
    void OnTriggerStay2D(Collider2D col)
    {
        //SET THIS WIDGET TO BE POSSESSED BY SIM THAT TRIGGERED IT
        if (simAIScript != null && simAIScript.needToHaul && col.gameObject.tag == "Sim")
        {
            simStatsScript.itemInPossession = gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            sim = null;
            simAIScript = null;
            simStatsScript = null;
            bc2d.enabled = false;
        }
    }

    void CarriedPosition()
    {
        widgetPos = sim.transform.position;
        gameObject.transform.position = widgetPos;
    }

}
