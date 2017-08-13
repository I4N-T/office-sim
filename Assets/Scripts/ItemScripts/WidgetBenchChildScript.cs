using UnityEngine;
using System.Collections;

public class WidgetBenchChildScript : MonoBehaviour {

    public WidgetBenchScript widgetBenchScript; 


    void Start()
    {
        widgetBenchScript = transform.parent.GetComponent<WidgetBenchScript>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            widgetBenchScript.simAIScript = col.gameObject.GetComponent<SimAI>();
            //inProgress = true;
        }
    }

}
