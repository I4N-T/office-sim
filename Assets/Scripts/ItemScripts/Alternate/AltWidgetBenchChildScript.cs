using UnityEngine;
using System.Collections;

public class AltWidgetBenchChildScript : MonoBehaviour {

    public AltWidgetBenchScript widgetBenchScript;


    void Start()
    {
        widgetBenchScript = transform.parent.GetComponent<AltWidgetBenchScript>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            widgetBenchScript.simAIScript = col.gameObject.GetComponent<AltSimAI>();
            //widgetBenchScript.isOccupied = true;
            //inProgress = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            widgetBenchScript.simAIScript.isWidgetBenchInProgress = false;
        }
    }
}
