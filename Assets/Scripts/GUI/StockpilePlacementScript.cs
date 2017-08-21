using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StockpilePlacementScript : MonoBehaviour {

    public Button stockpileBtn;

    void Awake()
    {
        stockpileBtn.onClick.AddListener(StockpileBtnAction);
    }

    public void StockpileBtnAction()
    {
        Inputs.drawingZoneStockpile = true;
    }
}
