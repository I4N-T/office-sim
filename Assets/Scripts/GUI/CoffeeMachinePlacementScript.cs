using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoffeeMachinePlacementScript : MonoBehaviour {

    public Button coffeeMachineBtn;

    void Awake()
    {
        coffeeMachineBtn.onClick.AddListener(CoffeeMachineBtnAction);
    }

    public void CoffeeMachineBtnAction()
    {
        if (GameStats.dollars >= 100)
        {
            Inputs.placingCoffeeMachine = true;
        }
    }
}
