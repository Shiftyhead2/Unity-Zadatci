using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    Pathfinder pathfinding;

    bool isProcessingPath;

    static PathRequestManager instance;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinder>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[],bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart,currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path,bool success)
    {
        currentPathRequest.callBack(path,success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[],bool> callBack;

        public PathRequest(Vector3 _start, Vector3 _end ,Action<Vector3[],bool> _callBack)
        {
            pathStart = _start;
            pathEnd = _end;
            callBack = _callBack;
        }
    }
}
