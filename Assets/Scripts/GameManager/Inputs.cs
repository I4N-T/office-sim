using UnityEngine;
using System.Collections;
using System.Linq;

public class Inputs : MonoBehaviour {

    //NOTE: SCRIPT FOR BENCH, ETC. HOVERING OVER TILES BEFORE PLACEMENT IS CONTAINED IN TileBehavior.cs

    //MOUSE POSITION
    Vector3 mousePosition;
    Vector3 panelRectPos;


    //RECTTRANSFORMS FOR KEEPING MOUSE CLICK FROM GOING THROUGH BUTTONS; USEFUL FOR OBJECT PLACEMENT
    public RectTransform needsPanelRect;
    public RectTransform productionPanelRect;
    public GameObject buttonCanv;

    //bools for placement
    public static bool placingWidgetBench = false;
    public static bool placingFridge = false;
    public static bool placingSalesBench = false;
    //bool isMouseOverUI;

    //BOOLS FOR ZONE DRAWING
    public static bool drawingZoneStockpile;

    //RECT FOR ZONE DRAWING
    public Rect zoneRect;
    public static bool firstCornerDone;

    public GameObject stockpileZoneObj;
    SpriteRenderer stockpileZoneObjRend;
    RectTransform stockpileZoneObjRectTrans;

    //BUILD STRUCTURES OBJECTS
    public GameObject widgetBenchObject;
    public GameObject fridgeObject;
    public GameObject salesBenchObject;

	// Use this for initialization
	void Start () {
    
       

	}
	
	// Update is called once per frame
	void Update () {     
        

        //UPDATE ISMOUSEOVERUI
        IsMouseOverUI();

            //CONSTANTLY UPDATE MOUSEPOSITION; try using instance of mouseposition script as argument to this method
            MousePositionGet();

        if (drawingZoneStockpile == true)
        {
            if (firstCornerDone)
            {
                zoneRect.max = mousePosition;
            }
        }

        //CANCEL PLACEMENT IF RIGHT CLICK
        if (Input.GetMouseButtonUp(1))
        {
            placingWidgetBench = false;
            placingFridge = false;
            placingSalesBench = false;
            //placingwhatever = false;

            if (drawingZoneStockpile && firstCornerDone)
            {
                firstCornerDone = false;
                drawingZoneStockpile = false;
                Destroy(GameStats.stockpileZoneList.Last());
            }

            drawingZoneStockpile = false;

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
        //widget bench
        if (placingWidgetBench == true && IsMouseOverUI() == false)
        {

            if (Input.GetMouseButtonUp(0) && GUIUtility.hotControl == 0)
            {
                GameStats.dollars -= 300;
                Instantiate(widgetBenchObject, mousePosition, Quaternion.identity);
                placingWidgetBench = false;
               
            }
        }

        //fridge
        if (placingFridge == true && IsMouseOverUI() == false)
        {
            if (Input.GetMouseButtonUp(0) && GUIUtility.hotControl == 0)
            {
                GameStats.dollars -= 200;
                Instantiate(fridgeObject, mousePosition, Quaternion.identity);
                placingFridge = false;
            }
        }

        //sales bench
        if (placingSalesBench == true && IsMouseOverUI() == false)
        {

            if (Input.GetMouseButtonUp(0) && GUIUtility.hotControl == 0)
            {
                GameStats.dollars -= 300;
                Instantiate(salesBenchObject, mousePosition, Quaternion.identity);
                placingSalesBench = false;

            }
        }

        //STOCKPILE ZONE
        if (drawingZoneStockpile == true && IsMouseOverUI() == false)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (!firstCornerDone)
                {
                    //zoneRectCornerStart = mousePosition;
                    zoneRect.min = mousePosition;
                    GameObject stockpileZoneNew = Instantiate(stockpileZoneObj, zoneRect.min, Quaternion.identity) as GameObject;
                    GameStats.stockpileZoneList.Add(stockpileZoneNew);
                    firstCornerDone = true;   
                }
                else if (firstCornerDone)
                {
                    //zoneRect.max = mousePosition;

                    GameStats.countStockpileZone++;
                    firstCornerDone = false;
                    drawingZoneStockpile = false;
                }
                
            }
        }
	}


    void MousePositionGet()
    {
        float mousex = Mathf.Round(MousePosition.mouseposition.x);
        float mousey = Mathf.Round(MousePosition.mouseposition.y);
        mousePosition = new Vector3(mousex, mousey, -1);  
    }

    bool IsMouseOverUI()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(productionPanelRect, Input.mousePosition) || RectTransformUtility.RectangleContainsScreenPoint(needsPanelRect, Input.mousePosition))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
