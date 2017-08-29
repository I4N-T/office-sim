using UnityEngine;
using System.Collections;

public class ZoneBehavior : MonoBehaviour {

    
    public SpriteRenderer render;
    public RectTransform rectTrans;
    public Rect rectangle;

    public GameObject gameManagerObject;
    public Inputs inputsScript;

    public bool isBeingCreated;


    void Start()
    {
        //GameStats.countStockpileZone++;
        GameStats.stockpileZoneList.Add(this.gameObject);

        isBeingCreated = true;

        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        inputsScript = gameManagerObject.GetComponent<Inputs>();

        render = gameObject.GetComponent<SpriteRenderer>();
        rectTrans = gameObject.GetComponent<RectTransform>();
        rectangle = rectTrans.rect;
    }

    void Update()
    {
        DrawZoneOnPlacement();
    }

    void DrawZoneOnPlacement()
    {
        if (Inputs.drawingZoneStockpile)
        {
            if (!isBeingCreated)
            {
                return;
            }
            else if (isBeingCreated)
            {
                rectTrans.position = new Vector3((inputsScript.zoneRect.min.x - 0.5f) + (inputsScript.zoneRect.width / 2f), (inputsScript.zoneRect.min.y - 0.5f) + (inputsScript.zoneRect.height / 2f), -1);

                //the subtraction by (.1, .1) insures that outlying tiles do not have the isStockpileZone bool checked
                if (inputsScript.zoneRect.size.x >= 0 && inputsScript.zoneRect.size.y >= 0)
                {
                    rectTrans.localScale = inputsScript.zoneRect.size - new Vector2(.1f, .1f);
                }
                else if (inputsScript.zoneRect.size.x >= 0 && inputsScript.zoneRect.size.y < 0)
                {
                    rectTrans.localScale = inputsScript.zoneRect.size - new Vector2(.1f, -.1f);
                }
                else if (inputsScript.zoneRect.size.x < 0 && inputsScript.zoneRect.size.y >= 0)
                {
                    rectTrans.localScale = inputsScript.zoneRect.size - new Vector2(-.1f, .1f);
                }
                else if (inputsScript.zoneRect.size.x < 0 && inputsScript.zoneRect.size.y < 0)
                {
                    rectTrans.localScale = inputsScript.zoneRect.size - new Vector2(-.1f, -.1f);
                }


            }
        }
        else if (!Inputs.drawingZoneStockpile)
        {
            isBeingCreated = false;
            //GameStats.hasStockpileZone = true;
      
        }
    }

    //DELETE ZONE
    void OnMouseDown()
    {
        //print("click happened");
        if (DeleteScript.isDelete)
        {
            Destroy(gameObject);
            DeleteScript.isDelete = false;
        }
    }

   


}
