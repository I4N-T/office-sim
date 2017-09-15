using UnityEngine;
using System.Collections;

public class BathroomStallScript : MonoBehaviour {

    public Vector2 bathroomStallPos;

    void Start()
    {

        GameStats.hasBathroomStall = true;
        bathroomStallPos = gameObject.transform.position;

        //ADD THIS ITEM TO THE FRIDGE ARRAY IN GAMESTATS
        GameStats.countBathroomStall++;
        GameStats.bathroomStallList.Add(this.gameObject);

    }


    void Update()
    {

        bathroomStallPos = gameObject.transform.position;

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
