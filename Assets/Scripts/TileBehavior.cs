using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBehavior : MonoBehaviour {


    SpriteRenderer rend;


    //ITEM SPRITES FOR PLACEMENT - consider using Dict instead of array
    public Sprite[] highlightSpriteArray;
    public Sprite groundSprite;

    //ZONE SPRITES - consider using Dict instead of array
    public Sprite[] zoneSpriteArray;

    //STORE CONTENTS OF TILE
    public GameObject itemOnTile;

    public GameObject gameManagerObject;
    public Inputs inputsScript;

    //WHAT TYPE OF ZONE IS THIS TILE
    public bool isStockpileZoneHighlight;
    public bool isStockpileZone;

    public GameObject stockpileZoneObj;

    // Use this for initialization
    void Start () {

        rend = gameObject.GetComponent<SpriteRenderer>();
        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        inputsScript = gameManagerObject.GetComponent<Inputs>();
	
	}

    void Update()
    {
        //CHANGE SPRITE FOR STOCKPILE TILES
        /*if (Inputs.drawingZoneStockpile)
        {
            if (inputsScript.zoneRect.Contains(gameObject.transform.position))
            {
                rend.sprite = zoneSpriteArray[0];
                //isStockpileZone = true;
            }
            else if (!inputsScript.zoneRect.Contains(gameObject.transform.position))
            {
                rend.sprite = groundSprite;
            }
        }
        
        else if (!Inputs.drawingZoneStockpile)
        {
            if (inputsScript.zoneRect.Contains(gameObject.transform.position))
            {
                isStockpileZone = true;
            }
        }   */

        //INSTANTIATE STOCKPILE ZONE OBJECT ON TOP OF TILES
        if (Inputs.drawingZoneStockpile)
        {
            if (inputsScript.zoneRect.Contains(gameObject.transform.position))
            {
                //rend.sprite = zoneSpriteArray[0];
                //isStockpileZone = true;
            }
            else if (!inputsScript.zoneRect.Contains(gameObject.transform.position))
            {
                rend.sprite = groundSprite;
            }
        }

        else if (!Inputs.drawingZoneStockpile && !isStockpileZone)
        {
            if (inputsScript.zoneRect.Contains(gameObject.transform.position))
            {
                isStockpileZone = true;
                //Instantiate(stockpileZoneObj, gameObject.transform.position, Quaternion.identity);
            }
        }



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
    }

    void OnMouseExit()
    {
        //if (!isStockpileZone)
        //{
            rend.sortingOrder = 0;
            rend.sprite = groundSprite;
       // }
        
       // if (isStockpileZone)
       // {
           // rend.sprite = zoneSpriteArray[0];
        //}
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
