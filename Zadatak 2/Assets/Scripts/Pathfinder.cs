using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Grid grid;

    public Transform seeker,target;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            FindPath(seeker.position,target.position);
        }
    }

    void FindPath(Vector3 startPosition,Vector3 targetPosition)
    {
        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node endNode = grid.NodeFromWorldPoint(targetPosition);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            if(currentNode == endNode)
            {
                RetracePath(startNode,endNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeightbour = currentNode.gCost + GetDistance(currentNode,neighbour);
                if(newMovementCostToNeightbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeightbour;
                    neighbour.hCost = GetDistance(neighbour,endNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {   
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(dstX > dstY)
        {
            return 14*dstY + 10*(dstX-dstY);
        }
        return 14*dstX + 10*(dstY - dstX);
    }
}
