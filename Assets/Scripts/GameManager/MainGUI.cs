using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainGUI : MonoBehaviour {

    //RESOURCE TEXT
    public Text dollarsText;

    //UI BUTTONS
    //BUILD
    public GameObject buildBtnObj;
    public Button buildBtn;

    public GameObject needsBtnObj;
    public Button needsBtn;

    //production
    public GameObject productionBtnObj;
    public Button productionBtn;
    public GameObject productionPanelObj;
    
    public GameObject widgetBenchBtnObj;
    public Button widgetBenchBtn;

    public GameObject salesBtnObj;
    public Button salesBtn;

    //ZONE
    public Button zoneBtn;

    void Awake()
    {
        //BUTTON LISTENERS
        buildBtn.onClick.AddListener(BuildBtnAction);
        productionBtn.onClick.AddListener(ProductionBtnAction);
        //widgetBenchBtn.onClick.AddListener(WidgetBenchBtnAction);

        needsBtnObj.SetActive(false);

        productionBtnObj.SetActive(false);
        productionPanelObj.SetActive(false);

        salesBtnObj.SetActive(false);

    }

	// Use this for initialization
	void Start () {


        
	}
	
	// Update is called once per frame
	void Update () {

        //RESOURCES
        dollarsText.text = "Dollars: $" + GameStats.dollars;

        //UI MENU

        
	}

    void BuildBtnAction()
    {
        needsBtnObj.SetActive(true);
        productionBtnObj.SetActive(true);
        salesBtnObj.SetActive(true);
    }

    void ProductionBtnAction()
    {
        productionPanelObj.SetActive(true);
    }

    
}
