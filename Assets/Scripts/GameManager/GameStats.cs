using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStats : MonoBehaviour {

    //RESOURCES AND MONEY
    public static int dollars;
    public static int stone;

    public static bool hasWidgetInStockpile;
    public static int countWidgetInStockpile;

    //ITEM/STRUCTURE BOOLEANS and COUNTS
    public static bool hasFridge;
    public static int countFridge;

    public static bool hasCoffeeMachine;
    public static int countCoffeeMachine;

    public static bool hasBathroomStall;
    public static int countBathroomStall;

    public static bool hasWidgetBench;
    public static int countWidgetBench;

    public static bool hasSalesBench;
    public static int countSalesBench;

    public static bool hasDraftingDesk;
    public static int countDraftingDesk;

    //ZONE BOOLEANS and COUNTS
    public static bool hasStockpileZone;
    public static int countStockpileZone;

    //ITEM/STRUCTURE ARRAYS -- used, for example, to get item position in the SimAI script
    public static List<GameObject> fridgeList = new List<GameObject>();
    public static List<GameObject> coffeeMachineList = new List<GameObject>();
    public static List<GameObject> bathroomStallList = new List<GameObject>();
    public static List<GameObject> widgetBenchList = new List<GameObject>();
    public static List<GameObject> salesBenchList = new List<GameObject>();
    public static List<GameObject> draftingDeskList = new List<GameObject>();

    public static List<GameObject> widgetList = new List<GameObject>();

    //SIM LIST - it's a list of all sims
    public static List<GameObject> simList = new List<GameObject>();

    //STOCKPILE ZONE LIST
    public static List<GameObject> stockpileZoneList = new List<GameObject>();

    //TILE LIST
    public static List<GameObject> tileList = new List<GameObject>();

    //NODE LIST (for pathfinding)
    public static List<Node> nodeList = new List<Node>();

    //OTHER
    public static int widgetDesignLevel;

    void Awake()
    {
        countFridge = -1;
        countCoffeeMachine = -1;
        countBathroomStall = -1;
        countWidgetBench = -1;
        countSalesBench = -1;
        countDraftingDesk = -1;

        countStockpileZone = -1;

        countWidgetInStockpile = -1;

        widgetDesignLevel = 1;
    }

	// Use this for initialization
	void Start () {

        dollars = 3000;
        stone = 100;

        hasFridge = false;
        hasCoffeeMachine = false;
        hasBathroomStall = false;

        hasWidgetBench = false;
        hasSalesBench = false;
        hasDraftingDesk = false;

        hasStockpileZone = false;

        hasWidgetInStockpile = false;
	    
	}
	
    void Update()
    {

        for (var i = fridgeList.Count - 1; i > -1; i--)
        {
            if (fridgeList[i] == null)
            {
                fridgeList.RemoveAt(i);
                countFridge -= 1;
            }
                
        }
        if (countFridge < 0)
        {
            hasFridge = false;
        }

        for (var i = coffeeMachineList.Count - 1; i > -1; i--)
        {
            if (coffeeMachineList[i] == null)
            {
                coffeeMachineList.RemoveAt(i);
                countCoffeeMachine -= 1;
            }

        }
        if (countCoffeeMachine < 0)
        {
            hasCoffeeMachine = false;
        }

        for (var i = bathroomStallList.Count - 1; i > -1; i--)
        {
            if (bathroomStallList[i] == null)
            {
                bathroomStallList.RemoveAt(i);
                countBathroomStall -= 1;
            }

        }
        if (countBathroomStall < 0)
        {
            hasBathroomStall = false;
        }

        for (var i = widgetBenchList.Count - 1; i > -1; i--)
        {
            if (widgetBenchList[i] == null)
            {
                widgetBenchList.RemoveAt(i);
                countWidgetBench -= 1;
            }
                
        }
        if (countWidgetBench < 0)
        {
            hasWidgetBench = false;
        }

        for (var i = salesBenchList.Count - 1; i > -1; i--)
        {
            if (salesBenchList[i] == null)
            {
                salesBenchList.RemoveAt(i);
                countSalesBench -= 1;
            }

        }
        if (countSalesBench < 0)
        {
            hasSalesBench = false;
        }
        else if (countSalesBench >= 0)
        {
            hasSalesBench = true;
        }

        for (var i = draftingDeskList.Count - 1; i > -1; i--)
        {
            if (draftingDeskList[i] == null)
            {
                draftingDeskList.RemoveAt(i);
                countDraftingDesk -= 1;
            }

        }
        if (countDraftingDesk < 0)
        {
            hasDraftingDesk = false;
        }
        else if (countDraftingDesk >= 0)
        {
            hasDraftingDesk = true;
        }

        //print("zone count: " + countStockpileZone);
        for (var i = stockpileZoneList.Count - 1; i > -1; i--)
        {
            if (stockpileZoneList[i] == null)
            {
                stockpileZoneList.RemoveAt(i);
                countStockpileZone = Mathf.Clamp((countStockpileZone - 1), -1, 999);
            }
        }
        if (countStockpileZone < 0)
        {
            hasStockpileZone = false;
        }
        else if (countStockpileZone >= 0)
        {
            hasStockpileZone = true;
        }

        countWidgetInStockpile = widgetList.Count - 1;
        for (var i = widgetList.Count - 1; i > -1; i--)
        {
            if (widgetList[i] == null)
            {
                widgetList.RemoveAt(i);
                countWidgetInStockpile -= 1;
            }

        }
        if (countWidgetInStockpile < 0)
        {
            hasWidgetInStockpile = false;
        }
        else if (countWidgetInStockpile >= 0)
        {
            hasWidgetInStockpile = true;

        }
    }         

}
