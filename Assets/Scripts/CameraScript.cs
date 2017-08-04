using UnityEngine;
using System.Collections;


public class CameraScript : MonoBehaviour
{

    GameObject theobj;
    TileMeshScript tileMeshScript;
    BoxCollider2D boxCollider;

    float colliderX;
    float colliderY;

    void Awake()
    {
        theobj = GameObject.Find("GameManager");
        tileMeshScript = theobj.GetComponent<TileMeshScript>();

        //box collider
        //boxCollider = this.GetComponent<BoxCollider2D>();
        colliderX = 13.25f;
        colliderY = 10f;
    }

    void Update()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");

        if (d > 0)
        {
            if (Camera.main.orthographicSize > 4f)
            {
                Camera.main.orthographicSize -= 1f;
                colliderX = Camera.main.orthographicSize * 2.65f;
                colliderY = Camera.main.orthographicSize * 2f;
                //boxCollider.size = new Vector3(colliderX, colliderY, 0);
            }
        }

        else if (d < 0)
        {
            if (Camera.main.orthographicSize < (tileMeshScript.squareSize / 2))
            {
                Camera.main.orthographicSize += 1f;
                colliderX = Camera.main.orthographicSize * 2.65f;
                colliderY = Camera.main.orthographicSize * 2f;
                //boxCollider.size = new Vector3(colliderX, colliderY, 0);
            }
        }

        //pan with WASD
        if (Input.GetKey("a"))
        {
            Camera.main.transform.Translate(new Vector3(-0.15f, 0, 0));
        }
        if (Input.GetKey("w"))
        {
            Camera.main.transform.Translate(new Vector3(0, 0.15f, 0));
        }
        if (Input.GetKey("d"))
        {
            Camera.main.transform.Translate(new Vector3(0.15f, 0, 0));
        }
        if (Input.GetKey("s"))
        {
            Camera.main.transform.Translate(new Vector3(0, -0.15f, 0));
        }
    }
}
