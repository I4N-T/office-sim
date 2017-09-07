using UnityEngine;
using System.Collections;

public class CancelCursorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        UpdatePosition();

        if (!DeleteScript.isDelete)
        {
            Destroy(gameObject);
        }
	}

    void UpdatePosition()
    {
        if (DeleteScript.isDelete)
        {
            gameObject.transform.position = new Vector3(MousePosition.mouseposition.x, MousePosition.mouseposition.y, -1);
        }
        
    }
}
