using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TileMeshScript : MonoBehaviour
{

    private Mesh mesh;

    public int squareSize;
    private Vector3[] vertices;
    private Vector2[] uv;

    private int quadCount;
    private int verticesCount;

    // Use this for initialization
    void Start()
    {

        Generate();

    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Tile Map";

        verticesCount = squareSize * squareSize * 6;
        vertices = new Vector3[(squareSize +1) * (squareSize + 1)];
        uv = new Vector2[vertices.Length];

        for (int i = 0, y = 0; y <= squareSize; y++)
        {
            for (int x = 0; x <= squareSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x/squareSize, (float)y/squareSize);
            }
        }
        /*uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(0, 1);
        uv[4] = new Vector2(1, 1);
        uv[5] = new Vector2(2, 1);
        uv[6] = new Vector2(0, 2);
        uv[7] = new Vector2(1, 2);
        uv[8] = new Vector2(2, 2);*/
       
        

        mesh.vertices = vertices;
        mesh.uv = uv;

        int[] triangles = new int[squareSize * squareSize * 6];
        for (int ti = 0, vi = 0, y = 0; y < squareSize; y++, vi++)
        {
            for (int x = 0; x < squareSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + squareSize + 1;
                triangles[ti + 5] = vi + squareSize + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }
}
      

