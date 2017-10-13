using UnityEngine;
using System.Collections;

public class Node /*: MonoBehaviour*/ {

    public Vector3 location;
    public bool isWalkable;
    public float G;
    public float H;
    public float F; //{ get { return this.G + this.H; } }
    public NodeState state;
    public Node parentNode;


    public Node(Vector3 location)
    {
        this.location = location;
        isWalkable = true;
        state = NodeState.Open;
    }

    public Node(Vector3 location, Node parentNode)
    {
        this.location = location;
        this.parentNode = parentNode;
        isWalkable = true;
        state = NodeState.Open;
    }

    public enum NodeState { Untested, Open, Closed }
}
