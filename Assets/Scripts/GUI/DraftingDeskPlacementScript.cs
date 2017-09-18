using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DraftingDeskPlacementScript : MonoBehaviour {

    public Button draftingDeskBtn;

    void Awake()
    {
        draftingDeskBtn.onClick.AddListener(DraftingDeskBtnAction);
    }

    public void DraftingDeskBtnAction()
    {
        if (GameStats.dollars >= 500)
        {
            Inputs.placingDraftingDesk = true;
        }
    }
}
