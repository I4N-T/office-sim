using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloseButtonScript : MonoBehaviour {

    public Button closeBtn;
    public GameObject simWindowCanvas;

    public SimWindowTextManager simWindowTxtManagerScript;
    public GameObject simObj;
    public SimManager simManagerScript;

    void Awake()
    {
        closeBtn.onClick.AddListener(CloseBtnAction);

        
    }

    void Start()
    {
        simWindowTxtManagerScript = transform.parent.GetComponent<SimWindowTextManager>();
        simObj = simWindowTxtManagerScript.simObj;
        simManagerScript = simObj.GetComponent<SimManager>();
    }

    public void CloseBtnAction()
    {

        //set simManager.simwindowcanvasobject to null so that simmanager script can instantiate it again when sim selected
        simManagerScript.simWindowCanvasObj = null;
        simManagerScript.isSimSelected = false;
        simManagerScript.hasRunDisable = false;

        Destroy(simWindowCanvas);
        
    }
}
