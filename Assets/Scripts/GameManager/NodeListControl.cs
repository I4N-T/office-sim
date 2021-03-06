﻿using UnityEngine;
using System.Collections;

public class NodeListControl : MonoBehaviour {

    public static NodeListControl instance;
    bool hasGeneratedNodes;

    public bool isTimeToCheck;

    void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(instance);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);

        //GenerateNodeList();
    }

    void Update()
    {
        //GenerateNodeList must happen AFTER tiles are generated
        if (TileGenerator.haveBeenGenerated == true && hasGeneratedNodes == false)
        {
            GenerateNodeList();
            hasGeneratedNodes = true;
        }
    }

    

    void GenerateNodeList()
    {
        Vector3 pos;
        Node newNode;
        
        foreach (GameObject tile in GameStats.tileList)
        {
            pos = tile.transform.position;
            newNode = new Node(pos);
        
            GameStats.nodeList.Add(newNode);
        }
    }

    //the point of this being a coroutine is because it must run AFTER the ontriggerstay2d in TileBehavior runs
    public IEnumerator CheckForObstacles()
    {
        yield return null;

        TileBehavior tileBehaviorScript;
        foreach (GameObject tile in GameStats.tileList)
            {   
                //if the tile has an object on it
                tileBehaviorScript = tile.GetComponent<TileBehavior>();
                if (tileBehaviorScript.itemOnTile != null)
                {
                for (int i = 0; i < GameStats.nodeList.Count; i++)
                {
                    if (GameStats.nodeList[i].location == tile.transform.position)
                    {
                        GameStats.nodeList[i].isWalkable = false;
                    }
                }
                }
        }

        isTimeToCheck = false;
    }


}
