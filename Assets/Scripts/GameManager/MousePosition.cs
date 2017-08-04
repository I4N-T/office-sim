using UnityEngine;
using System.Collections;

public class MousePosition : MonoBehaviour {

    float mousex;
    float mousey;
    public static Vector3 mouseposition;
	
	
	// Update is called once per frame
	void Update () {

        //print position of mouse
        mousex = (Input.mousePosition.x);
        mousey = (Input.mousePosition.y);
        mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mousex, mousey, 0));
        //print(mouseposition);

        //Debug.Log("(" + mousex + ", " + mousey + ")");
    }
}
