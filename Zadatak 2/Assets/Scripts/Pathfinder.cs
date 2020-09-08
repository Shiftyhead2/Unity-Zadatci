using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinder : MonoBehaviour
{
    Grid grid;
    PathRequestManager requestManager;


    void Awake()
    {
        grid = GetComponent<Grid>();
        requestManager = GetComponent<PathRequestManager>();
    }


    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos,endPos));
    }

    IEnumerator FindPath(Vector3 startPosition,Vector3 targetPosition)
    {
        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node endNode = grid.NodeFromWorldPoint(targetPosition);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        if(startNode.walkable && endNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);
            while(openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);
                if(currentNode == endNode)
                {
                    
                    pathSuccess = true;
                    break;
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
        yield return null;

        if(pathSuccess){
            waypoints = RetracePath(startNode,endNode);

        }

        requestManager.FinishedProcessingPath(waypoints,pathSuccess);
    }

    Vector3[] RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplePaths(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplePaths(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
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
