using UnityEngine;
using System.Collections;

public class CoffeeScript : MonoBehaviour {

    public Vector2 coffeeMachinePos;

    void Start()
    {

        GameStats.hasCoffeeMachine = true;
        coffeeMachinePos = gameObject.transform.position;

        //ADD THIS ITEM TO THE FRIDGE ARRAY IN GAMESTATS
        GameStats.countCoffeeMachine++;
        GameStats.coffeeMachineList.Add(this.gameObject);

    }


    void Update()
    {

        coffeeMachinePos = gameObject.transform.position;

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
