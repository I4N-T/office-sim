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

    //ITEM/STRUCTURE ARRAYS -- used, for example, to get item position in the SimAI script
    public static List<GameObject> fridgeList = new List<GameObject>();
    public static List<GameObject> widgetBenchList = new List<GameObject>();

    void Awake()
    {
        countFridge = -1;
        countWidgetBench = -1;
    }

	// Use this for initialization
	void Start () {

        dollars = 1000;
        stone = 100;

        hasFridge = false;
        //countFridge = -1;

        hasWidgetBench = false;
	    
	}
	
    void Update()
    {
        print(countFridge);
        for (var i = fridgeList.Count - 1; i > -1; i--)
        {
            if (fridgeList[i] == null)
                fridgeList.RemoveAt(i);
        }

        print(countWidgetBench);
        for (var i = widgetBenchList.Count - 1; i > -1; i--)
        {
            if (widgetBenchList[i] == null)
                widgetBenchList.RemoveAt(i);
        }

    }

}
