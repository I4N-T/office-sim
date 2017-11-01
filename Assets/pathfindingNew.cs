using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class pathfindingNew : MonoBehaviour {

    //SCRIPT REFERENCES
    SimStats simStatsScript;
    SimAI simAIScript;

    //LISTS
    public List<Node> openNodeList = new List<Node>();
    public List<Node> closedNodeList = new List<Node>();

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
    Vector3 loc; //cab be local?

    //BOOLS
    bool isStartPosSet;
    public bool isRunned;
    public bool isTakePathTime;
    bool isCreatedPath;
    public bool isGoing;
    public bool isStillPathing;

    Coroutine MoveIE;

    List<Vector3> pathList = new List<Vector3>();

    void Start()
    {
        simStatsScript = gameObject.GetComponent<SimStats>();
        simAIScript = gameObject.GetComponent<SimAI>();
    }

    void Update()
    {
        //print("takepath? " + isTakePathTime);
       
    }

	public void AStarPathMethod(Vector3 targetPos)
    {    
        if (!isRunned)
        {

            if (!isStartPosSet)
            {
                
                startNodePos = new Vector3(Mathf.Round(simStatsScript.simPos.x), Mathf.Round(simStatsScript.simPos.y), 0);
                //print("start position: " + startNodePos);
                //print("target position: " + targetPos);
                Node startNode = new Node(startNodePos);
                openNodeList.Add(startNode);
                //currentNode = startNode;
                isStartPosSet = true;
            }
            //http://www.policyalmanac.org/games/aStarTutorial.htm
            //a
            ChooseNextNode();
            //b
            //print("current node: " + currentNode.location);
            /*print("current F: " + currentNode.F);
            print("current G: " + currentNode.G);
            print("current H: " + currentNode.H);*/

            openNodeList.Remove(currentNode);
            closedNodeList.Add(currentNode);

            //if targetPos is on closedNodeList (path has been found)
            int index = closedNodeList.FindIndex(Node => Node.location == targetPos);
            //print(index);
            if (index >= 0)
            {
                //isCreatedPath = false;
                isRunned = true;
                isStartPosSet = false;
                isCreatedPath = false;
                pathList.Clear();
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
                        CreateAddAndSetParentNode(node, targetPos, 1f);
                    }
                    else if (node.location == topLeft || node.location == topRight || node.location == bottomRight || node.location == bottomLeft) ///NEED TO REFACTOR THIS UNWIELDLY MESS
                    {
                        //DO NOT ADD NODE IF IT CUTS CORNER
                        if (node.location == topLeft)
                        {
                            //if node to the left or node above currentNode is unwalkable - do not add node
                            Node nodeAboveCurrentNode = null;
                            Node nodeLeftCurrentNode = null;

                            Vector3 nodeAboveCurrentNodePos = new Vector3(currentNode.location.x, currentNode.location.y + 1f, 0);
                            Vector3 nodeLeftCurrentNodePos = new Vector3(currentNode.location.x - 1f, currentNode.location.y, 0);
                            foreach (Node nodeI in GameStats.nodeList)
                            {
                                if (nodeI.location == nodeAboveCurrentNodePos)
                                {
                                    nodeAboveCurrentNode = nodeI;
                                }
                                else if (nodeI.location == nodeLeftCurrentNodePos)
                                {
                                    nodeLeftCurrentNode = nodeI;
                                }
                            }
                            //if (nodeAboveMostRecentNode != null)
                            //{
                            if (nodeAboveCurrentNode.isWalkable && nodeLeftCurrentNode.isWalkable)
                            {
                                CreateAddAndSetParentNode(node, targetPos, 1.4f);
                            }

                        }
                        else if (node.location == topRight)
                        {
                            //if node to the right or node above currentNode is unwalkable - do not add node
                            Node nodeAboveCurrentNode = null;
                            Node nodeRightCurrentNode = null;

                            Vector3 nodeAboveCurrentNodePos = new Vector3(currentNode.location.x, currentNode.location.y + 1f, 0);
                            Vector3 nodeRightCurrentNodePos = new Vector3(currentNode.location.x + 1f, currentNode.location.y, 0);
                            foreach (Node nodeI in GameStats.nodeList)
                            {
                                if (nodeI.location == nodeAboveCurrentNodePos)
                                {
                                    nodeAboveCurrentNode = nodeI;
                                }
                                else if (nodeI.location == nodeRightCurrentNodePos)
                                {
                                    nodeRightCurrentNode = nodeI;
                                }
                            }
                            //if (nodeAboveMostRecentNode != null)
                            //{
                            if (nodeAboveCurrentNode.isWalkable && nodeRightCurrentNode.isWalkable)
                            {
                                CreateAddAndSetParentNode(node, targetPos, 1.4f);
                            }
                        }
                        else if (node.location == bottomRight)
                        {
                            //if node to the right or node below currentNode is unwalkable - do not add node
                            Node nodeBelowCurrentNode = null;
                            Node nodeRightCurrentNode = null;

                            Vector3 nodeBelowCurrentNodePos = new Vector3(currentNode.location.x, currentNode.location.y - 1f, 0);
                            Vector3 nodeRightCurrentNodePos = new Vector3(currentNode.location.x + 1f, currentNode.location.y, 0);
                            foreach (Node nodeI in GameStats.nodeList)
                            {
                                if (nodeI.location == nodeBelowCurrentNodePos)
                                {
                                    nodeBelowCurrentNode = nodeI;
                                }
                                else if (nodeI.location == nodeRightCurrentNodePos)
                                {
                                    nodeRightCurrentNode = nodeI;
                                }
                            }
                            //if (nodeAboveMostRecentNode != null)
                            //{
                            if (nodeBelowCurrentNode.isWalkable && nodeRightCurrentNode.isWalkable)
                            {
                                CreateAddAndSetParentNode(node, targetPos, 1.4f);
                            }
                        }
                        else if (node.location == bottomLeft)
                        {
                            //if node to the left or node below currentNode is unwalkable - do not add node
                            Node nodeBelowCurrentNode = null;
                            Node nodeLeftCurrentNode = null;

                            Vector3 nodeBelowCurrentNodePos = new Vector3(currentNode.location.x, currentNode.location.y - 1f, 0);
                            Vector3 nodeLeftCurrentNodePos = new Vector3(currentNode.location.x - 1f, currentNode.location.y, 0);
                            foreach (Node nodeI in GameStats.nodeList)
                            {
                                if (nodeI.location == nodeBelowCurrentNodePos)
                                {
                                    nodeBelowCurrentNode = nodeI;
                                }
                                else if (nodeI.location == nodeLeftCurrentNodePos)
                                {
                                    nodeLeftCurrentNode = nodeI;
                                }
                            }
                            //if (nodeAboveMostRecentNode != null)
                            //{
                            if (nodeBelowCurrentNode.isWalkable && nodeLeftCurrentNode.isWalkable)
                            {
                                CreateAddAndSetParentNode(node, targetPos, 1.4f);
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
        Node mostRecentNode = null;
        if (closedNodeList.Count > 0)
        {
            mostRecentNode = closedNodeList[closedNodeList.Count - 1];
        }
        
        
        float lowestF = 9999f;
        foreach (Node node in openNodeList)
        {
            if (node.F <= lowestF)
            {
                lowestF = node.F;
                currentNode = node;
                //DELETE THIS COMMENTED OUT STUFF AFTER SUFFICIENT TESTING
                //if node does not cut corner -> currentNode = node
                /*if (mostRecentNode != null)
                {
                    //if upright diag move
                    if (node.location.x == mostRecentNode.location.x + 1f && node.location.y == mostRecentNode.location.y + 1f)
                    {
                        Node nodeAboveMostRecentNode = null;
                        Node nodeRightMostRecentNode = null;

                        Vector3 nodeAboveMostRecentNodePos = new Vector3(mostRecentNode.location.x, mostRecentNode.location.y + 1f, 0);
                        Vector3 nodeRightMostRecentNodePos = new Vector3(mostRecentNode.location.x + 1f, mostRecentNode.location.y, 0);
                        foreach (Node nodeI in GameStats.nodeList)
                        {
                            if (nodeI.location == nodeAboveMostRecentNodePos)
                            {
                                nodeAboveMostRecentNode = nodeI;
                                print("nodeI: " + nodeI.isWalkable);
                                print("nodeAbove: " + nodeAboveMostRecentNode.isWalkable);
                            }
                            else if (nodeI.location == nodeRightMostRecentNodePos)
                            {
                                nodeRightMostRecentNode = nodeI;
                            }
                        }
                        //if (nodeAboveMostRecentNode != null)
                        //{
                        if (nodeAboveMostRecentNode.isWalkable && nodeRightMostRecentNode.isWalkable)
                        {
                            lowestF = node.F;
                            currentNode = node;
                        }
                        //}            
                    }
                    //if downright diag move
                    else if (node.location.x == mostRecentNode.location.x + 1 && node.location.y == mostRecentNode.location.y - 1)
                    {
                        Node nodeBelowMostRecentNode = null;
                        Node nodeRightMostRecentNode = null;

                        Vector3 nodeBelowMostRecentNodePos = new Vector3(mostRecentNode.location.x, mostRecentNode.location.y - 1f, 0);
                        Vector3 nodeRightMostRecentNodePos = new Vector3(mostRecentNode.location.x + 1f, mostRecentNode.location.y, 0);
                        foreach (Node nodeI in GameStats.nodeList)
                        {
                            if (nodeI.location == nodeBelowMostRecentNodePos)
                            {
                                nodeBelowMostRecentNode = nodeI;
                            }
                            else if (nodeI.location == nodeRightMostRecentNodePos)
                            {
                                nodeRightMostRecentNode = nodeI;
                            }
                        }
                        //if (nodeAboveMostRecentNode != null)
                        //{
                        if (nodeBelowMostRecentNode.isWalkable && nodeRightMostRecentNode.isWalkable)
                        {
                            lowestF = node.F;
                            currentNode = node;
                        }
                        //}
                    }
                    //if downleft diag move
                    else if (node.location.x == mostRecentNode.location.x - 1 && node.location.y == mostRecentNode.location.y - 1)
                    {
                        Node nodeBelowMostRecentNode = null;
                        Node nodeLeftMostRecentNode = null;

                        Vector3 nodeBelowMostRecentNodePos = new Vector3(mostRecentNode.location.x, mostRecentNode.location.y - 1f, 0);
                        Vector3 nodeLeftMostRecentNodePos = new Vector3(mostRecentNode.location.x - 1f, mostRecentNode.location.y, 0);
                        foreach (Node nodeI in GameStats.nodeList)
                        {
                            if (nodeI.location == nodeBelowMostRecentNodePos)
                            {
                                nodeBelowMostRecentNode = nodeI;
                            }
                            else if (nodeI.location == nodeLeftMostRecentNodePos)
                            {
                                nodeLeftMostRecentNode = nodeI;
                            }
                        }
                        //if (nodeAboveMostRecentNode != null)
                        //{
                        if (nodeBelowMostRecentNode.isWalkable && nodeLeftMostRecentNode.isWalkable)
                        {
                            lowestF = node.F;
                            currentNode = node;
                        }
                        //}
                    }
                    //if upleft diag move
                    else if (node.location.x == mostRecentNode.location.x - 1 && node.location.y == mostRecentNode.location.y + 1)
                    {
                        Node nodeAboveMostRecentNode = null;
                        Node nodeLeftMostRecentNode = null;

                        Vector3 nodeAboveMostRecentNodePos = new Vector3(mostRecentNode.location.x, mostRecentNode.location.y + 1f, 0);
                        Vector3 nodeLeftMostRecentNodePos = new Vector3(mostRecentNode.location.x - 1f, mostRecentNode.location.y, 0);
                        foreach (Node nodeI in GameStats.nodeList)
                        {
                            if (nodeI.location == nodeAboveMostRecentNodePos)
                            {
                                nodeAboveMostRecentNode = nodeI;
                            }
                            else if (nodeI.location == nodeLeftMostRecentNodePos)
                            {
                                nodeLeftMostRecentNode = nodeI;
                            }
                        }
                        //if (nodeAboveMostRecentNode != null)
                        //{
                        if (nodeAboveMostRecentNode.isWalkable && nodeLeftMostRecentNode.isWalkable)
                        {
                            lowestF = node.F;
                            currentNode = node;
                        }
                        //}     
                    }
                    else
                    {
                        lowestF = node.F;
                        currentNode = node;
                    }
                }
                else if (mostRecentNode == null)
                {
                    lowestF = node.F;
                    currentNode = node;
                }   */                       
            }
        }      
    }

    void CreateAddAndSetParentNode(Node node, Vector3 targetPos, float movementCost)
    {
        //create new node with position at this node and parent node of w/e was passed into AddAdjacentNodes
        Node newNode = new Node(node.location, currentNode);

        newNode.G = currentNode.G + movementCost;
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
            float Gnew = currentNode.G + movementCost;
            //change parent node but not if it cuts corners?
            if (Gnew < openNodeList[index].G)
            {
                openNodeList[index].parentNode = currentNode;
            }
        }
    }

    public IEnumerator TakePath(Vector3 targetPos, string itemToTarget)
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

                //This next stuff is to ensure that sim will change direction if another sim reaches the target first
                simAIScript.GetTargetPos(itemToTarget);
                if (simAIScript.targetPos != (Vector2)targetPos)
                {
                    print("ok then");
                    isStillPathing = false;
                    isTakePathTime = false;
                    isGoing = false;
                    isRunned = false;
                    openNodeList.Clear();
                    closedNodeList.Clear();
                    yield break;
                }

            }
        }
        //!!!By this point, target destination has been reached!!! 
        //clear lists so a new path can be created
        openNodeList.Clear();
        closedNodeList.Clear();

        //the isGoing bool is used in SimFSM script to keep the sim going toward targetPos even while it is there already
        //print("State: " + simAIScript.simFSMScript.taskState);
        isGoing = true;
        isStillPathing = false;
        isRunned = false;
        

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
