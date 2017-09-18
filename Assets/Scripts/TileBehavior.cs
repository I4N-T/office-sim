using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBehavior : MonoBehaviour {


    SpriteRenderer rend;
    public static float rendAngle;


    //ITEM SPRITES FOR PLACEMENT - consider using Dict instead of array
    public Sprite[] highlightSpriteArray;
    public Sprite groundSprite;

    //ZONE SPRITES - consider using Dict instead of array
    public Sprite[] zoneSpriteArray;

    //STORE CONTENTS OF TILE
    public GameObject itemOnTile;

    public GameObject gameManagerObject;
    public Inputs inputsScript;

    //public bool isSimOnTile;

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

    }

    //METHODS
    void RotateSprite()
    {
        if (Input.GetKeyDown("r"))
        {
            rend.transform.Rotate(Vector3.forward * -90);
            rendAngle -= 90f;
            //this is necessary for the conditionaly statements in the widget instantiation logic of the widgetbenchscript
            if (rendAngle == -90f)
            {
                rendAngle = 270f;
            }       
        }
        if (Input.GetKeyDown("e"))
        {
            rend.transform.Rotate(Vector3.forward * 90);
            rendAngle += 90f;
            //this is necessary for the conditionaly statements in the widget instantiation logic of the widgetbenchscript
            if (rendAngle == 360f)
            {
                rendAngle = 0;
            }
        }
        rend.transform.rotation = Quaternion.AngleAxis(rendAngle, Vector3.forward);
    }
	
    //MOUSE EVENTS
	void OnMouseOver()
    {
        if (Inputs.placingWidgetBench == true)
        {
            rend.sprite = highlightSpriteArray[1];
            rend.sortingOrder = 1;
            RotateSprite();
            
        }
        else if (Inputs.placingFridge == true)
        {
            rend.sprite = highlightSpriteArray[2];
            rend.sortingOrder = 1;
            RotateSprite();
        }
        else if (Inputs.placingCoffeeMachine == true)
        {
            rend.sprite = highlightSpriteArray[3];
            rend.sortingOrder = 1;
            RotateSprite();
        }
        else if (Inputs.placingBathroomStall == true)
        {
            rend.sprite = highlightSpriteArray[4];
            rend.sortingOrder = 1;
            RotateSprite();
        }
        else if (Inputs.placingSalesBench == true)
        {
            rend.sprite = highlightSpriteArray[5];
            rend.sortingOrder = 1;
            RotateSprite();
        }
        else if (Inputs.placingDraftingDesk == true)
        {
            rend.sprite = highlightSpriteArray[6];
            rend.sortingOrder = 1;
            RotateSprite();
        }
    }

    void OnMouseExit()
    {
        rend.sortingOrder = 0;
        rend.sprite = groundSprite;
    }


    //COLLISIONS AND TRIGGERS
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag != "Sim" && col.gameObject.tag != "StockpileZone" && itemOnTile == null)
        {
            itemOnTile = col.gameObject;
        }
        if (col.gameObject.tag == "StockpileZone")
        {
            isStockpileZone = true;
        }

        //keep up with widgets in stockpile
       /* if (isStockpileZone)
        {
            if (itemOnTile != null)
            {
                if (itemOnTile.tag == "Widget")
                {
                    if (!isSimOnTile)
                    {
                        GameStats.countWidgetInStockpile++;
                    }
                    
                }
            }
        }*/
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Sim")
        {
            //only set itemOnTile to null IF the item exiting the trigger is the item that was stored in itemOnTile
            if (itemOnTile != null)
            {
                if (itemOnTile.GetInstanceID() == col.gameObject.GetInstanceID())
                {
                    itemOnTile = null;
                }
            }  
        }
        if (col.gameObject.tag == "StockpileZone")
        {
            isStockpileZone = false;
        }
    }
}
