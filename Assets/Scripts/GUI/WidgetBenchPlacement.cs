using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WidgetBenchPlacement : MonoBehaviour {

    //public GameObject widgetBenchBtnObj;
    public Button widgetBenchBtn;

    void Awake()
    {
        widgetBenchBtn.onClick.AddListener(WidgetBenchBtnAction);
    }

    public void WidgetBenchBtnAction()
    {
        if (GameStats.dollars >= 300)
        {
            Inputs.placingWidgetBench = true;
        }
    }
}
