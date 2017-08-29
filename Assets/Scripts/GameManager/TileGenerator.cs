using UnityEngine;
using System.Collections;

public class TileGenerator : MonoBehaviour {

    public int squareSize;
    public GameObject tile;
    GameObject tileForList;

	// Use this for initialization
	void Start () {
        Generate(squareSize);
	}

    void Generate(int squareSize)
    {
        int tileCount = squareSize * squareSize;

        for (int y = 0; y < squareSize; y++)
        {
            for (int x = 0; x < squareSize; x++)
            {
                tileForList = Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                GameStats.tileList.Add(tileForList);
            }
        }
    }



}
