using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BathroomStallPlacementScript : MonoBehaviour {

    public Button bathroomStallBtn;

    void Awake()
    {
        bathroomStallBtn.onClick.AddListener(BathroomStallBtnAction);
    }

    public void BathroomStallBtnAction()
    {
        if (GameStats.dollars >= 300)
        {
            Inputs.placingBathroomStall = true;
        }
    }
}
