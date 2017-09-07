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

    //sales
    public GameObject salesBtnObj;
    public Button salesBtn;

    public GameObject salesBenchBtnObj;
    public Button salesBenchBtn;
    public GameObject salesPanelObj;

    //ZONE
    public GameObject zoneBtnObj;
    public Button zoneBtn;

    //stockpile
    public GameObject stockpileBtnObj;
    public Button stockpileBtn;

    //DELETE
    public GameObject deleteBtnObj;
    public Button deleteBtn;

    //DELETE CURSOR (NOT REALLY GUI STUFF)
    GameObject deleteCursor;

    void Awake()
    {
        //BUTTON LISTENERS
        buildBtn.onClick.AddListener(BuildBtnAction);

        needsBtn.onClick.AddListener(NeedsBtnAction);
        productionBtn.onClick.AddListener(ProductionBtnAction);
        salesBtn.onClick.AddListener(SalesBtnAction);
        //widgetBenchBtn.onClick.AddListener(WidgetBenchBtnAction);

        zoneBtn.onClick.AddListener(ZoneBtnAction);
        //stockpileBtn.onClick.AddListener(StockpileBtnAction);

        deleteBtn.onClick.AddListener(DeleteBtnAction);

        //SET CERTAIN BUTTONS INACTIVE
        needsBtnObj.SetActive(false);
        needsPanelObj.SetActive(false);

        productionBtnObj.SetActive(false);
        productionPanelObj.SetActive(false);

        salesBtnObj.SetActive(false);
        salesPanelObj.SetActive(false);

        stockpileBtnObj.SetActive(false);



    }

	// Use this for initialization
	void Start () {
        deleteCursor = (GameObject)Resources.Load("Prefabs/cancelCursor");


    }
	
	// Update is called once per frame
	void Update () {

        //RESOURCES
        dollarsText.text = "Dollars: $" + GameStats.dollars;

        //CLOSE ON RIGHT CLICK
        if (Input.GetMouseButtonUp(1))
        {
            needsBtnObj.SetActive(false);
            productionBtnObj.SetActive(false);
            salesBtnObj.SetActive(false);
            needsPanelObj.SetActive(false);
            productionPanelObj.SetActive(false);
            salesPanelObj.SetActive(false);
            stockpileBtnObj.SetActive(false);
        }

        
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
            salesPanelObj.SetActive(false);
        }         
    }

    void NeedsBtnAction()
    {
        if (!needsPanelObj.activeSelf)
        {
            needsPanelObj.SetActive(true);

            productionPanelObj.SetActive(false);
            salesPanelObj.SetActive(false);
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
            salesPanelObj.SetActive(false);
        }
        else if (productionPanelObj.activeSelf)
        {
            productionPanelObj.SetActive(false);
            
        }
    }

    void SalesBtnAction()
    {
        if (!salesPanelObj.activeSelf)
        {
            salesPanelObj.SetActive(true);

            needsPanelObj.SetActive(false);
            productionPanelObj.SetActive(false);
        }
        else if (salesPanelObj.activeSelf)
        {
            salesPanelObj.SetActive(false);

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
            salesPanelObj.SetActive(false);
        }
        else if (stockpileBtnObj.activeSelf)
        {
            stockpileBtnObj.SetActive(false);    
        }
    }

    void DeleteBtnAction()
    {
        //hide every other button
        needsBtnObj.SetActive(false);
        productionBtnObj.SetActive(false);
        salesBtnObj.SetActive(false);
        needsPanelObj.SetActive(false);
        productionPanelObj.SetActive(false);
        salesPanelObj.SetActive(false);
        stockpileBtnObj.SetActive(false);

        //instantiate red X cursor
        
        Instantiate(deleteCursor, MousePosition.mouseposition, Quaternion.identity);

    }



}
