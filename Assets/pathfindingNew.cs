using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class pathfindingNew : MonoBehaviour {

    //SCRIPT REFERENCES
    SimStats simStatsScript;
    SimAI simAIScript;

    //LISTS
    List<Node> openNodeList = new List<Node>();
    List<Node> closedNodeList = new List<Node>();

    //OTHER FIELDS
    Vector3 startNodePos;
    Vector3 currentNodePos;

    Node currentNode;
    Node chosenNode;

    //ADJACENT NODES
    Vector3 topLeft;
    Vector3 top;
    Vector3 topRight;
    Vector3 right;
    Vector3 bottomRight;
    Vector3 bottom;
    Vector3 bottomLeft;
    Vector3 left;
    Vector3 loc;

    //BOOLS
    bool isStartPosSet;
    bool isRunned;
    public bool isTakePathTime;
    bool isCreatedPath;
    public bool isGoing;

    Coroutine MoveIE;

    List<Vector3> pathList = new List<Vector3>();

    void Start()
    {
        simStatsScript = gameObject.GetComponent<SimStats>();
        simAIScript = gameObject.GetComponent<SimAI>();
    }

    void Update()
    {
        print("takepath? " + isTakePathTime);
       

        //print(isRunned);
        /*if (isRunned)
        {
            isTakePathTime = true;
            /*foreach(Node node in closedNodeList)
            {
                print("closed nodes: " + node.location + " ~~~~ Parent: " + node.parentNode.location);
            }*/
        //}
    }
	public void AStarPathMethod(Vector3 targetPos)
    {    
        if (!isRunned)
        {

            if (!isStartPosSet)
            {
                
                startNodePos = new Vector3(Mathf.Round(simStatsScript.simPos.x), Mathf.Round(simStatsScript.simPos.y), 0);
                print("start position: " + startNodePos);
                print("target position: " + targetPos);
                Node startNode = new Node(startNodePos);
                openNodeList.Add(startNode);
                //currentNode = startNode;
                isStartPosSet = true;
            }
            //http://www.policyalmanac.org/games/aStarTutorial.htm
            //a
            ChooseNextNode();
            //b
            /*print("current node: " + currentNode.location);
            print("current F: " + currentNode.F);
            print("current G: " + currentNode.G);
            print("current H: " + currentNode.H);*/

            openNodeList.Remove(currentNode);
            closedNodeList.Add(currentNode);

            int index = closedNodeList.FindIndex(Node => Node.location == targetPos);
            //print(index);
            if (index >= 0)
            {
                isRunned = true;
                isTakePathTime = true;
            }
            //c
            AddAdjacentNodes(currentNode, targetPos);

            
        }
    }

    void AddAdjacentNodes(Node currentNode, Vector3 targetPos)
    {
        Vector3 currentNodePos = currentNode.location;

        topLeft = new Vector3(currentNodePos.x - 1f, currentNodePos.y + 1f, 0);
        top = new Vector3(currentNodePos.x, currentNodePos.y + 1f, 0);
        topRight = new Vector3(currentNodePos.x + 1f, currentNodePos.y + 1f, 0);
        right = new Vector3(currentNodePos.x + 1f, currentNodePos.y, 0);
        bottomRight = new Vector3(currentNodePos.x + 1f, currentNodePos.y - 1f, 0);
        bottom = new Vector3(currentNodePos.x, currentNodePos.y - 1f, 0);
        bottomLeft = new Vector3(currentNodePos.x - 1f, currentNodePos.y - 1f, 0);
        left = new Vector3(currentNodePos.x - 1f, currentNodePos.y, 0);

        //CREATE OPEN NODE LIST (list of adjacent walkable nodes)
        foreach (Node node in GameStats.nodeList)
        {
            //if node contains no obstacles (but ignore the "unwalkable" target node)
            if (node.isWalkable || (!node.isWalkable && node.location == targetPos))
            {
                //if a node at this position is not already on closed node list
                int index1 = closedNodeList.FindIndex(Node => Node.location == node.location);
                if (index1 < 0)
                {
                                   
                    //and if node is adjacent to sim position
                    if (node.location == top || node.location == right || node.location == bottom || node.location == left)
                    {
                        //create new node with position at this node and parent node of w/e was passed into AddAdjacentNodes
                        Node newNode = new Node(node.location, currentNode);

                        newNode.G = currentNode.G + 1f;
                        newNode.H = Mathf.Abs(Vector3.Distance(targetPos, newNode.location));
                        newNode.F = newNode.G + newNode.H;

                        //if there is not already a node at this location - add to openNodeList
                        int index = openNodeList.FindIndex(Node => Node.location == node.location);
                        if (index < 0)
                        {
                           /* print("new node: " + newNode.location);
                            print("new F: " + newNode.F);
                            print("new G: " + newNode.G);
                            print("new H: " + newNode.H);*/

                            //print("added node: " + node.location);
                            openNodeList.Add(newNode);
                        }
                        else if (index >= 0)
                        {
                            //print("got here");
                            float Gnew = currentNode.G + 1f;
                            if (Gnew < openNodeList[index].G)
                            {
                                openNodeList[index].parentNode = currentNode;
                            }
                        }
                    }
                    else if (node.location == topLeft || node.location == topRight || node.location == bottomRight || node.location == bottomLeft)
                    {
                        //create new node with position at this node and parent node of w/e was passed into AddAdjacentNodes
                        Node newNode = new Node(node.location, currentNode);

                        newNode.G = currentNode.G + 1.4f;
                        newNode.H = Mathf.Abs(Vector3.Distance(targetPos, newNode.location));
                        newNode.F = newNode.G + newNode.H;

                        //if there is not already a node at this location
                        int index = openNodeList.FindIndex(Node => Node.location == node.location);
                        if (index < 0)
                        {
                           /* print("new diag node: " + newNode.location);
                            print("new diag F: " + newNode.F);
                            print("new diag G: " + newNode.G);
                            print("new diag H: " + newNode.H);*/

                            //print(node.location);
                            openNodeList.Add(newNode);
                        }
                        else if (index >= 0)
                        {
                            float Gnew = currentNode.G + 1.4f;
                            if (Gnew < openNodeList[index].G)
                            {
                                openNodeList[index].parentNode = currentNode;
                            }
                        }
                    }

                }
            }
        }
        /*openNodeList.Remove(currentNode);
        closedNodeList.Add(currentNode);*/
    }

    void ChooseNextNode()
    {
        
        float lowestF = 9999f;
        foreach (Node node in openNodeList)
        {
            if (node.F <= lowestF)
            {
                lowestF = node.F;
                //chosenNode = node;
                currentNode = node;
            }
        }      
    }

    public IEnumerator TakePath(Vector3 targetPos)
    {
        //List<Vector3> pathList = new List<Vector3>();
        //pathList.Add(targetPos);
        if (!isCreatedPath)
        {
            Node node = closedNodeList[closedNodeList.Count - 1];
            while (node.parentNode != null)
            {
                pathList.Add(node.location);
                //print("reverse: " + node.location);
                node = node.parentNode;
            }
            pathList.Reverse();
            isCreatedPath = true;
        }

        if (pathList != null)
        {
            foreach (Vector3 location in pathList)
            {
                
                loc = new Vector3(location.x, location.y, -1);
                Vector3 pos = new Vector3(simStatsScript.simPos.x, simStatsScript.simPos.y, 0);
                //print("simPos: " + simStatsScript.simPos);
                // print("location: " + location);

                ////above this is good
                MoveIE = StartCoroutine(GoTowardCR(loc));
                yield return MoveIE;

                while (new Vector3(simStatsScript.simPos.x, simStatsScript.simPos.y, -1) != loc)
                {
                    simAIScript.GoToward(loc);
                }


            }
        }
        //simAIScript.GoToward(pathList[2]);


        //isTakePathTime = false;
        //yield return null;
        isGoing = true;

    }

    public IEnumerator GoTowardCR(Vector3 targetPos)
    {
       while (new Vector3(simStatsScript.simPos.x, simStatsScript.simPos.y, -1) != targetPos)
        {
            simAIScript.GoToward(targetPos);
            yield return null;
        }
    }

 
}
