using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStats : MonoBehaviour {

    //RESOURCES AND MONEY
    public static int dollars;
    public static int stone;

    //ITEM/STRUCTURE BOOLEANS and COUNTS
    public static bool hasFridge;
    public static int countFridge;

    public static bool hasWidgetBench;
    public static int countWidgetBench;

    //ZONE BOOLEANS and COUNTS
    public static bool hasStockpileZone;
    public static int countStockpileZone;

    //ITEM/STRUCTURE ARRAYS -- used, for example, to get item position in the SimAI script
    public static List<GameObject> fridgeList = new List<GameObject>();
    public static List<GameObject> widgetBenchList = new List<GameObject>();

    //SIM LIST - it's a list of all sims, what they are doing, and which object they are using
    public static List<GameObject> simList = new List<GameObject>();

    //STOCKPILE ZONE LIST
    public static List<GameObject> stockpileZoneList = new List<GameObject>();

    void Awake()
    {
        countFridge = -1;
        countWidgetBench = -1;
        countStockpileZone = -1;
    }

	// Use this for initialization
	void Start () {

        dollars = 1000;
        stone = 100;

        hasFridge = false;
        hasWidgetBench = false;
        hasStockpileZone = false;
	    
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

        print(countWidgetBench);
        for (var i = widgetBenchList.Count - 1; i > -1; i--)
        {
            if (widgetBenchList[i] == null)
            {
                widgetBenchList.RemoveAt(i);
                countWidgetBench -= 1;
            }
                
        }

        for (var i = stockpileZoneList.Count - 1; i > -1; i--)
        {
            if (stockpileZoneList[i] == null)
            {
                stockpileZoneList.RemoveAt(i);
                countStockpileZone -= 1;
            }
                

        }
        if (countStockpileZone < 0)
        {
            hasStockpileZone = false;
        }

    }

}
