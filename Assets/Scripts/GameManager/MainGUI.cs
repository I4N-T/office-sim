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

    //needs
    public GameObject needsBtnObj;
    public Button needsBtn;
    public GameObject needsPanelObj;

    public GameObject fridgeBtnObj;
    public Button fridgeBtn;

    //production
    public GameObject productionBtnObj;
    public Button productionBtn;
    public GameObject productionPanelObj;
    
    public GameObject widgetBenchBtnObj;
    public Button widgetBenchBtn;

    public GameObject salesBtnObj;
    public Button salesBtn;

    //ZONE
    public GameObject zoneBtnObj;
    public Button zoneBtn;

    //stockpile
    public GameObject stockpileBtnObj;
    public Button stockpileBtn;

    void Awake()
    {
        //BUTTON LISTENERS
        buildBtn.onClick.AddListener(BuildBtnAction);

        needsBtn.onClick.AddListener(NeedsBtnAction);
        productionBtn.onClick.AddListener(ProductionBtnAction);
        //widgetBenchBtn.onClick.AddListener(WidgetBenchBtnAction);

        zoneBtn.onClick.AddListener(ZoneBtnAction);
        //stockpileBtn.onClick.AddListener(StockpileBtnAction);

        needsBtnObj.SetActive(false);
        needsPanelObj.SetActive(false);

        productionBtnObj.SetActive(false);
        productionPanelObj.SetActive(false);

        salesBtnObj.SetActive(false);

        stockpileBtnObj.SetActive(false);

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
        if (!needsBtnObj.activeSelf && !productionBtnObj.activeSelf && !salesBtnObj.activeSelf)
        {
            needsBtnObj.SetActive(true);
            productionBtnObj.SetActive(true);
            salesBtnObj.SetActive(true);

            stockpileBtnObj.SetActive(false);
        }
        else if (needsBtnObj.activeSelf && productionBtnObj.activeSelf && salesBtnObj.activeSelf)
        {
            needsBtnObj.SetActive(false);
            productionBtnObj.SetActive(false);
            salesBtnObj.SetActive(false);

            needsPanelObj.SetActive(false);
            productionPanelObj.SetActive(false);
        }         
    }

    void NeedsBtnAction()
    {
        if (!needsPanelObj.activeSelf)
        {
            needsPanelObj.SetActive(true);

            productionPanelObj.SetActive(false);
        }
        else if (needsPanelObj.activeSelf)
        {
            needsPanelObj.SetActive(false);
            
        }
        
    }

    void ProductionBtnAction()
    {
        if (!productionPanelObj.activeSelf)
        {
            productionPanelObj.SetActive(true);

            needsPanelObj.SetActive(false);
        }
        else if (productionPanelObj.activeSelf)
        {
            productionPanelObj.SetActive(false);
            
        }
    }

    void ZoneBtnAction()
    {
        if (!stockpileBtnObj.activeSelf)
        {
            stockpileBtnObj.SetActive(true);

            needsBtnObj.SetActive(false);
            productionBtnObj.SetActive(false);
            salesBtnObj.SetActive(false);
            needsPanelObj.SetActive(false);
            productionPanelObj.SetActive(false);
        }
        else if (stockpileBtnObj.activeSelf)
        {
            stockpileBtnObj.SetActive(false);    
        }
    }



}
