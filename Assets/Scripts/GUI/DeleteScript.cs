using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeleteScript : MonoBehaviour {

    public static bool isDelete;
    public Button deleteBtn;

	// Use this for initialization
	void Awake ()
    {
        isDelete = false;
        deleteBtn.onClick.AddListener(DeleteBtnAction);
	 
	}
	
    public void DeleteBtnAction()
    {
        if (!isDelete)
        {
            isDelete = true;
        }
        else if (isDelete)
        {
            isDelete = false;
        }
    }
    
}
