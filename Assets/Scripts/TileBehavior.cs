using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBehavior : MonoBehaviour {


    SpriteRenderer rend;
    public Sprite[] highlightSpriteArray;
    public Sprite groundSprite;
    

    // Use this for initialization
    void Start () {

        rend = gameObject.GetComponent<SpriteRenderer>();
	
	}


	
	void OnMouseOver()
    {
        if (Inputs.placingWidgetBench == true)
        {
            rend.sprite = highlightSpriteArray[1];
            rend.sortingOrder = 1;
        }

        else
        {
            rend.sprite = highlightSpriteArray[0];
        }
        
    }

    void OnMouseExit()
    {
        //Inputs.placingWidgetBench = false;
        rend.sortingOrder = 0;
        rend.sprite = groundSprite;
    }
}
