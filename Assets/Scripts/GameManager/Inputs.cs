using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

    //NOTE: SCRIPT FOR BENCH, ETC. HOVERING OVER TILES BEFORE PLACEMENT IS CONTAINED IN TileBehavior.cs

    //MOUSE POSITION
    Vector3 mousePosition;
    Vector3 panelRectPos;


    //RECTTRANSFORMS FOR KEEPING MOUSE CLICK FROM GOING THROUGH BUTTONS; USEFUL FOR OBJECT PLACEMENT
    public RectTransform productionPanelRect;
    public GameObject buttonCanv;

    //bools
    public static bool placingWidgetBench = false;
    //bool isMouseOverUI;

    //BUILD STRUCTURES OBJECTS
    public GameObject widgetBenchObject;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //UPDATE ISMOUSEOVERUI
        IsMouseOverUI();

            //CONSTANTLY UPDATE MOUSEPOSITION; try using instance of mouseposition script as argument to this method
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
        if (placingWidgetBench == true && IsMouseOverUI() == false)
        {

            if (Input.GetMouseButtonUp(0) && GUIUtility.hotControl == 0)
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

    bool IsMouseOverUI()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(productionPanelRect, Input.mousePosition))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
