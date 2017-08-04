using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

    //MOUSE POSITION
    Vector3 mousePosition;

    //bools
    public static bool placingWidgetBench = false;

    //BUILD STRUCTURES OBJECTS
    public GameObject widgetBenchObject;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //CONSTANTLY UPDATE MOUSEPOSITION; try using instance of moustposition script as argument to this method
        MousePositionGet();

        //CANCEL PLACEMENT IF RIGHT CLICK
        if (Input.GetMouseButtonUp(1))
        {
            placingWidgetBench = false;
            //placingwhatever = false;
        }

        //TOGGLE BUILD WIDGET BENCH
        //CHECK DOLLARS
        if (GameStats.dollars >= 300)
        {
            if (Input.GetKeyUp(KeyCode.B))
            {
                placingWidgetBench = true;
            }
        }

        //PURCHASE AND BUILD WITH LEFT CLICK
        if (placingWidgetBench == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                GameStats.dollars -= 300;
                Instantiate(widgetBenchObject, mousePosition, Quaternion.identity);
                placingWidgetBench = false;
            }
        }
	}

    void MousePositionGet()
    {
        float mousex = Mathf.Round(MousePosition.mouseposition.x);
        float mousey = Mathf.Round(MousePosition.mouseposition.y);
        mousePosition = new Vector3(mousex, mousey, 1);
    }
}
