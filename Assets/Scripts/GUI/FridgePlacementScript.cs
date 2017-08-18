using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FridgePlacementScript : MonoBehaviour {

    
    public Button fridgeBtn;

    void Awake()
    {
        fridgeBtn.onClick.AddListener(FridgeBtnAction);
    }

    public void FridgeBtnAction()
    {
        if (GameStats.dollars >= 300)
        {
            Inputs.placingFridge = true;
        }
    }
}
