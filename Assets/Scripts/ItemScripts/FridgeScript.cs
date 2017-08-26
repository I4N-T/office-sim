using UnityEngine;
using System.Collections;

public class FridgeScript : MonoBehaviour {

    public Vector2 fridgePos;

	void Start () {

        GameStats.hasFridge = true;
        fridgePos = gameObject.transform.position;

        //ADD THIS ITEM TO THE FRIDGE ARRAY IN GAMESTATS
        GameStats.countFridge++;
        GameStats.fridgeList.Add(this.gameObject);
	
	}
	
	
	void Update () {

        fridgePos = gameObject.transform.position;

    }

    //DELETE OBJECT
    void OnMouseDown()
    {
        print("click happened");
        if (DeleteScript.isDelete)
        {
            Destroy(gameObject);
            DeleteScript.isDelete = false;
        }
    }
}
