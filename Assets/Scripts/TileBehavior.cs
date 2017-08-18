using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBehavior : MonoBehaviour {


    SpriteRenderer rend;
    public Sprite[] highlightSpriteArray;
    public Sprite groundSprite;

    //STORE CONTENTS OF TILE
    public GameObject itemOnTile;
    

    // Use this for initialization
    void Start () {

        rend = gameObject.GetComponent<SpriteRenderer>();
	
	}

    void Update()
    {
        
    }

	
	void OnMouseOver()
    {
        if (Inputs.placingWidgetBench == true)
        {
            rend.sprite = highlightSpriteArray[1];
            rend.sortingOrder = 1;
        }
        else if (Inputs.placingFridge == true)
        {
            rend.sprite = highlightSpriteArray[2];
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

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag != "Sim" && itemOnTile == null)
        {
            itemOnTile = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Sim")
        {
            itemOnTile = null;
        }
    }
}
