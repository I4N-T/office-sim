using UnityEngine;
using System.Collections;

public class TileGenerator : MonoBehaviour {

    public int squareSize;
    public GameObject tile;
    public Transform tileHolderTransform;
    GameObject tileForList;

    public static bool haveBeenGenerated;

	void Start ()
    {
        Generate(squareSize);
	}

    void Generate(int squareSize)
    {
        int tileCount = squareSize * squareSize;

        for (int y = 0; y < squareSize; y++)
        {
            for (int x = 0; x < squareSize; x++)
            {
                tileForList = Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity, tileHolderTransform) as GameObject;
                GameStats.tileList.Add(tileForList);
            }
        }

        haveBeenGenerated = true;
    }


}
