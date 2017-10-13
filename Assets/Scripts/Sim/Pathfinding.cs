using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

    //each sim has personal list of nodes that is a copy of Gamestats global list of nodes
    //use tile list to get positions for node list
    //when placing an object, update which tiles are isWalkable in the global list of nodes
    //when sim needs to pathfind, set the simNodeList = Gamestats.nodeList (instead of doing it constantly in Update())

    SimStats simStatsScript;

    //ADJACENT NODES
    Vector3 topLeft;
    Vector3 top;
    Vector3 topRight;
    Vector3 right;
    Vector3 bottomRight;
    Vector3 bottom;
    Vector3 bottomLeft;
    Vector3 left;

    List<Node> simNodeList = new List<Node>();
    List<Node> openNodeList;

    bool hasBeenStarted;
    bool isGoing;
    bool isTimeToWait;
    Vector3 targetNode = new Vector3(0, 0, 0);

    bool isAbsStartSet;
    Vector3 absStartNodePos = new Vector3(0, 0, 0);

    float moveSpeed = 2f;

    void Start()
    {
        //simNodeList = GameStats.nodeList;
        isGoing = true;
        simStatsScript = gameObject.GetComponent<SimStats>();
    }

    void Update()
    {
        

    }

    public void GoTowardAStar(Vector3 targetPos)
    {
        if (!hasBeenStarted)
        {
            print("diddly");

            StartCoroutine(GoTowardAStarCR(targetPos));
            hasBeenStarted = true;
        }

        if (isGoing && hasBeenStarted)
        {
            GoToward(targetNode);
        }
        
    }

    public IEnumerator GoTowardAStarCR(Vector3 targetPos)
    {
        //SET ABSOLUTE START POSITION
        if (!isAbsStartSet)
        {
            Vector3 absStartNodePos = new Vector3(Mathf.Round(simStatsScript.simPos.x), Mathf.Round(simStatsScript.simPos.y), 0);
            isAbsStartSet = true;
        }
       

        //MAKE THIS ROUND TO NEAREST NODE POSITION
        Vector3 startNodePos = new Vector3(Mathf.Round(simStatsScript.simPos.x), Mathf.Round(simStatsScript.simPos.y), 0);

        topLeft = new Vector3(startNodePos.x - 1f, startNodePos.y + 1f, 0);
        top = new Vector3(startNodePos.x, startNodePos.y + 1f, 0);
        topRight = new Vector3(startNodePos.x + 1f, startNodePos.y + 1f, 0);
        right = new Vector3(startNodePos.x + 1f, startNodePos.y, 0);
        bottomRight = new Vector3(startNodePos.x + 1f, startNodePos.y - 1f, 0);
        bottom = new Vector3(startNodePos.x, startNodePos.y - 1f, 0);
        bottomLeft = new Vector3(startNodePos.x - 1f, startNodePos.y - 1f, 0);
        left = new Vector3(startNodePos.x - 1f, startNodePos.y, 0);

        //CREATE OPEN NODE LIST (list of adjacent walkable nodes)
        foreach (Node node in GameStats.nodeList)
        {
            //if node contains no obstacles
            if (node.isWalkable)
            {
                //if node is not closed
                if (node.state != Node.NodeState.Closed)
                {

                    //and if node is adjacent to sim position
                    if (/*node.location == (Vector3)simStatsScript.simPos || */node.location == topLeft || node.location == top || node.location == topRight ||
                        node.location == right || node.location == bottomRight || node.location == bottom || node.location == bottomLeft || node.location == left)
                    {
                        //make an IF NODE NOT ALREADY IN LIST
                        if (!simNodeList.Contains(node))
                        {
                            //print(node.location);
                            simNodeList.Add(node);
                        }
                    }
                    
                }
            }
        }

        //DETERMINE F G and H VALUES OF EACH OPEN NODE
        foreach (Node node in simNodeList)
        {
            if (node.location == startNodePos)
            {
                node.state = Node.NodeState.Closed;
            }
            /*if (node.location != topLeft || node.location != top || node.location != topRight ||
                node.location != right || node.location != bottomRight || node.location != bottom || node.location != bottomLeft || node.location != left)
            {
                node.state = Node.NodeState.Closed;
            }
            else if (node.location == topLeft || node.location == top || node.location == topRight ||
                        node.location == right || node.location == bottomRight || node.location == bottom || node.location == bottomLeft || node.location == left)
            {
                node.state = Node.NodeState.Open;
            }*/

                /*if (node.location == top || node.location == right || node.location == bottom || node.location == left)
                {
                    node.G += 10;
                }
                else if (node.location == topLeft || node.location == topRight || node.location == bottomRight || node.location == bottomLeft)
                {
                    node.G += 14;
                }*/

            node.G = Mathf.Abs(Vector3.Distance(absStartNodePos, node.location));

            //add condition to only set H if it has not yet been set?
            node.H = Mathf.Abs(Vector3.Distance(targetPos, node.location));

            node.F = node.G + node.H;
        }

        //Vector3 targetNode;

        float lowestF = 9999;
        
        Vector3 lowestFPos;
        foreach (Node node in simNodeList)
        {
            if(node.state == Node.NodeState.Open)
            {
                if (node.F <= lowestF)
                {
                    //print(node.location);
                    lowestF = node.F;

                    print("F is: " + node.F);
                    lowestFPos = node.location;
                    targetNode = lowestFPos;
                }

                /*if (node.location == (Vector3)simStatsScript.simPos)
                {
                    node.state = Node.NodeState.Closed;

                }*/
                //print(node.G);
            }

        }
        print("ITS BONKERS BITCH");
        //Coroutine goCoroutine = StartCoroutine(GoToward(targetNode));
        //isGoing = true;

        //if (isGoing)
        //{
        //GoToward(targetNode);
        //isGoing = true;
        //Coroutine goCoroutine = StartCoroutine(GoTowardCR(targetNode));
        isGoing = true;
            yield return new WaitForSeconds(1f);
        isGoing = false;
        // StopCoroutine(goCoroutine);
        //}
        print("test");
        //StopCoroutine(goCoroutine);

        hasBeenStarted = false;
        yield return null;
        

        
    }

    /*public bool IsGoing(Vector3 targetNode)
    {
        if ((Vector3)simStatsScript.simPos == targetNode)
        {
            return false;
        }
        else
        {
            return true;
        }
    }*/

    public IEnumerator GoTowardCR(Vector2 targetPos)
    {
        if (isGoing)
        {
            print("going");
            transform.position = Vector3.MoveTowards(new Vector3(simStatsScript.simPos.x, simStatsScript.simPos.y, -1f), new Vector3(targetPos.x, targetPos.y, -1f), moveSpeed * Time.deltaTime);
            
            //yield return new WaitForSeconds(3f);
        }
        else if (!isGoing)
        {
            yield return null;
        }
        
    }

    public void GoToward(Vector2 targetPos)
    {
        //print("going");
        transform.position = Vector3.MoveTowards(new Vector3(simStatsScript.simPos.x, simStatsScript.simPos.y, -1f), new Vector3(targetPos.x, targetPos.y, -1f), moveSpeed * Time.deltaTime);

        /*if (new Vector3(simStatsScript.simPos.x, simStatsScript.simPos.y, -1f) == new Vector3(targetPos.x, targetPos.y, -1f))
        {
            //print("rodallo");
            isGoing = false;
        }*/
    }
}
