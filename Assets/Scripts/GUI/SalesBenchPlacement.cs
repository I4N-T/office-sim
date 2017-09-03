using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SalesBenchPlacement : MonoBehaviour {

    public Button salesBenchBtn;

    void Awake()
    {
        salesBenchBtn.onClick.AddListener(SalesBenchBtnAction);
    }

    public void SalesBenchBtnAction()
    {
        if (GameStats.dollars >= 300)
        {
            Inputs.placingSalesBench = true;
        }
    }
}
